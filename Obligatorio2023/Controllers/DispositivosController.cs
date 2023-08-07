using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Obligatorio2023.Data;
using Obligatorio2023.Models;

namespace Obligatorio2023.Controllers
{
    public class DispositivosController : Controller
    {
        private readonly ObligatorioContext _context;

        public DispositivosController(ObligatorioContext context)
        {
            _context = context;
        }

        // GET: Dispositivos/Index
        public async Task<IActionResult> Index()
        {
            if (!Guid.TryParse(User.Claims.First(x => x.Type.Equals("Id")).Value, out Guid usuarioId)) return BadRequest();

            List<Dispositivo> dispositivos;

            if (User.IsInRole("Administrador"))
            {
                dispositivos = await _context.Dispositivo.Include(d => d.UPaciente).ToListAsync();
            }
            else if (User.IsInRole("Medico"))
            {
                // Si el usuario actual es un médico, solo puede ver los dispositivos que él/ella creó.
                dispositivos = await _context.Dispositivo
                    .Where(d => d.MedicoId == usuarioId) // Filtrar por el usuario actual
                    .Include(d => d.UPaciente)
                    .ToListAsync();
            }
            else
            {
                // Si el usuario actual es un paciente, solo puede ver sus dispositivos.
                dispositivos = await _context.Dispositivo
                    .Where(d => d.PacienteId == usuarioId) // Filtrar por el usuario actual
                    .Include(d => d.UPaciente)
                    .ToListAsync();
            }

            return View(dispositivos);
        }

        // GET: Dispositivos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dispositivo == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivo
         .Include(d => d.UPaciente)
         .FirstOrDefaultAsync(m => m.Id == id);

            if (dispositivo == null)
            {
                return NotFound();
            }

            return View(dispositivo);
        }

        //metodo para obtener el ultimo reporte para cada dispositivo
        public IActionResult ObtenerUltimoDatoReporte(int id)
        {
            var ultimoDatoReporte = _context.DatoReporte
                .Where(dr => dr.DispositivoId == id) //where aplica la condicion de que seleccione solo los registros donde coincidan los ID
                .OrderByDescending(dr => dr.FechaHoraUltRegistro) //el dato mas reciente aparece primero
                .FirstOrDefault(); 
            //devolvemos una vista parcial
            return PartialView("_UltimoDatoReporte", ultimoDatoReporte);
        }

        //metodo para mostrar los datos vitales que se reciben del dispositivo seleccionado
        public async Task<IActionResult> DatosVitales(int? id)
        {
            if (id == null || _context.Dispositivo == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivo
         .Include(d => d.UPaciente)
         .FirstOrDefaultAsync(m => m.Id == id);

            if (dispositivo == null)
            {
                return NotFound();
            }

            return View(dispositivo);
        }


        //metodo para obtener todos los reportes del dispositivo seleccionado
        [HttpGet]
        public IActionResult ObtenerReportes(int id)
        {
            var reportes = _context.DatoReporte
                .Where(dr => dr.DispositivoId == id)
                .OrderByDescending(dr => dr.FechaHoraUltRegistro)
                .ToList();

            return PartialView("_DatosReporte", reportes);
        }



        // GET: Dispositivos/Create
        [Authorize(Roles = "Administrador, Medico")]
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.UPaciente, "Id", "NombreApellido");
            return View();
        }

        // POST: Dispositivos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Detalle, Activo,PacienteId")] Dispositivo dispositivo)
        {
            if (User.IsInRole("Medico"))
                dispositivo.MedicoId = GetIdUsuarioLogueado();


            dispositivo.FechaHoraAlta = DateTime.Now;
            dispositivo.FechaHoraUltimaModificacion = DateTime.Now;
            dispositivo.Token = Guid.NewGuid();
            _context.Add(dispositivo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Dispositivos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dispositivo == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivo.FindAsync(id);
            if (dispositivo == null)
            {
                return NotFound();
            }
            ViewData["PacienteId"] = new SelectList(_context.UPaciente, "Id", "NombreApellido", dispositivo.PacienteId);
            return View(dispositivo);
        }

        // POST: Dispositivos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Detalle, FechaHoraAlta, FechaHoraUltimaModificacion, Activo,PacienteId, Token")] Dispositivo dispositivo)
        {
            if (id != dispositivo.Id)
            {
                return NotFound();

            }

            if (ModelState.IsValid)
            {
                try
                {
                    dispositivo.FechaHoraUltimaModificacion = DateTime.Now;
                    _context.Update(dispositivo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispositivoExists(dispositivo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.UPaciente, "Id", "NombreApellido", dispositivo.PacienteId);
            return View(dispositivo);
        }

        // GET: Dispositivos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dispositivo == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivo
                .Include(d => d.UPaciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dispositivo == null)
            {
                return NotFound();
            }

            return View(dispositivo);
        }

        // POST: Dispositivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Dispositivo == null)
                {
                    return Problem("Entity set 'ObligatorioContext.Dispositivo'  is null.");
                }
                var dispositivo = await _context.Dispositivo.FindAsync(id);
                if (dispositivo != null)
                {
                    _context.Dispositivo.Remove(dispositivo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                //en catch usamos exception de base de datos ya que tenemos un trigger que impide eliminar el dispositivo si esta en uso
                var sqlException = ex.InnerException as SqlException;
                if (sqlException != null && sqlException.Number == 50000)
                {
                    //cargamos en un tempdata el error
                    TempData["ErrorMessage"] = "No se puede eliminar un dispositivo que está en uso.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Se produjo un error al eliminar el dispositivo.";
                }
                return RedirectToAction("Delete", "Dispositivos", new { id = id });
            }


        }

        private bool DispositivoExists(int id)
        {
            return (_context.Dispositivo?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private Guid GetIdUsuarioLogueado()
        {
            if (!Guid.TryParse(User.Claims.First(x => x.Type.Equals("Id")).Value, out Guid usuarioId))
                throw new Exception("Esta Id no es un guid");

            return usuarioId;
        }

        //reutilizamos el metodo de index para tener otra lista con los requerimientos de BD
        public async Task<IActionResult> ListadoRBD()
        {
            if (!Guid.TryParse(User.Claims.First(x => x.Type.Equals("Id")).Value, out Guid usuarioId)) return BadRequest();

            List<Dispositivo> dispositivos;

            if (User.IsInRole("Administrador"))
            {
                dispositivos = await _context.Dispositivo.Include(d => d.UPaciente).ToListAsync();
            }
            else if (User.IsInRole("Medico"))
            {
                // Si el usuario actual es un médico, solo puede ver los dispositivos que él/ella creó.
                dispositivos = await _context.Dispositivo
                    .Where(d => d.MedicoId == usuarioId) // Filtrar por el usuario actual
                    .Include(d => d.UPaciente)
                    .ToListAsync();
            }
            else
            {
                // Si el usuario actual es un paciente, solo puede ver sus dispositivos.
                dispositivos = await _context.Dispositivo
                    .Where(d => d.PacienteId == usuarioId) // Filtrar por el usuario actual
                    .Include(d => d.UPaciente)
                    .ToListAsync();
            }

            return View(dispositivos);
        }


        public IActionResult RBD08()
        {
            return View();
        }
        public IActionResult RBD09()
        {
            return View();
        }

        public static SqlConnection ObtenerConexion()
        {
            //connection aparte para usarla varias veces
            string strcon = @"Data Source=LAPTOP-MRHGENDT\SQLEXPRESS;Initial Catalog = Obligatorio_2023 ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(strcon);
            return conn;
        }

        public IActionResult MostrarLogEndPoint()
        {
            //mostramos el requerimiento de BD mediante ADO
            try
            {
                DataTable registrosLog = new DataTable();
                //usamos using para asegurar que cuando se termine el bloque de ejecucion se cierren esos recursos
                using (SqlConnection connection = ObtenerConexion())
                {
                    connection.Open();


                    string query = "SELECT * FROM LogEndpoint ORDER BY FechaInvocacion DESC";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.Fill(registrosLog);
                    }
                }
                return View(registrosLog);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }

        public IActionResult MostrarHistoricoDispositivo()
        {
            try
            {
                var historicoDispositivos = new DataTable();

                using (SqlConnection connection = ObtenerConexion())
                {
                    connection.Open();

                    string query = "SELECT * FROM HistoricoDispositivos";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(historicoDispositivos);
                        }
                    }
                }

                return View(historicoDispositivos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return View();
        }





    }
}
