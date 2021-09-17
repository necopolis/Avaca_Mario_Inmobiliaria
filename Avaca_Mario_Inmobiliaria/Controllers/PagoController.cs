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
    public class PagoController : Controller
    {
        protected readonly IConfiguration configuration;
        PagoData dataPago;

        public PagoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            dataPago = new PagoData(configuration);
        }
            // GET: PagoController
            public ActionResult Index()
        {
            var pagos = dataPago.ObtenerTodos();
            return View(pagos);
        }

        // GET: PagoController/Details/5
        public ActionResult Details(int id)
        {
            var pago = dataPago.ObtenerPorId(id);
            return View(pago);
        }

        // GET: PagoController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = dataPago.Alta(pago);
                    if (res > 0)
                    {
                        TempData["Message"] = "Pago Creado Correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = @"No se ha podido Agregar el Pago 
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
                TempData["Error"] = @"No se ha podido Agregar el Pago, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

        // GET: PagoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PagoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
