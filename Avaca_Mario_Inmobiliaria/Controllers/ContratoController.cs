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
    public class ContratoController : Controller
    {
        protected readonly IConfiguration configuration;
        ContratoData dataContrato;
        public ContratoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            dataContrato = new ContratoData(configuration);
        }
        // GET: ContratoController
        public ActionResult Index()
        {
            var lista = dataContrato.ObtenerTodos();
            return View(lista);
        }

        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContratoController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                var res = dataContrato.Alta(contrato);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "No se ha podido cargar el contrato, intente nuevamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: ContratoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContratoController/Edit/5
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

        // GET: ContratoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContratoController/Delete/5
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
