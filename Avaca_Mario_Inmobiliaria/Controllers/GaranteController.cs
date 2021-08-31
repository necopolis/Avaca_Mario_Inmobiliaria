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
                var res = data.Alta(garante);
                if (res > 0) { 
                return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "No se ha podido cargar el garante, intente nuevamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                //ViewBag.StackTrate = ex.StackTrace;
                return View();
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
                var res = data.Modificar(id, garante);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                //ViewBag.StackTrate = ex.StackTrace;
                return View();
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
            try
            {
                data.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                //ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }
    }
}
