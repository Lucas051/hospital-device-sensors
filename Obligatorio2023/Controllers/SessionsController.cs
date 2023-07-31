using Microsoft.AspNetCore.Authentication;
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
            if (_context.UPaciente.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra) != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nomUsu),
                    new Claim(ClaimTypes.Role, "Paciente")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "UPacientes");
            }
            else if (_context.UMedico.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra) != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nomUsu),
                    new Claim(ClaimTypes.Role, "Medico")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "UMedicos");
            }
            else if (_context.UAdministrador.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra) != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nomUsu),
                    new Claim(ClaimTypes.Role, "Administrador")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "UAdministradores");
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
