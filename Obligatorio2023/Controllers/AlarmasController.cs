﻿using System;
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

        //creamos este metodo para traer del contexto los registros de alarmas sin la necesidad de crear
        //otro controlador de 0, ya que solo necesitamos mostrar las alarmas que "saltaron", no se deben manipular por el usuario
        public IActionResult RegistroAlarma()
        {
            var registros = _context.RegistroAlarma
                .Include(ra => ra.Paciente)
                .Include(ra => ra.Alarma)
                .ToList();

            return View(registros);
        }
        public static SqlConnection ObtenerConexion()
        {
            string strcon = @"Data Source=LAPTOP-MRHGENDT\SQLEXPRESS;Initial Catalog = Obligatorio_2023 ;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(strcon);
            return conn;
        }
        public IActionResult UltimosRegistrosAlarma(int deviceId)
        {
            var lastAlarmRecords = new List<RegistroAlarma>();

            SqlConnection conn = ObtenerConexion(); 
            
            
        
                    // Abrir la conexión con la base de datos
                     conn.Open();
               
            
                    string query = "SELECT TOP 10 * FROM RegistroAlarma WHERE IdDispositivo = @deviceId ORDER BY FechaHoraGeneracion ASC";

                    // Crear el comando SQL
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        // Agregar el parámetro del dispositivo ID a la consulta
                        command.Parameters.AddWithValue("@deviceId", deviceId);

                        // Ejecutar la consulta y obtener los resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Crear un objeto RegistroAlarma y asignar los valores del lector a las propiedades
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

                                // Agregar el objeto a la lista
                                lastAlarmRecords.Add(registroAlarma);
                            }
                        }
                    }
            

            return View("UltimosRegistrosAlarma", lastAlarmRecords);
        }

    }
}
