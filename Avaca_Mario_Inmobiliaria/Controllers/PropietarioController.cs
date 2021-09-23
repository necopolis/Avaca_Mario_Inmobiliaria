using Avaca_Mario_Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Controllers
{
    public class PropietarioController : Controller
    {
        protected readonly IConfiguration configuration;
        PropietarioData data;
        public PropietarioController(IConfiguration  configuration)
        {
            this.configuration = configuration;
            data = new PropietarioData(configuration);
        }

        // GET: PropietarioController
        public ActionResult Index()
        {
            var lista = data.ObtenerTodos();
            if (TempData.ContainsKey("Message")|| TempData.ContainsKey("Error"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }

        // GET: PropietarioController/Details/5
        public ActionResult Details(int id)
        {
            Propietario p = data.ObtenerPorId(id);
            return View(p);
        }

        // GET: PropietarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = data.Alta(propietario);
                    if (res>0)
                    {
                        TempData["Message"] = "Propietario Creado Correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = @"No se ha podido Agregar el propietario
                                        Intentelo mas tarde o realice el reclamo a servicio tecnico";
                        return View();
                    }
                    
                }
                else
                {
                    ViewBag.Error = "Los datos no son validos, revise el tipo de informacion que ingresa, he intente nuevamente";
                    return View();
                }
                
                
                
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    TempData["Error"] = @"Esta queriendo agregar un documento duplicado";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = @"No se ha podido Agregar el propietario, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

        // GET: PropietarioController/Edit/5
        public ActionResult Edit(int id)
        {
            Propietario p=data.ObtenerPorId(id);
            return View(p);
        }

        // POST: PropietarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                
                //ModelState.IsValid utiliza la validacion del modelo [Requeride][EmailAddress][etc], etc alguien ingresa un valor incorrecto 
                if (ModelState.IsValid)
                {
                    var res = data.Modificar(id, propietario);
                    if (res>0)
                    {
                        TempData["Message"] = "Propiedad Editada con exito";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = @"No se ha podido Editar el propietario
                                    Intentelo mas tarde o realice el reclamo a servicio tecnico";
                        return View();
                    }
                    
                }
                else
                {
                    ViewBag.Error = "Los datos no son validos, revise el tipo de informacion que ingresa, he intente nuevamente";
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                TempData["Error"] = @"No se ha podido Editar el propietario, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

        // GET: PropietarioController/Delete/5
        public ActionResult Delete(int id)
        {
            Propietario p = data.ObtenerPorId(id);
            return View(p);
        }

        // POST: PropietarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
                bool admin = false;
                var NotPropiedades = false;
                try
                {
                    
                    if (User.IsInRole("Administrador"))
                    {
                        NotPropiedades = data.NoTienePropiedades(id);
                        admin = true;
                    }
                    if (NotPropiedades)
                    {
                        TempData["Error"] = @"Inquilino que quiere eliminar tiene Propiedades";
                        return RedirectToAction(nameof(Index));
                    }
                    var res = data.Baja(id, admin);
                    if (res > 0)
                    {
                        TempData["Message"] = @"Propietario eliminado correctamente";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Error"] = @"No se ha podido eliminar el Propietario, intente nuevamente";
                    return RedirectToAction(nameof(Index));

                }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                //ViewBag.StackTrate = ex.StackTrace;
                TempData["Error"] = @"No se ha podido Eliminar el propietario, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }
    }
}
