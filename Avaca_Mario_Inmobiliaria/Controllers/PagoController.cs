using Avaca_Mario_Inmobiliaria.Models;
using InmobiliariaAlbornoz.ModelsAux;
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
        ContratoData dataContrato;
        InquilinoData dataInquilino;

        public PagoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            dataPago = new PagoData(configuration);
            dataContrato = new ContratoData(configuration);
            dataInquilino = new InquilinoData(configuration);
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
        public ActionResult Create(int id)
        {
            if (id > 0) { 

                Contrato c = dataContrato.ObtenerPorId(id);
                if (c.Id > 0) {
                    ViewBag.Contrato = c;
                    return View();
                }
            }
            return View();
            
        }

        // GET: PagosController/Inquilino/{dni}
        public ActionResult Inquilino(string dni)
        {

            PagoCreate pc = new PagoCreate();
            try
            {
                if (dni != null)
                {
                    Inquilino i = dataInquilino.ObtenerPorDni(dni);
                    IList<Contrato> c = dataContrato.AllByInquilino(i.Id);

                    pc.Inquilino = i;
                    pc.Contratos = c;
                }

                return Ok(pc); // { "Inquilino": { "Nombre":"Pepito", ... }, "Contratos":[{"IdINmueble": 8}, {}, {}] }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Viene por formulario
                    if (id > 0 && pago.Id==0)
                    {
                        // Rescarta id del contrato y volver a chequear que realmente este vigente
                        //var vigente = dataContrato.algo(id);
                        // Si contrato esta vigente, Enviar p al repo.
                        pago.Id = 0;
                        pago.Id = id;
                        //var res = data.Put(p);
                        return RedirectToAction(nameof(Index));
                    }
                    else {

                        //var res= data.Put(p);
                        return RedirectToAction(nameof(Index));

                    }
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
