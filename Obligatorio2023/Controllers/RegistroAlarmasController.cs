using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obligatorio2023.Data;
using Obligatorio2023.Models;
using System.Security.Claims;

namespace Obligatorio2023.Controllers
{
    public class RegistroAlarmasController : Controller
    {
        private readonly ObligatorioContext _context;

        public RegistroAlarmasController(ObligatorioContext context)
        {
            _context = context;
        }
        public IActionResult RegistroAlarma()
        {
            List<RegistroAlarma> registros;

            if (User.IsInRole("Administrador"))
            {
                registros = _context.RegistroAlarma
                .Include(ra => ra.Paciente)
                .Include(ra => ra.Alarma)
                .ToList();
            }
            else if (User.IsInRole("Medico"))
            {
                var usuarioId = new Guid(User.FindFirstValue("Id"));

                registros = _context.RegistroAlarma
                    .Include(ra => ra.Dispositivo)
                    .Where(ra => ra.Dispositivo.MedicoId == usuarioId)
                    .Include(ra => ra.Paciente)
                    .Include(ra => ra.Alarma)
                    .ToList();
            }
            else
            {
                // Si el usuario actual es un paciente, muestra un mensaje de error.
                ViewBag.ErrorMessage = "No tienes permiso para ver las alarmas.";
                return View();
            }
            return View(registros.OrderByDescending(x => x.FechaHoraGeneracion));
        }
    }
}
