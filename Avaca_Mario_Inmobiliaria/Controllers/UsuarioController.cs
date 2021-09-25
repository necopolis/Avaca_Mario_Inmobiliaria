using Avaca_Mario_Inmobiliaria.Models;
using Avaca_Mario_Inmobiliaria.ModelsAux;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        protected readonly IConfiguration configuration;
        UsuarioData dataUsuario;
        private IWebHostEnvironment environment;

        //InmuebleData dataInmueble;
        //GaranteData dataGarante;
        //InquilinoData dataInquilino;
        public UsuarioController(IConfiguration configuration, IWebHostEnvironment enviroment)
        {

            this.configuration = configuration;
            dataUsuario = new UsuarioData(configuration);
            this.environment = enviroment;
            //dataInmueble = new InmuebleData(configuration);
            //dataGarante = new GaranteData(configuration);
            //dataInquilino = new InquilinoData(configuration);
        }
        // GET: Usuarios
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var usuarios = dataUsuario.ObtenerTodos();
            if (TempData.ContainsKey("Message") || TempData.ContainsKey("Error"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.Error = TempData["Error"];
            }
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
            try
            {
                var e = dataUsuario.ObtenerPorId(id);
                return View(e);
            }
            catch (Exception ex)
            {
                TempData["Error"]="ERROR comuniquese con servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
            }
            
        }

        // GET: Usuarios/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario u)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: u.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                u.Clave = hashed;
                //u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;
                var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                int res = dataUsuario.Alta(u);
                if (u.AvatarFile != null && res > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                    dataUsuario.Modificacion(u);
                }
                TempData["Message"] = "Usuario creado con exito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "ERROR comuniquese con servicio tecnico";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Usuarios/Edit/5
        [Authorize]
        public ActionResult Perfil()
        {
            try
            {
                ViewData["Title"] = "Mi perfil";
                var u = dataUsuario.ObtenerPorEmail(User.Identity.Name);
                if (u!=null)
                {
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    return View("Edit", u);
                }
                else
                {
                    TempData["Error"]=@"No se ha podido encontrar el usuario";
                    return View("Edit", u);
                }
                

            }
            catch (Exception e)
            {
                TempData["Error"] = "Error en Perfil, comuniquese con el servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
            }
            
        }

        // GET: Usuarios/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewData["Title"] = "Editar usuario";
                var u = dataUsuario.ObtenerPorId(id);
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View(u);
            }
            catch (Exception)
            {
                TempData["Error"] = "ERROR en Editar, comuniquese con el servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
            }
           
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario u)
        {
            var returnUrl = Request.Headers["referer"].FirstOrDefault();
            bool editAvatar = false;
            bool editRol = false;

            try
            {
                var sessionUser = dataUsuario.ObtenerPorEmail(User.Identity.Name);

                if (u.Id == 0) // significa que viene desde /Usuarios/Perfil y se quiere editar a sí mismo
                {
                    u.Id = sessionUser.Id; //Le asigno al binding el Id de la sesión
                }

                // Ahora busco el user que se intenta editar
                var userToEdit = dataUsuario.ObtenerPorId(u.Id);

                // Chequeo que el usuario exista (medio al pedo)
                if (userToEdit == null)
                {
                    TempData["Error"] = "No se pudo comprobar el usuario. Intente nuevamente.";
                    return RedirectToAction("Denied", "Home");
                }

                // Se chequea que si no es administrador, esté editando su perfil
                if (!User.IsInRole("Administrador"))
                {
                    // Si el NO empleado está intentando editar un user con un id
                    // distinto al propio, o si está intentando mandar un value para
                    // el atributo Rol, es pateado al page "Denied"
                    if (sessionUser.Id != u.Id || u.Rol > 0)
                        return RedirectToAction("Denied", "Home");
                }


                // TODO: Aplicar lógica para modificar usuario
                // Ojo que admin puede modificar Roles. Empleado NO.
                // Empleado solo modifica desde /Usuarios/Perfil

                if (u.Rol > 0)
                {
                    editRol = true; // Bandera para el repo
                }

                if (u.AvatarFile != null)
                {
                    string wwwPath = environment.WebRootPath; // ruta raíz del servidor
                    string pathUploads = Path.Combine(wwwPath, "Uploads");

                    if (!Directory.Exists(pathUploads))
                    {
                        Directory.CreateDirectory(pathUploads);
                    }

                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir

                    string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
                    string avatarFullPath = Path.Combine(pathUploads, fileName);

                    using (FileStream stream = new FileStream(avatarFullPath, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);

                        // Si todo fue bien, revalidamos ruta en el usuario y marcamos bandera
                        u.Avatar = Path.Combine("Uploads", fileName);
                        editAvatar = true;
                    }
                }

                var res = dataUsuario.Update(u, editRol, editAvatar);

                if (res > 0)
                {
                    TempData["Message"] = "Usuario Editado Correctamente";
                    return Redirect(returnUrl);
                }
                else
                {
                    TempData["Error"] = @"ERROR en Editar Usuario, comuniquese con servicio tecnico";
                    return Redirect(returnUrl);
                }


            }
            catch (Exception ex)
            {//colocar breakpoints en la siguiente línea por si algo falla
                TempData["Error"]= @"ERROR en Editar Usuario, comuniquese con servicio tecnico";
                //return RedirectToAction("Index", "Home");
                return RedirectToAction(nameof(Index));
                //throw;
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                var res = dataUsuario.ObtenerPorId(id);
                if (res != null)
                {
                    return View(res);
                }
                else
                {
                    TempData["Error"] = @"Usuario No encontrado";
                    return View();
                }
            }
            catch (Exception e)
            {
                TempData["Error"]=@"No se ha podido acceder a la vista, comuniquese con Servicio Tecnico";
                return RedirectToAction(nameof(Index));
                //throw e;
            }
            
            
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var res = dataUsuario.Baja(id);
                if (res>0)
                {
                    TempData["Message"] =@"Usario Eliminado con exito";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"]=@"No se ha podido eliminar el Usuario, intente nuevamente";
                    return RedirectToAction(nameof(Delete));
                }
                
            }
            catch
            {
                return View();
            }
        }
        [Authorize]
        public IActionResult Avatar()
        {
            var u = dataUsuario.ObtenerPorEmail(User.Identity.Name);
            string fileName = "avatar_" + u.Id + Path.GetExtension(u.Avatar);
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            string pathCompleto = Path.Combine(path, fileName);

            //leer el archivo
            byte[] fileBytes = System.IO.File.ReadAllBytes(pathCompleto);
            //devolverlo
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // GET: Usuarios/Create
        [Authorize]
        public ActionResult Foto()
        {
            try
            {
                var u = dataUsuario.ObtenerPorEmail(User.Identity.Name);
                var stream = System.IO.File.Open(
                    Path.Combine(environment.WebRootPath, u.Avatar.Substring(1)),
                    FileMode.Open,
                    FileAccess.Read);
                var ext = Path.GetExtension(u.Avatar);
                return new FileStreamResult(stream, $"image/{ext.Substring(1)}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Usuarios/Create
        [Authorize]
        public ActionResult Datos()
        {
            try
            {
                var u = dataUsuario.ObtenerPorEmail(User.Identity.Name);
                string buffer = "Nombre;Apellido;Email" + Environment.NewLine +
                    $"{u.Nombre};{u.Apellido};{u.Email}";
                var stream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(buffer));
                var res = new FileStreamResult(stream, "text/plain");
                res.FileDownloadName = "Datos.csv";
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult LoginModal()
        {
            return PartialView("_LoginModal", new LoginView());
        }

        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) {
                return RedirectToAction("Index", "Home");
            }
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Usuarios/Login/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = dataUsuario.ObtenerPorEmail(login.Usuario);
                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        

        // GET: Usuario/EditPass/{id}
        [Authorize]
        public ActionResult EditPass(int id) {
            try
            {
                var sessionUser = dataUsuario.ObtenerPorEmail(User.Identity.Name);
                if (!User.IsInRole("Administrador") && id != sessionUser.Id)
                {
                    return RedirectToAction("Restringido", "Home");
                } 
                
                var userToEdit = dataUsuario.ObtenerPorId(id);
                if (userToEdit != null)
                {
                    ViewBag.Usuario = userToEdit;
                    ViewBag.Message ="Ingreso a la vista";
                    return View();
                }
                else
                {
                    TempData["Error"]=@"No se encontro el usuario";
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
            }
            catch (Exception e)
            {
                TempData["Error"]=@"Error grave comuniquese con Servicio tecnico";
                return View();
                //throw;
            }
        }
        // POST: Usuario/EditPass/{id}
        [HttpPost]
        [Authorize]
        public ActionResult EditPass(int id, UsuarioPassEdit p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sessionUser = dataUsuario.ObtenerPorEmail(User.Identity.Name);
                    var userToEdit = dataUsuario.ObtenerPorId(id);
                    if (userToEdit == null) 
                    {
                        // El usuario a editar no existe
                        TempData["Error"] =@"No se encontro el susuario";
                        return Redirect(Request.Headers["referer"].FirstOrDefault());
                    }
                    // Si es empleado y toco para querer cambiar otro usuario, no lo dejamos
                    if (!User.IsInRole("Administrador") && sessionUser.Id != id)
                    {
                        return RedirectToAction("Restringido", "Home");
                    }
                    // Todo listo para proceder
                    // 
                    string PassViejaHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: p.PassVieja,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    // Controlando que la contraseña anterior sea igual a la que esta en BD
                    if (PassViejaHashed!=userToEdit.Clave)
                    {
                        TempData["Error"]=@"La contraseña ingresada es incorrecta";
                        return RedirectToAction(nameof(EditPass), new { id=id});
                    }
                    string PassNuevaHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: p.NuevaPass,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    var res = dataUsuario.UpdatePass(id, PassNuevaHashed);
                    if (res > 0)
                    {
                        ViewBag.Message= "Contraseña actualizada correctamente";

                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "No se pudo actualizar correctamente, vuelva a intentar";
                        ViewBag.Usuario = userToEdit;
                        return View(); ;
                    }
                }
                
                return View();
            }
            catch (Exception)
            {
                return View();
                //throw;
            }
        }
    }
}
