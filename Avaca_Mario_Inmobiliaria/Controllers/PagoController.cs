using Avaca_Mario_Inmobiliaria.Models;
using InmobiliariaAlbornoz.ModelsAux;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Controllers
{
    [Authorize]
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
            if (TempData.ContainsKey("Message") || TempData.ContainsKey("Error"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.Error = TempData["Error"];
            }
            ViewBag.ContratoUnico = false;
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
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Pago pago)
        {  // id viene por ruta pago viene 
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
                        // Armo el pago y consultao si el contrato es valido
                        pago.Id = 0;
                        pago.Id = id;
                     }
                    Contrato c = dataContrato.ObtenerPorId(pago.ContratoId);
                    if (c.Activo)
                    {
                        //var res = repo.Put(pago);

                        var res = dataPago.Put(pago);
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
                    else {

                        TempData["Message"]= "El contrato indicado no esta vigente";
                        return RedirectToAction();
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
            var res = dataPago.ObtenerPorId(id);
            ViewBag.Id = res.Id;
            return View(res);

        }

        // POST: PagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var res = dataPago.Modificacion(pago);
                    if (res > 0)
                    {
                        TempData["Message"] = "Pago editado con exito";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Message"] = "Error en guardar en la base de datos el pago, vuelva a intentar";
                    return View(nameof(Index), new { pago=pago});

                }
                else {
                    ViewBag.Message = "Ha querido ingresar valores no aceptados, vuelva a ingresar";
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error grave comuniquese con el servicio tecnico";
                return View();
            }
        }

        // GET: PagoController/Delete/5
        public ActionResult Delete(int id)
        {
            var res = dataPago.ObtenerPorId(id);
            return View(res);
        }

        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            {
                var res = dataPago.Baja(id);
                if (res>0) {
                    TempData["Message"] = "Pago eliminado con exito";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Pago eliminado sin exito";
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (Exception ex)
            {
                TempData["Error"] =@"Error en Eliminar un pago, llame a servicio tecnico";
                return View();
            }
        }
        // GET: Contratos/pagos/idContrato
        public ActionResult PagosContratos(int id)
        {
            bool vigente = false;
            var returnUrl = Request.Headers["referer"].FirstOrDefault();
            try
            {
                //Controlar que el contrato sea vigente
                // SELECT c.Id FROM contrato c WHERE c.InmuebleId = @Id
               // AND c.Activo = 1 AND getdate() BETWEEN c.FechaInicio AND c.FechaFin
                vigente = dataContrato.ContratoVigente(id);
                if (vigente)
                {
                    var res = dataPago.PagosContratos(id);
                    if (res.Count > 0)
                    {
                        ViewBag.Message = "Lista de Pagos del contrato encontrada";
                        ViewBag.ContratoUnico = true;
                        return View("Index", res);
                    }

                    ViewBag.Message = "No hay pagos del contrato encontrada, puede realizar el primer pago";
                    ViewBag.Contrato = dataContrato.ObtenerPorId(id);
                    return View("Create");
                }
                TempData["Error"] = "El contrato no es vigente no se puede realizar pagos";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData["Error"] = "ERROR en Pagos de un Contrato, llame a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
            }
            
        }
    }
}
