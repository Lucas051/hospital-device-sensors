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
            UPaciente usuPac = _context.UPaciente.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra);
            UMedico usuMed = _context.UMedico.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra);
            UAdministrador usuAdm = _context.UAdministrador.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra);

            if (usuPac != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nomUsu),
                    new Claim(ClaimTypes.Role, "Paciente"),
                    new Claim(type: "Id", value: usuPac.Id.ToString()),

                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "UPacientes");
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
                return RedirectToAction("Index", "UMedicos");
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
