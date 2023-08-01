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
    public class AlarmasController : ControllerBase
    {
        private readonly ObligatorioContext _context;

        public AlarmasController(ObligatorioContext context)
        {
            _context = context;
        }

        // GET: api/Alarmas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alarma>>> GetAlarma()
        {
          if (_context.Alarma == null)
          {
              return NotFound();
          }
            return await _context.Alarma.ToListAsync();
        }

        // GET: api/Alarmas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alarma>> GetAlarma(int id)
        {
          if (_context.Alarma == null)
          {
              return NotFound();
          }
            var alarma = await _context.Alarma.FindAsync(id);

            if (alarma == null)
            {
                return NotFound();
            }

            return alarma;
        }

        // PUT: api/Alarmas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlarma(int id, Alarma alarma)
        {
            if (id != alarma.Id)
            {
                return BadRequest();
            }

            _context.Entry(alarma).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlarmaExists(id))
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

        // POST: api/Alarmas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Alarma>> PostAlarma(Alarma alarma)
        {
          if (_context.Alarma == null)
          {
              return Problem("Entity set 'ObligatorioContext.Alarma'  is null.");
          }
            _context.Alarma.Add(alarma);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlarma", new { id = alarma.Id }, alarma);
        }

        // DELETE: api/Alarmas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlarma(int id)
        {
            if (_context.Alarma == null)
            {
                return NotFound();
            }
            var alarma = await _context.Alarma.FindAsync(id);
            if (alarma == null)
            {
                return NotFound();
            }

            _context.Alarma.Remove(alarma);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlarmaExists(int id)
        {
            return (_context.Alarma?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
