using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obligatorio2023.Data;
using Obligatorio2023.Models;

namespace Obligatorio2023.Controllers.API
{
    [Route("api/[controller]")]
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
          if (_context.DatoReporte == null)
          {
              return NotFound();
          }
            var datoReporte = await _context.DatoReporte.FindAsync(id);

            if (datoReporte == null)
            {
                return NotFound();
            }

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
        [HttpPost]
        public async Task<ActionResult<DatoReporte>> PostDatoReporte(DatoReporte datoReporte)
        {
          if (_context.DatoReporte == null)
          {
              return Problem("Entity set 'ObligatorioContext.DatoReporte'  is null.");
          }
            _context.DatoReporte.Add(datoReporte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDatoReporte", new { id = datoReporte.Id }, datoReporte);
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
