using Avaca_Mario_Inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class GaranteController : Controller
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public GaranteController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/<controller>
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {

            try
            {

                var email = User.Identity.Name;
                var propietario = await contexto.Propietario.FirstOrDefaultAsync(x => x.Email == email);

                if (id == 0)
                {
                    return BadRequest();
                }


                return Ok(await contexto.Garante.Join(
                    contexto.Contrato.Where(x => x.InmuebleId == id),
                    gar => gar.Id,
                    com => com.GaranteId,
                    (gar, com) => gar).FirstOrDefaultAsync());


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());

            }




        }

        /*

            // POST api/<controller>
            [HttpPost]
                public async Task<IActionResult> Post([FromForm]Contrato entidad)
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            var usuario = User.Identity.Name;
                            entidad.Inmueble.PropietarioId = contexto.Propietario.Single(e => e.Email == User.Identity.Name).Id;
                            contexto.Contrato.Add(entidad);
                            contexto.SaveChanges();
                            return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
                        }
                        return BadRequest("Model State Invalido");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }

                // PUT api/<controller>/5
                [HttpPut]
                public async Task<IActionResult> Put([FromBody] Contrato entidad)
                {
                    try
                    {
                        if (ModelState.IsValid && contexto.Inmueble.AsNoTracking().Include(e => e.Duenio).FirstOrDefault(e => e.Id == entidad.Id && e.Duenio.Email == User.Identity.Name) != null)
                        {
                            //entidad.Id = id;
                            contexto.Contrato.Update(entidad);
                            contexto.SaveChanges();
                            return Ok(entidad);
                        }
                        return BadRequest();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }

                // DELETE api/<controller>/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> Delete(int id)
                {
                    try
                    {
                        var entidad = contexto.Contrato.Include(e => e.Inmueble.PropietarioId).FirstOrDefault(e => e.Id == id && e.Inmueble.Duenio.Email == User.Identity.Name);
                        if (entidad != null)
                        {
                            contexto.Contrato.Remove(entidad);
                            contexto.SaveChanges();
                            return Ok();
                        }
                        return BadRequest();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }

                // DELETE api/<controller>/5
                [HttpDelete("BajaLogica/{id}")]
                public async Task<IActionResult> BajaLogica(int id)
                {
                    try
                    {
                        var entidad = contexto.Contrato.Include(e => e.Inmueble.Duenio).FirstOrDefault(e => e.Id == id && e.Inmueble.Duenio.Email == User.Identity.Name);
                        if (entidad != null)
                        {
                            //entidad.Superficie = -1;//cambiar por estado = 0
                            contexto.Contrato.Update(entidad);
                            contexto.SaveChanges();
                            return Ok();
                        }
                        return BadRequest();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }

                */
    }
}
