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
    public class InquilinosController : Controller
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public InquilinosController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/<controller/25>
        [HttpGet("{id}")]
        public async Task<ActionResult<Contrato>> Get(int id)
        {

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                return await 
                    contexto.Contrato.Where(x => x.InmuebleId == id)
                    .Include(x=> x.Garante)
                    .Include(x=> x.Inquilino)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());

            }
        }

        //ESTA MAL ESTE METODO
       /*
        [HttpGet]
       public async Task<IActionResult> GetOpt(Inmueble inmueble)
        {

            try
            {


                if (inmueble.Id == 0)
                {
                    return BadRequest();
                }


                return Ok(await contexto.Inquilino.Join(
                    contexto.Contrato.Where(x => x.InmuebleId == inmueble.Id),
                    inq => inq.Id,
                    com => com.InquilinoId,
                    (inq, com) => inq).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());

            }
        }
        */
    }
}
