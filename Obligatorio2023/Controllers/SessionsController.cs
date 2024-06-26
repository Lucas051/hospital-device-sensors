﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Obligatorio2023.Data;
using Obligatorio2023.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Obligatorio2023.Controllers
{


    public class SessionsController : Controller
    {
        private readonly ObligatorioContext _context;

        public SessionsController(ObligatorioContext context)
        {
            _context = context;
        }
        // GET: Sessions/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Sessions/Login
        [HttpPost]
        public async Task<IActionResult> Login(string nomUsu, string contra)
        {
            //se busca un usuario pac, med o adm que coincida por nomUsu y contra en la base de datos y se guarda el mismo
            UPaciente usuPac = _context.UPaciente.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra);
            UMedico usuMed = _context.UMedico.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra);
            UAdministrador usuAdm = _context.UAdministrador.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra);
            
            // Verificar si el usuario es un paciente.
            if (usuPac != null)
            {
                // Crear una lista de reclamaciones (claims) para el paciente.
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nomUsu),
                    new Claim(ClaimTypes.Role, "Paciente"),
                    new Claim(type: "Id", value: usuPac.Id.ToString()),

                };
                // Crear una identidad de reclamaciones con la autenticación de cookies.
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // Iniciar sesión utilizando la identidad creada.
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }
            else if (usuMed != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nomUsu),
                    new Claim(ClaimTypes.Role, "Medico"),
                    new Claim(type: "Id", value: usuMed.Id.ToString()),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }
            else if (usuAdm != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nomUsu),
                    new Claim(ClaimTypes.Role, "Administrador"),
                    new Claim(type: "Id", value: usuAdm.Id.ToString()),


                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }

            // Si falla se muestra mensaje de error
            ModelState.AddModelError("", "Nombre o contraseña invalidos");
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Sessions");
        }

        public ActionResult AccesoDenegado()
        {
            return View();
        }
    }

}
