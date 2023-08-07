using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Obligatorio2023.Data;
using Obligatorio2023.Models;

namespace Obligatorio2023.Controllers
{
    public class AlarmasController : Controller
    {
        private readonly ObligatorioContext _context;

        public AlarmasController(ObligatorioContext context)
        {
            _context = context;
        }

        // GET: Alarmas
        public async Task<IActionResult> Index()
        {
            var obligatorioContext = _context.Alarma.Include(a => a.Paciente);
            return View(await obligatorioContext.ToListAsync());
        }

        // GET: Alarmas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Alarma == null)
            {
                return NotFound();
            }

            var alarma = await _context.Alarma
                .Include(a => a.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alarma == null)
            {
                return NotFound();
            }

            return View(alarma);
        }

        // GET: Alarmas/Create
        public IActionResult Create()
        {
            ViewData["IdPaciente"] = new SelectList(_context.UPaciente, "Id", "NombreApellido");
            return View();
        }

        // POST: Alarmas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,DatoEvaluar,ValorLimite,Comparador,IdPaciente")] Alarma alarma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alarma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPaciente"] = new SelectList(_context.UPaciente, "Id", "NombreApellido", alarma.IdPaciente);
            return View(alarma);
        }

        // GET: Alarmas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Alarma == null)
            {
                return NotFound();
            }

            var alarma = await _context.Alarma.FindAsync(id);
            if (alarma == null)
            {
                return NotFound();
            }
            ViewData["IdPaciente"] = new SelectList(_context.UPaciente, "Id", "Id", alarma.IdPaciente);
            return View(alarma);
        }

        // POST: Alarmas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,DatoEvaluar,ValorLimite,Comparador,IdPaciente")] Alarma alarma)
        {
            if (id != alarma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alarma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlarmaExists(alarma.Id))
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
            ViewData["IdPaciente"] = new SelectList(_context.UPaciente, "Id", "Id", alarma.IdPaciente);
            return View(alarma);
        }

        // GET: Alarmas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Alarma == null)
            {
                return NotFound();
            }

            var alarma = await _context.Alarma
                .Include(a => a.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alarma == null)
            {
                return NotFound();
            }

            return View(alarma);
        }

        // POST: Alarmas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Alarma == null)
            {
                return Problem("Entity set 'ObligatorioContext.Alarma'  is null.");
            }
            var alarma = await _context.Alarma.FindAsync(id);
            if (alarma != null)
            {
                _context.Alarma.Remove(alarma);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlarmaExists(int id)
        {
          return (_context.Alarma?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public static SqlConnection ObtenerConexion()
        {
            //connection aparte para usarla varias veces
            string strcon = @"Data Source=LAPTOP-MRHGENDT\SQLEXPRESS;Initial Catalog = Obligatorio_2023 ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(strcon);
            return conn;
        }

        public IActionResult UltimosRegistrosAlarma(int Id)
        {
            //creamos esta var para asignar una list de registro alarmas
            var ultimosRegistros = new List<RegistroAlarma>();

            try
            {
                using (SqlConnection conn = ObtenerConexion())
                {
                    // 1 - Abrir conn
                    conn.Open();

                    string query = "SELECT TOP 10 * FROM RegistroAlarma WHERE IdDispositivo = @deviceId ORDER BY FechaHoraGeneracion ASC";

                    // 2 - command
                    //utilizamos using para asegurar que cuando se termine el bloque los recursos se liberen
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        // Agregar el parámetro de busqueda
                        command.Parameters.AddWithValue("@deviceId", Id);

                       //usamos reader para select
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                               
                                var registroAlarma = new RegistroAlarma
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    FechaHoraGeneracion = Convert.ToDateTime(reader["FechaHoraGeneracion"]),
                                    DatoEvaluar = reader["DatoEvaluar"].ToString(),
                                    ValorLimite = Convert.ToSingle(reader["ValorLimite"]),
                                    ValorRecibido = reader["ValorRecibido"].ToString(),
                                    IdPaciente = Guid.Parse(reader["IdPaciente"].ToString()),
                                    IdAlarma = Convert.ToInt32(reader["IdAlarma"])
                                };

                             
                                ultimosRegistros.Add(registroAlarma);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View("UltimosRegistrosAlarma", ultimosRegistros);
        }
    }
}
