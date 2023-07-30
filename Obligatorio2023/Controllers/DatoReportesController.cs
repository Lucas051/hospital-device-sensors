using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Obligatorio2023.Data;
using Obligatorio2023.Models;

namespace Obligatorio2023.Controllers
{
    public class DatoReportesController : Controller
    {
        private readonly ObligatorioContext _context;

        public DatoReportesController(ObligatorioContext context)
        {
            _context = context;
        }

        // GET: DatoReportes
        public async Task<IActionResult> Index()
        {
            var obligatorioContext = _context.DatoReporte.Include(d => d.Dispositivo);
            return View(await obligatorioContext.ToListAsync());
        }

        // GET: DatoReportes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DatoReporte == null)
            {
                return NotFound();
            }

            var datoReporte = await _context.DatoReporte
                .Include(d => d.Dispositivo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datoReporte == null)
            {
                return NotFound();
            }

            return View(datoReporte);
        }

        // GET: DatoReportes/Create
        public IActionResult Create()
        {
            ViewData["DispositivoId"] = new SelectList(_context.Dispositivo, "Id", "Id");
            return View();
        }

        // POST: DatoReportes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PresionArterial,Temperatura,SaturacionOxigeno,Pulso,FechaHoraUltRegistro,DispositivoId")] DatoReporte datoReporte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datoReporte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DispositivoId"] = new SelectList(_context.Dispositivo, "Id", "Id", datoReporte.DispositivoId);
            return View(datoReporte);
        }

        // GET: DatoReportes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DatoReporte == null)
            {
                return NotFound();
            }

            var datoReporte = await _context.DatoReporte.FindAsync(id);
            if (datoReporte == null)
            {
                return NotFound();
            }
            ViewData["DispositivoId"] = new SelectList(_context.Dispositivo, "Id", "Id", datoReporte.DispositivoId);
            return View(datoReporte);
        }

        // POST: DatoReportes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PresionArterial,Temperatura,SaturacionOxigeno,Pulso,FechaHoraUltRegistro,DispositivoId")] DatoReporte datoReporte)
        {
            if (id != datoReporte.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datoReporte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatoReporteExists(datoReporte.Id))
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
            ViewData["DispositivoId"] = new SelectList(_context.Dispositivo, "Id", "Id", datoReporte.DispositivoId);
            return View(datoReporte);
        }

        // GET: DatoReportes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DatoReporte == null)
            {
                return NotFound();
            }

            var datoReporte = await _context.DatoReporte
                .Include(d => d.Dispositivo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datoReporte == null)
            {
                return NotFound();
            }

            return View(datoReporte);
        }

        // POST: DatoReportes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DatoReporte == null)
            {
                return Problem("Entity set 'ObligatorioContext.DatoReporte'  is null.");
            }
            var datoReporte = await _context.DatoReporte.FindAsync(id);
            if (datoReporte != null)
            {
                _context.DatoReporte.Remove(datoReporte);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatoReporteExists(int id)
        {
          return (_context.DatoReporte?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
