﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obligatorio2023.Data;
using Obligatorio2023.Models;

namespace Obligatorio2023.Controllers.API
{
    [Route("api/dispositivos")]
    [ApiController]
    public class DatoReportesController : ControllerBase
    {
        private readonly ObligatorioContext _context;

        public DatoReportesController(ObligatorioContext context)
        {
            _context = context;
        }

        // GET: api/DatoReportes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatoReporte>>> GetDatoReporte()
        {
            if (_context.DatoReporte == null)
            {
                return NotFound();
            }
            return await _context.DatoReporte.ToListAsync();
        }

        // GET: api/DatoReportes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DatoReporte>> GetDatoReporte(int id)
        {
            // crea e inicia el Stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (_context.DatoReporte == null)
            {
                return NotFound();
            }
            var datoReporte = await _context.DatoReporte.FindAsync(id);
            if (datoReporte == null)
            {
                return NotFound();
            }
            stopwatch.Stop();

            //registrar la invocacion
            string NombreEndpoint = "GetDatoReporte";
            DateTime FechaInvocacion = DateTime.Now;
            int Duracion = Convert.ToInt32(stopwatch.ElapsedMilliseconds);
            _context.LogInvocacionEndpoint(NombreEndpoint, FechaInvocacion, Duracion);

            return datoReporte;
        }

        // PUT: api/DatoReportes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDatoReporte(int id, DatoReporte datoReporte)
        {
            if (id != datoReporte.Id)
            {
                return BadRequest();
            }

            _context.Entry(datoReporte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatoReporteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DatoReportes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{token}/datos/")]
        public async Task<ActionResult<DatoReporte>> PostDatoReporte(DatoReporte datoReporte)
        {
            // crea e inicia el Stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Obtener el dispositivo asociado al reporte
            var dispositivo = await _context.Dispositivo
                .Include(d => d.UPaciente) // Incluir la entidad paciente asociada al dispositivo
                .FirstOrDefaultAsync(d => d.Id == datoReporte.DispositivoId);
            if (dispositivo == null)
            {
                return NotFound("Device not found.");
            }
            // Obtener todas las alarmas asociadas al paciente del dispositivo
            var alarmas = await _context.Alarma
                .Where(a => a.IdPaciente == dispositivo.PacienteId)
                .ToListAsync();

            var registroAlarmas = new List<RegistroAlarma>();

            // Verificar si los datos cumplen con alguna alarma y guardar los registros en RegistroAlarma
            foreach (var alarma in alarmas)
            {
                if (VerificarAlarma(datoReporte, alarma))
                {
                    // Crear un nuevo registro en RegistroAlarma
                    registroAlarmas.Add(new RegistroAlarma
                    {
                        FechaHoraGeneracion = DateTime.Now,
                        DatoEvaluar = alarma.DatoEvaluar,
                        ValorRecibido = datoReporte.GetType().GetProperty(alarma.DatoEvaluar).GetValue(datoReporte).ToString(),
                        ValorLimite = alarma.ValorLimite,
                        IdPaciente = alarma.IdPaciente,
                        IdDispositivo = datoReporte.DispositivoId,
                        Alarma = alarma
                    });
                }
            }


            if (registroAlarmas.Any())
            {
                // Guardamos activaciones de alarmas
                _context.RegistroAlarma.AddRange(registroAlarmas);
                await _context.SaveChangesAsync();
            }

            // Logueamos el tiempo que tomó la ejecucion del endpoint
            stopwatch.Stop();

            string NombreEndpoint = "PostDatoReporte";
            DateTime FechaInvocacion = DateTime.Now;
            int Duracion = Convert.ToInt32(stopwatch.ElapsedMilliseconds);
            _context.LogInvocacionEndpoint(NombreEndpoint, FechaInvocacion, Duracion);
            if (_context.DatoReporte == null)
            {
                return Problem("Entity set 'ObligatorioContext.DatoReporte'  is null.");
            }
            _context.DatoReporte.Add(datoReporte);
            await _context.SaveChangesAsync();



            return Ok(new
            {
                Success = true,
                Message = registroAlarmas.Any()
                    ? "¡Atencion! Una alarma se ha activado!"
                    : ""
            });

        }
         
        private bool VerificarAlarma(DatoReporte datoReporte, Alarma alarma)
        {


            // Realizar la comparación con el valor límite de la alarma


            switch (alarma.DatoEvaluar)
            {
                case "PresionSistolica":
                    return CompararValor(datoReporte.PresionSistolica, alarma.ValorLimite, alarma.Comparador);
                case "PresionDistolica":
                    return CompararValor(datoReporte.PresionDistolica, alarma.ValorLimite, alarma.Comparador);
                case "Temperatura":
                    return CompararValor(datoReporte.Temperatura, alarma.ValorLimite, alarma.Comparador);
                case "SaturacionOxigeno":
                    return CompararValor(datoReporte.SaturacionOxigeno, alarma.ValorLimite, alarma.Comparador);
                case "Pulso":
                    return CompararValor(datoReporte.Pulso, alarma.ValorLimite, alarma.Comparador);
                default:
                    // Si no se reconoce el dato a evaluar, se devuelve false (alarma no cumplida)
                    return false;
            }
        }

        private bool CompararValor(float valorReporte, float valorLimite, Comparador comparador)
        {
            switch (comparador)
            {
                case Comparador.Mayor:
                    return valorReporte > valorLimite;
                case Comparador.Menor:
                    return valorReporte < valorLimite;
                case Comparador.Igual:
                    return valorReporte == valorLimite;
                case Comparador.MayorIgual:
                    return valorReporte >= valorLimite;
                case Comparador.MenorIgual:
                    return valorReporte <= valorLimite;
                default:
                    // Si no se reconoce el comparador, se devuelve false (alarma no cumplida)
                    return false;
            }
        }

        // DELETE: api/DatoReportes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDatoReporte(int id)
        {
            if (_context.DatoReporte == null)
            {
                return NotFound();
            }
            var datoReporte = await _context.DatoReporte.FindAsync(id);
            if (datoReporte == null)
            {
                return NotFound();
            }

            _context.DatoReporte.Remove(datoReporte);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DatoReporteExists(int id)
        {
            return (_context.DatoReporte?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
