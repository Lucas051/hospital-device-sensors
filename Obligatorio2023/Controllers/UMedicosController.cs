using System;
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
    [Authorize(Roles = "Administrador")]
    public class UMedicosController : Controller
    {
        private readonly ObligatorioContext _context;

        public UMedicosController(ObligatorioContext context)
        {
            _context = context;
        }

        // GET: UMedicos
        public async Task<IActionResult> Index()
        {
              return _context.UMedico != null ? 
                          View(await _context.UMedico.ToListAsync()) :
                          Problem("Entity set 'ObligatorioContext.UMedico'  is null.");
        }

        // GET: UMedicos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.UMedico == null)
            {
                return NotFound();
            }

            var uMedico = await _context.UMedico
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uMedico == null)
            {
                return NotFound();
            }

            return View(uMedico);
        }

        // GET: UMedicos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UMedicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Matricula,Especialidad,Id,NombreUsuario,Contraseña,Email,NombreApellido,Telefono,Direccion")] UMedico uMedico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uMedico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uMedico);
        }

        // GET: UMedicos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.UMedico == null)
            {
                return NotFound();
            }

            var uMedico = await _context.UMedico.FindAsync(id);
            if (uMedico == null)
            {
                return NotFound();
            }
            return View(uMedico);
        }

        // POST: UMedicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Matricula,Especialidad,Id,NombreUsuario,Contraseña,Email,NombreApellido,Telefono,Direccion")] UMedico uMedico)
        {
            if (id != uMedico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uMedico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UMedicoExists(uMedico.Id))
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
            return View(uMedico);
        }

        // GET: UMedicos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.UMedico == null)
            {
                return NotFound();
            }

            var uMedico = await _context.UMedico
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uMedico == null)
            {
                return NotFound();
            }

            return View(uMedico);
        }

        // POST: UMedicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.UMedico == null)
            {
                return Problem("Entity set 'ObligatorioContext.UMedico'  is null.");
            }
            var uMedico = await _context.UMedico.FindAsync(id);
            if (uMedico != null)
            {
                _context.UMedico.Remove(uMedico);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UMedicoExists(Guid id)
        {
          return (_context.UMedico?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
