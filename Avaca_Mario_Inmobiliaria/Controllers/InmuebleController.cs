using Avaca_Mario_Inmobiliaria.Models;
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
    public class InmuebleController : Controller
    {
        protected readonly IConfiguration configuration;
        InmuebleData dataInm;
        PropietarioData dataProp;
        public InmuebleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            dataInm = new InmuebleData(configuration);
            dataProp = new PropietarioData(configuration);
        }
        // GET: InmuebleController
        public ActionResult Index()
        {
            var lista = dataInm.ObtenerTodos();
            if (TempData.ContainsKey("Message") || TempData.ContainsKey("Error"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var entidad = dataInm.ObtenerPorId(id);
            return View(entidad);
        }

        // GET: InmuebleController/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = dataProp.ObtenerTodosActivos();
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            if (ViewBag.Propietarios.Count==0)
            {
                TempData["Error"] = "No hay en base de datos Propietarios para cargar un inmueble";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //TODO Controlar que los id de propietario exista
                    var res = dataInm.Alta(inmueble);
                    if (res > 0)
                    {
                        TempData["Message"] = "Inmueble Creado Correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = @"No se ha podido Agregar el Inmuebles 
                                        Intentelo mas tarde o realice el reclamo a servicio tecnico";
                        ViewBag.Propietarios = dataProp.ObtenerTodosActivos();
                        ViewBag.Usos = Inmueble.ObtenerUsos();
                        ViewBag.Tipos = Inmueble.ObtenerTipos();
                        return View();
                    }

                }else
                {
                    ViewBag.Error = "Los datos no son validos, revise el tipo de informacion que ingresa, he intente nuevamente";
                    ViewBag.Propietarios = dataProp.ObtenerTodosActivos();
                    ViewBag.Usos = Inmueble.ObtenerUsos();
                    ViewBag.Tipos = Inmueble.ObtenerTipos();
                    return View();
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = @"ERROR al Agregar el Inmueble, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var tieneContrato = dataInm.NoTieneContrato(id);
            var res = dataInm.ObtenerPorId(id);
            if (tieneContrato)
            {
                ViewBag.Message = "Inmueble con contrato solo se puede modificar Precio y Cantidad de ambientes";
                ViewBag.TieneContrato =tieneContrato;
                return View(res);
            }
            ViewBag.Propietarios = dataProp.ObtenerTodosActivos();
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            // TODO: Tendria que revisar si tiene contrato y si lo tiene solo editar el precio
            //var isTaken = dataInm.isTaken(id);
            //if (isTaken)
            //{

            //}

            ViewBag.TieneContrato =tieneContrato;
            return View(res);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                        var res = dataInm.Modificacion(inmueble);
                        if (res > 0)
                        {
                            TempData["Message"] = "Inmueble Actualizado Correctamente";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ViewBag.Error = @"No se ha podido Agregar el Inmueble
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
                TempData["Error"] = @"ERROR al Editar el Inmueble, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

        // GET: InmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var res = dataInm.ObtenerPorId(id);
                return View(res);
            }
            catch (Exception ex)
            {
                TempData["Error"]="Error grave comuniquese con el serivicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
            }
            
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            bool admin = false;
            var tienePropietario = false;
            var isToken = false;
            try
            {

                if (User.IsInRole("Administrador"))
                {
                    tienePropietario = dataInm.TienePropietario(id);
                    isToken = dataInm.NoTieneContrato(id);
                    admin = true;
                }
                if (tienePropietario || isToken)
                {
                    TempData["Error"] = @"El Inmueble que quiere eliminar tiene contratos o esta alquilado";
                    return RedirectToAction(nameof(Index));
                }
                var res = dataInm.Baja(id, admin);
                if (res > 0)
                {
                    TempData["Message"] = @"Inmueble eliminado correctamente";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = @"No se ha podido eliminar el Inmueble, intente nuevamente";
                return RedirectToAction(nameof(Index));

                /* aca las elimino y listo*/

            }
            catch (Exception ex)
            {
                TempData["Error"] = @"ERROR al Eliminar el Inquilino, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }
        // GET: Inmuebles/ListaInmuebles/4
        public ActionResult ListaInmuebles(int id)
        {
            var returnUrl = Request.Headers["referer"].FirstOrDefault(); ;
            try
            {
                var res = dataInm.ListaInmPropietario(id);
                if (res.Count>0)
                {
                    ViewBag.Message = @"Lista de Inmubeles encontrados";
                    //Podria usar la vista de index de inmuebles
                    ViewBag.returnUrl = returnUrl;
                    ViewBag.Bandera = true;
                    return View("Index",res);
                }
                TempData["Error"] = "No hay inmuebles para este propietario";
                return RedirectToAction("Index","Propietario");
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error comuniquese con el servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
            }
        }
        // GET: Inmuebles/Activos
        public ActionResult Activos()
        {
            string returnUrl = Request.Headers["referer"].FirstOrDefault();
            try
            {
                var res = dataInm.ObtenerTodosActivos();
                if (res.Count > 0)
                {
                    ViewBag.returnUrl = returnUrl;
                    return View(nameof(Index), res);
                }
                TempData["Error"] = "No hay Inmuebles Activos";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No se pudo realizar la tarea, ERROR comuniquese con el servicio tecnico";
                return View();
                //throw;
            }
        }
    }
}
