using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Avaca_Mario_Inmobiliaria.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;

namespace Avaca_Mario_Inmobiliaria.Controllers
{
    [Authorize]
    public class InquilinoController : Controller
    {
        protected readonly IConfiguration configuration;
        InquilinoData data;

        public InquilinoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            data = new InquilinoData(configuration);
        }

        // GET: InquilinoController
        public ActionResult Index()
        {
            var lista = data.ObtenerTodos();
            if (TempData.ContainsKey("Message") || TempData.ContainsKey("Error"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }

        // GET: InquilinoController/Details/5
        public ActionResult Details(int id)
        {
            Inquilino inquilino = data.ObtenerPorId(id);
            return View(inquilino);
        }

        // GET: InquilinoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = data.Alta(i); 
                    if (res>0)
                    {
                        TempData["Message"] = "Inquilino Creado Correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = @"No se ha podido Agregar el Inquilino 
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
            catch (SqlException e)
            {

                if (e.Number == 2627)
                {
                    TempData["Error"] = "El numero de documento que quiere ingresar esta duplicado";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = @"No se ha podido Agregar el Inquilino, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

        // GET: InquilinoController/Edit/5
        public ActionResult Edit(int id)
        {
            Inquilino inquilino = data.ObtenerPorId(id);
            return View(inquilino);
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = data.Modificar(id, inquilino);
                    if (res > 0)
                    {
                        TempData["Message"] = "Inquilino Editada con exito";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = @"No se ha podido Agregar el Inquilino
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
            catch (SqlException e)
            {

                if (e.Number == 2627)
                {
                    TempData["Error"] = "El numero de documento que quiere ingresar esta duplicado";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = @"No se ha podido Agregar el Inquilino, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var res = data.ObtenerPorId(id);
                if (res != null)
                {
                    return View(res);
                    //TempData["Message"] = @"Inquilino Eliminado correctamente";
                    //return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Control por si cliente toca
                    TempData["Error"] = @"Inquilino a borrar no existe";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = @"Error grave comuniquese con servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
            }
            /* Volver a verlo */
            
        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino inqui)
        {
            bool admin = false;
            var TieneContrato = false;
            try
            {
                
                if (User.IsInRole("Administrador") )
                {
                    TieneContrato = data.TieneContrato(id);
                    admin = true;
                }
                if(TieneContrato)
                {
                    TempData["Error"] = @"Inquilino que quiere eliminar tiene contratos";
                    return RedirectToAction(nameof(Index));
                }
                var res = data.Baja(id, admin);
                if (res>0)
                {
                    TempData["Message"]=@"Inquilino eliminado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                
                TempData["Error"] = @"No se ha podido eliminar el inquilino, intente nuevamente";
                return RedirectToAction(nameof(Index));

                /* aca las elimino y listo*/

            }
            catch (Exception ex)
            {
                TempData["Error"] = @"No se ha podido Eliminar el Inquilino, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

    }
}
