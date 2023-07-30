﻿using System;
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
            var obligatorioContext = _context.Alarma.Include(a => a.Dispositivo);
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
                .Include(a => a.Dispositivo)
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
            ViewData["DispositivoId"] = new SelectList(_context.Dispositivo, "Id", "Id");
            return View();
        }

        // POST: Alarmas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DatoEvaluar,ValorLimite,Comparador,DispositivoId")] Alarma alarma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alarma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DispositivoId"] = new SelectList(_context.Dispositivo, "Id", "Id", alarma.DispositivoId);
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
            ViewData["DispositivoId"] = new SelectList(_context.Dispositivo, "Id", "Id", alarma.DispositivoId);
            return View(alarma);
        }

        // POST: Alarmas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatoEvaluar,ValorLimite,Comparador,DispositivoId")] Alarma alarma)
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
            ViewData["DispositivoId"] = new SelectList(_context.Dispositivo, "Id", "Id", alarma.DispositivoId);
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
                .Include(a => a.Dispositivo)
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
    }
}