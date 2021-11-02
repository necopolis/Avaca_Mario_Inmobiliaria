﻿using Avaca_Mario_Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly ApplicationDbContext Contexto;

		public TestController(ApplicationDbContext dataContext)
		{
			this.Contexto = dataContext;
		}
		// GET: api/<controller>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				return Ok(new
				{
					Mensaje = "Éxito",
					Error = 0,
					Resultado = new
					{
						Clave = "Key",
						Valor = "Value"
					},
				});
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		/*

		// GET api/<controller>/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			return Ok(Contexto.Propietario.Find(id));
		}

		// GET api/<controller>/5
		[HttpGet("usuarios/{id=0}")]
		public IActionResult GetUsers(int id)
		{
			return Ok(Contexto.Usuario.ToList());
		}

		// GET api/<controller>/5
		[HttpGet("emails/{id=0}")]
		public IActionResult Emails(int id)
		{
			if (id > 0)
				return Ok(Contexto.Propietario.Where(x => x.Id == id).Select(x => x.Email).Single());
			else
				return Ok(Contexto.Propietario.Select(x => x.Email).ToList());
		}

		// GET api/<controller>/5
		[HttpGet("anonimo/{id}")]
		public IActionResult GetAnonimo(int id)
		{
			return id > 0 ?
				Ok(Contexto.Propietario.Where(x => x.Id == id)
				.Select(x => new { Id = x.Id, x.Email }).Single()) :
				Ok(Contexto.Propietario.Select(x => new { Id = x.Id, x.Email }).ToList());
		}

		// POST api/<controller>
		[HttpPost]
		public void Post([FromForm] string value, IFormFile file)
		{

		}

		// POST api/<controller>/usuario/5
		[HttpPost("usuario/{id}")]
		public Usuario Post([FromForm] Usuario usuario, int id)
		{
			usuario.Id = id;
			return usuario;
		}

		// POST api/<controller>/usuario/5
		[HttpPost("login")]
		public async Task<IActionResult> Post([FromForm] LoginView login)
		{
			return Ok(login);
		}

		// PUT api/<controller>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<controller>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

		// DELETE api/<controller>/5
		[HttpGet("personas")]
		public async Task<IActionResult> Personas()
		{
			//return Ok(Contexto.Personas.Include(x => x.Pasatiempos).ThenInclude(x => x.Pasatiempo).Select(x => new PersonaView(x)));
			var lta = await Contexto.Personas.Include(x => x.Pasatiempos).ThenInclude(x => x.Pasatiempo).ToListAsync();
			return Ok(lta.Select(x => new PersonaView(x)));
		}

		// DELETE api/<controller>/5
		[HttpGet("pasatiempos")]
		public async Task<IActionResult> Pasatiempos()
		{
			return Ok(Contexto.PersonaPasatiempos.Include(x => x.Pasatiempo).Where(x => x.PersonaId == 10).Select(x => x.Pasatiempo));
		}

		// DELETE api/<controller>/5
		[HttpGet("dependientes")]
		public async Task<IActionResult> Dependientes()
		{
			Persona p10 = new Persona
			{
				Id = 10,
				Pasatiempos = new List<PersonaPasatiempo>
				{
					new PersonaPasatiempo {
						Id = 10,
					},
					new PersonaPasatiempo {
						Id = 0,
						PersonaId = 10,
						PasatiempoId = 11,
					},
				},
			};
			Contexto.Personas.Attach(p10);
			await Contexto.SaveChangesAsync();
			return Ok(1);
		}
		*/
	}

}
