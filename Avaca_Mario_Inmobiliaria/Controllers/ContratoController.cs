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
            if (TempData.ContainsKey("Message") || TempData.ContainsKey("Error"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.Error = TempData["Error"];
            }
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
            ViewBag.Inmuebles =dataInmueble.ObtenerTodosValidos();
            ViewBag.Garantes = dataGarante.ObtenerTodosActivos();
            ViewBag.Inquilinos = dataInquilino.ObtenerTodosActivos();
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fechasCorrectas = dataContrato.fechasCorrectas(contrato.InmuebleId, contrato.FechaInicio, contrato.FechaFin);
                    var inmuebleleIsOk = dataInmueble.InmubleHabilitado(contrato.InmuebleId);
                    var inquilinoIsOk = dataInquilino.ObtenerPorId(contrato.InquilinoId).Id > 0 ? true : false;

                    if (fechasCorrectas) {

                        if (inmuebleleIsOk && inquilinoIsOk && fechasCorrectas)
                        {
                            var res = dataContrato.Alta(contrato);
                            if (res > 0)
                            {
                                TempData["Message"] = "Contrato Creado Correctamente";
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                ViewBag.Error = "No se ha podido cargar el contrato, intente nuevamente";
                                return RedirectToAction(nameof(Create));
                            }
                        }
                        else {

                            return View();
                        }
                    }
                    else {
                        TempData["Error"] = "En las fecha que desea alquilar no se puede, busque otras";
                        return RedirectToAction(nameof(Create));
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
                TempData["Error"] = @"No se ha podido Agregar el Contrato, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
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
                if (ModelState.IsValid)
                {
                    var res = dataContrato.Modificacion(contrato);
                    if (res > 0)
                    {
                        TempData["Message"] = "Contrato Editado Correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = "No se pudo dar de baja intente nuevamente";
                        return View(res);
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
                TempData["Error"] = @"No se ha podido Editar el Contrato, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
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
                TempData["Error"] = @"No se ha podido Agregar el Inquilino, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

        

    }
}
