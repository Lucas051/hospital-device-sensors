using System;
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
        //hola
        public DatoReportesController(ObligatorioContext context)
        {
            _context = context;
        }

        // GET: api/DatoReportes ObtenerTodos
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
        //Task<ActionResult<DatoReporte>> indica que es asincrono, devuelve un resultado tipo DatoReporte
        public async Task<ActionResult<DatoReporte>> GetDatoReporte(int id)
        {
            // Crea e inicia el Stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //_____________________________________

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

            //Registrar la invocacion
            string NombreEndpoint = "GetDatoReporte";
            DateTime FechaInvocacion = DateTime.Now;
            int Duracion = Convert.ToInt32(stopwatch.ElapsedMilliseconds);
            _context.LogInvocacionEndpoint(NombreEndpoint, FechaInvocacion, Duracion);

            return datoReporte;
        }

        // PUT: api/DatoReportes/5
    
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

        [HttpPost("{token}/datos/")]
        public async Task<ActionResult<DatoReporte>> PostDatoReporte(DatoReporte datoReporte)
        {
            // Crea e inicia el Stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Obtener el dispositivo asociado al reporte
            var dispositivo = await _context.Dispositivo
                .Include(d => d.UPaciente) // Incluir la entidad paciente asociada al dispositivo
                .FirstOrDefaultAsync(d => d.Id == datoReporte.DispositivoId);

            if (dispositivo == null)
            {
                return NotFound("Dispositivo no encontrado.");
            }

            // Obtener todas las alarmas asociadas al paciente del dispositivo
            var alarmas = await _context.Alarma
                .Where(a => a.IdPaciente == dispositivo.PacienteId)
                .ToListAsync();

            // Verificar si los datos cumplen con alguna alarma y guardar los registros en RegistroAlarma
            foreach (var alarma in alarmas)
            {
                if (VerificarAlarma(datoReporte, alarma))
                {
                    // Crear un nuevo registro en RegistroAlarma
                    var registroAlarma = new RegistroAlarma
                    {
                        FechaHoraGeneracion = DateTime.Now,
                        DatoEvaluar = alarma.DatoEvaluar,
                        ValorRecibido = datoReporte.GetType().GetProperty(alarma.DatoEvaluar).GetValue(datoReporte).ToString(),
                        ValorLimite = alarma.ValorLimite,
                        IdPaciente = alarma.IdPaciente,
                        IdDispositivo = datoReporte.DispositivoId,
                        Alarma = alarma
                    };

                    _context.RegistroAlarma.Add(registroAlarma);
                    await _context.SaveChangesAsync();

                    // Retorna un mensaje de alerta
                    return Ok(new
                    {
                        Success = true,
                        Message = "¡Atencion! Una alarma se ha activado!"
                    });

                }
            }

            stopwatch.Stop();

            //Registrar la invocacion
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
            return CreatedAtAction("GetDatoReporte", new { id = datoReporte.Id }, datoReporte);
        }

        //Funcion que utilizamos en PostDatoReporte para verificar si se cumple una alarma, devuelve bool para ser utilizado en la sentencia IF
        private bool VerificarAlarma(DatoReporte datoReporte, Alarma alarma)
        {
            // Usamos un switch para poder contemplar todos los casos que pueden ser causa de alarma, usamos otra funcion
            //CompararValor(datoReporte, alarma, Comparador) para enviar los datos a comparar en cada caso
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
        //Aqui nos llegan los parametros a comparar
        private bool CompararValor(float valorReporte, float valorLimite, Comparador comparador)
        {
            //en el switch utilizamos el parametro de comparador, dependiendo del caso vamos a utilizar los operadores logicos que correspondan
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
