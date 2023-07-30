﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        // GET: Dispositivos
        public async Task<IActionResult> Index()
        {
              return _context.Dispositivo != null ? 
                          View(await _context.Dispositivo.ToListAsync()) :
                          Problem("Entity set 'ObligatorioContext.Dispositivo'  is null.");
        }

        // GET: Dispositivos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dispositivo == null)
            {
                return NotFound();
            }

            var dispositivo = await _context.Dispositivo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dispositivo == null)
            {
                return NotFound();
            }

            return View(dispositivo);
        }

        // GET: Dispositivos/Create
        [Authorize(Roles = $"{nameof(UAdministrador)},{nameof(UAdministrador)}")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dispositivos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Detalle,FechaHoraAlta,FechaHoraUltimaModificacion,Activo,UPacienteId,UsuarioId")] Dispositivo dispositivo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispositivo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dispositivo);
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
            return View(dispositivo);
        }

        // POST: Dispositivos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Detalle,FechaHoraAlta,FechaHoraUltimaModificacion,Activo,UPacienteId,UsuarioId")] Dispositivo dispositivo)
        {
            if (id != dispositivo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        private bool DispositivoExists(int id)
        {
          return (_context.Dispositivo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
