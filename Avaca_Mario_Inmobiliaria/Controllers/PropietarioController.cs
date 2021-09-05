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
    public class PropietarioController : Controller
    {
        protected readonly IConfiguration configuration;
        PropietarioData data;
        public PropietarioController(IConfiguration  configuration)
        {
            this.configuration = configuration;
            data = new PropietarioData(configuration);
        }

        // GET: PropietarioController
        public ActionResult Index()
        {
            var lista = data.ObtenerTodos();
           ViewBag.Message = TempData["Message"];
            return View(lista);
        }

        // GET: PropietarioController/Details/5
        public ActionResult Details(int id)
        {
            Propietario p = data.ObtenerPorId(id);
            return View(p);
        }

        // GET: PropietarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                var res = data.Alta(propietario);
                if ((res > 0) && (ModelState.IsValid))
                {
                    TempData["Message"] = "TEMPO DATA Logrado con exito";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = @"No se ha podido Agregar el propietario" 
                                    + propietario.Apellido + @"Intentelo mas tarde o realice el reclamo a servicio tecnico";
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.Error = @"No se ha podido Agregar el propietario, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return View();
            }
        }

        // GET: PropietarioController/Edit/5
        public ActionResult Edit(int id)
        {
            Propietario p=data.ObtenerPorId(id);
            return View(p);
        }

        // POST: PropietarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                var res = data.Modificar(id, propietario);
                if ((res > 0) && (ModelState.IsValid))
                {
                    TempData["Message"] = "Logrado con exito";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = @"No se ha podido Editar el propietario"
                                    + propietario.Apellido + "Intentelo mas tarde o realice el reclamo a servicio tecnico";
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                //ViewBag.StackTrate = ex.StackTrace;
                ViewBag.Error = @"No se ha podido Editar el propietario, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return View();
            }
        }

        // GET: PropietarioController/Delete/5
        public ActionResult Delete(int id)
        {
            Propietario p = data.ObtenerPorId(id);
            return View(p);
        }

        // POST: PropietarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var res = data.Baja(id);
                if (res>0)
                {
                    ViewBag.Message = "Propietario Eliminado con Exito";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = @"No se ha podido Eliminar el propietario
                                    Intentelo mas tarde o realice el reclamo a servicio tecnico";
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.Message;
                //ViewBag.StackTrate = ex.StackTrace;
                ViewBag.Error = @"No se ha podido Eliminar el propietario, 
                                se ha producido algun tipo de error, realice el reclamo a servicio tecnico";
                return View();
            }
        }
    }
}
