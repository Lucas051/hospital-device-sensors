using Microsoft.AspNetCore.Mvc;
using Obligatorio2023.Data;
using Obligatorio2023.Models;
using System.Diagnostics;

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
        public ActionResult Login(string nomUsu, string contra)
        {
            //validando nombre de usuario y contrasenia con la base de datos
            //si es valido se redirect al controller correspondiente
            if (_context.UPaciente.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra) != null)
            {
               // ViewData["Rol"] = "Paciente";
                return RedirectToAction("Index", "UPacientes");
            }else if (_context.UMedico.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra) != null)
            {
                //ViewData["Rol"] = "Medico";
                return RedirectToAction("Index", "UMedicos");
            }else if (_context.UAdministrador.SingleOrDefault(p => p.NombreUsuario == nomUsu && p.Contraseña == contra) != null)
            {
               // ViewData["Rol"] = "Administrador";
                return RedirectToAction("Index", "UAdministradores");
            }

            // Si falla se muestra mensaje de error
            ModelState.AddModelError("", "Nombre o contraseña invalidos");
            return View();
        }
    }

}
