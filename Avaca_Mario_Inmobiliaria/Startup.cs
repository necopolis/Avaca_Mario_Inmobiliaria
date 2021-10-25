﻿using Avaca_Mario_Inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.


        public void ConfigureServices(IServiceCollection services)
            {
            
              services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(options =>//el sitio web valida con cookie
              {
                      options.LoginPath = "/Usuario/Login";
                      options.LogoutPath = "/Usuario/Logout";
                      options.AccessDeniedPath = "/Home/Restringido";
                  })
                  .AddJwtBearer(options =>//la api web valida con token
              {
                      options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidateAudience = true,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          ValidIssuer = configuration["TokenAuthentication:Issuer"],
                          ValidAudience = configuration["TokenAuthentication:Audience"],
                          IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                              configuration["TokenAuthentication:SecretKey"])),
                      };
                  // opción extra para usar el token el hub
                  options.Events = new JwtBearerEvents
                      {
                          OnMessageReceived = context =>
                          {
                          // Read the token out of the query string
                          var accessToken = context.Request.Query["access_token"];
                          // If the request is for our hub...
                          var path = context.HttpContext.Request.Path;
                              if (!string.IsNullOrEmpty(accessToken) &&
                                  path.StartsWithSegments("/chatsegurohub"))
                              {//reemplazar la url por la usada en la ruta ⬆
                              context.Token = accessToken;
                              }
                              return Task.CompletedTask;
                          }
                      };
                  });
              
            
              services.AddAuthorization(options =>
              {
                  //options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
                  options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "SuperAdministrador"));
              });

            
              services.AddMvc();
            services.AddDbContext<DataContext>(
              options => options.UseSqlServer(
                  configuration["ConnectionStrings:DefaultConnection"]));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            // Habilitar CORS
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            // Uso de archivos estáticos (*.html, *.css, *.js, etc.)
            app.UseStaticFiles();
            app.UseRouting();
            // Permitir cookies
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
            });
            // Habilitar autenticación
            app.UseAuthentication();
            app.UseAuthorization();
            // App en ambiente de desarrollo?
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"login", 
                    pattern: "entrar/{**accion}", new { controller = "Usuario", action = "Login" });
                
                endpoints.MapControllerRoute(
                    name: "SearchInquilinoContratos",
                    pattern: "Pago/Inquilino/{dni}", new { controller = "Pago", action = "Inquilino" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
