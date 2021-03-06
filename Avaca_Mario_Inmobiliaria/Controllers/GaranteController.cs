using Avaca_Mario_Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Controllers
{
    [Authorize]
    public class GaranteController : Controller
    {
        protected readonly IConfiguration configuration;
        GaranteData data;
        public GaranteController(IConfiguration configuration)
        {
            this.configuration = configuration;
            data = new GaranteData(configuration);
        }
        // GET: GaranteController
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

        // GET: GaranteController/Details/5
        public ActionResult Details(int id)
        {
            var res = data.ObtenerPorId(id);
            return View(res);
        }

        // GET: GaranteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GaranteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Garante garante)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = data.Alta(garante);
                    if (res > 0)
                    {
                        TempData["Message"] = "Garante Creado Correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = "No se ha podido cargar el garante, intente nuevamente";
                        return RedirectToAction(nameof(Index));
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

        // GET: GaranteController/Edit/5
        public ActionResult Edit(int id)
        {
            var res = data.ObtenerPorId(id);
            return View(res);
        }

        // POST: GaranteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Garante garante)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = data.Modificar(id, garante);
                    if (res > 0)
                    {
                        TempData["Message"] = "Garante Creado Correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = @"No se ha podido Agregar el Garante
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
                TempData["Error"] = @"No se ha podido Agregar el Garante, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

        // GET: GaranteController/Delete/5
        public ActionResult Delete(int id)
        {
            var res = data.ObtenerPorId(id);
            return View(res);
        }

        // POST: GaranteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            bool admin = false;
            var TieneContrato = false;
            try
            {

                if (User.IsInRole("Administrador"))
                {
                    TieneContrato = data.TieneContrato(id);
                    admin = true;
                }
                if (TieneContrato)
                {
                    TempData["Error"] = @"Garante que quiere eliminar tiene contratos";
                    return RedirectToAction(nameof(Index));
                }
                var res = data.Baja(id, admin);
                if (res > 0)
                {
                    TempData["Message"] = @"Garante eliminado correctamente";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = @"No se ha podido eliminar el Garante, intente nuevamente";
                return RedirectToAction(nameof(Index));

                /* aca las elimino y listo*/

            }
            catch (Exception ex)
            {
                TempData["Error"] = @"No se ha podido Eliminar el Garante, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }
    }
}
