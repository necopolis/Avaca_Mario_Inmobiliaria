using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Avaca_Mario_Inmobiliaria.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Avaca_Mario_Inmobiliaria.Controllers
{
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
                data.Alta(i);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
                data.Modificar(id, inquilino);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            /* Volver a verlo */
            int res = data.Baja(id);
            if (res > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino inqui)
        {
            try
            {
                /* aca las elimino y listo*/
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
