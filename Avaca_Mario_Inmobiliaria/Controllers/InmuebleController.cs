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
                TempData["Error"] = @"No se ha podido Agregar el Inmueble, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var res = dataInm.ObtenerPorId(id);
            ViewBag.Propietarios = dataProp.ObtenerTodosActivos();
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            //var isTaken = dataInm.isTaken(id);
            //if (isTaken)
            //{

            //}
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
                        TempData["Message"] = "Inmueble Creado Correctamente";
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
                TempData["Error"] = @"No se ha podido Editar el Inmueble, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
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
            var NotienePropietario = false;
            var isToken = false;
            try
            {

                if (User.IsInRole("Administrador"))
                {
                    NotienePropietario = dataInm.NoTienePropietario(id);
                    isToken = dataInm.NoTieneContrato(id);
                    admin = true;
                }
                if (NotienePropietario || isToken)
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
                TempData["Error"] = @"No se ha podido Eliminar el Inquilino, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
                //return Json(new { Error = ex.Message });
            }
        }
        // GET: Inmuebles/ListaInmuebles/4
        public ActionResult ListaInmuebles(int id)
        {
            //var returnUrl = Request.Headers["referer"].FirstOrDefault(); ;
            try
            {
                var res = dataInm.ListaInmPropietario(id);
                if (res != null)
                {
                    ViewBag.Message = @"Lista de Inmubeles encontrados";
                    //ViewBag.returnUrl = returnUrl;
                    return View(res);
                }
                ViewBag.Error = "No hay inmuebles para este propietario";
                return View();
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error grave comuniquese con el servicio tecnico";
                return RedirectToAction(nameof(Index));
                //throw;
            }
        }

    }
}
