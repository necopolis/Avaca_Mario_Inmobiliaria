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
    public class InmuebleController : Controller
    {
        protected readonly IConfiguration configuration;
        InmuebleData data;
        PropietarioData dataProp;
        public InmuebleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            data = new InmuebleData(configuration);
            dataProp = new PropietarioData(configuration);
        }
        // GET: InmuebleController
        public ActionResult Index()
        {
            var lista = data.ObtenerTodos();
            return View(lista);
        }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var entidad = data.ObtenerPorId(id);
            return View(entidad);
        }

        // GET: InmuebleController/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = dataProp.ObtenerTodos();
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                var res= data.Alta(inmueble);
                if (res > 0)
                {

                    return RedirectToAction(nameof(Index));
                }
                else {
                    Exception ex = new Exception("No se ha podido guardar el inmuebele intente nuevamente");
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                //ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var res = data.ObtenerPorId(id);
            ViewBag.Propietarios = dataProp.ObtenerTodos();
            return View(res);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                data.Modificacion(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                //ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        // GET: InmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var res = data.ObtenerPorId(id);
            return View(res);
        }

        // POST: InmuebleController/Delete/5
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
