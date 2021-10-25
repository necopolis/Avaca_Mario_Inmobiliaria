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
    public class ContratosController : Controller
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public ContratosController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        //#############################  TODOS LOS CONTRATOS   ################################
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var lista = await contexto.Contrato
                                .Include(x => x.Inquilino)
                                .Include(x => x.Inmueble)
                                .Include(x=> x.Garante)
                                .Where(x => x.Inmueble.Duenio.Email == usuario && x.Activo==true).ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //#############################  CONTRATOS ID   ################################

        [HttpGet("{id}")]
        public async Task<ActionResult<Contrato>> ContratoInmueble(int id)
        {

            try
            {
                if (id != 0)
                {
                    return await contexto.Contrato
                        .Include(x => x.Inquilino)
                        .Include(x => x.Inmueble)
                        .Include(x => x.Garante)
                        .Where(x =>
                     x.InmuebleId == id)
                    .FirstOrDefaultAsync();
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());

            }
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    try
        //    {
        //        var usuario = User.Identity.Name;
        //        var contrato = await contexto.Contrato
        //                            .Include(x => x.Inquilino)
        //                            .Include(x => x.Garante)
        //                            .Include(x => x.Inmueble)
        //                            .Where(x => x.InmuebleId == id)
        //                            .SingleAsync();
        //        return Ok(contrato);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}


    }
}
