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
        InmuebleData dataInmueble;
        GaranteData dataGarante;
        InquilinoData dataInquilino;
        public ContratoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            dataContrato = new ContratoData(configuration);
            dataInmueble = new InmuebleData(configuration);
            dataGarante = new GaranteData(configuration);
            dataInquilino = new InquilinoData(configuration);
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
            try
            {
                var res = dataContrato.ObtenerPorId(id);
                return View(res);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
                //throw;
            }
            
        }

        // GET: ContratoController/Create
        public ActionResult Create()
        {
            ViewBag.Inmuebles =dataInmueble.ObtenerTodos();
            ViewBag.Garantes = dataGarante.ObtenerTodos();
            ViewBag.Inquilinos = dataInquilino.ObtenerTodos();
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
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: ContratoController/Edit/5
        public ActionResult Edit(int id)
        {
            var res = dataContrato.ObtenerPorId(id);
            ViewBag.Inmuebles = dataInmueble.ObtenerTodos();
            ViewBag.Garantes = dataGarante.ObtenerTodos();
            ViewBag.Inquilinos = dataInquilino.ObtenerTodos();
            return View(res);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                var res = dataContrato.Modificacion(contrato);
                if (res>0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "No se pudo dar de baja intente nuevamente";
                    return View(res);
                }
                
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: ContratoController/Delete/5
        public ActionResult Delete(int id)
        {
            var res = dataContrato.ObtenerPorId(id);
            return View(res);
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var res = dataContrato.Baja(id);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else {
                    ViewBag.Error = "Algo ha salido mal no se ha podido eliminar";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
