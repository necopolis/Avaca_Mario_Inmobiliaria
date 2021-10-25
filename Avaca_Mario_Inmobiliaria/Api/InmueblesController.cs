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
    public class InmueblesController : Controller
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public InmueblesController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Inmueble
                    .Include(e => e.Duenio)
                    .Where(e => e.Duenio.Email == usuario));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Inmueble
                    .Include(e => e.Duenio)
                    .Where(e => e.Duenio.Email == usuario)
                    .Single(e => e.Id == id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]Inmueble entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.PropietarioId = contexto.Propietario
                        .Single(e => e.Email == User.Identity.Name).Id;

                    contexto.Inmueble.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
                }
                return BadRequest("LLego hasta aca");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /*

        // PUT api/<controller>/5
        [HttpPut("id")]
        public async Task<IActionResult> Put([FromBody] Inmueble entidad, int id)
        {
            try
            {
                if (ModelState.IsValid && contexto.Inmueble
                    .AsNoTracking().Include(e => e.Duenio)
                    .FirstOrDefault(e => e.Id == entidad.Id && e.Duenio.Email == User.Identity.Name) != null)
                {
                    //entidad.Id = id;
                    contexto.Inmueble.Update(entidad);
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

        */

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entidad = contexto.Inmueble
                    .Include(e => e.Duenio)
                    .FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name);

                if (entidad != null)
                {
                    contexto.Inmueble.Remove(entidad);
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
                var entidad = contexto.Inmueble
                    .Include(e => e.Duenio)
                    .FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name);
                if (entidad != null)
                {
                    //entidad.Superficie = -1;//cambiar por estado = 0
                    contexto.Inmueble.Update(entidad);
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

        [HttpGet("alquilados")]
        public async Task<IActionResult> GetAlquilados()
        {

            try
            {
                //context.tabla_1.join(tabla_2, columna_Keyforeana_tabla_1, (JOIN)columna_key_tabla_2, datos_que_quiero_traer  )

                var email = User.Identity.Name;
                //var propietario = await contexto.Propietario.FirstOrDefaultAsync(x => x.Email == email);
                return Ok( await contexto.Inmueble.Join(
                    contexto.Contrato.Where(x => x.FechaFin > DateTime.Now && x.FechaInicio < DateTime.Now),
                    inm => inm.Id,
                    con => con.InmuebleId,
                    (inm, con) => inm).ToListAsync());

                //return inmuebles;


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());

            }
        }
        [HttpPut]//Cambiar Estado
        public async Task<ActionResult<Inmueble>> Put([FromBody] Inmueble inmueble)
        {

            try
            {

                var email = User.Identity.Name;
                var propietario = await contexto.Propietario.FirstOrDefaultAsync(x => x.Email == email);
                int id = inmueble.Id;
                bool estado = inmueble.Activo;
                Inmueble inmuebleV = await contexto.Inmueble.FirstOrDefaultAsync(x => x.Id == id && x.PropietarioId == propietario.Id);
                if (inmuebleV == null)
                {
                    return NotFound();
                }

                inmuebleV.Activo = estado;
                contexto.Update(inmuebleV);
                await contexto.SaveChangesAsync();
                return inmuebleV;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
