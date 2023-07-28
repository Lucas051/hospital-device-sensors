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
    public class UAdministradoresController : Controller
    {
        private readonly ObligatorioContext _context;

        public UAdministradoresController(ObligatorioContext context)
        {
            _context = context;
        }

        // GET: UAdministradores
        public async Task<IActionResult> Index()
        {
              return _context.Administrador != null ? 
                          View(await _context.Administrador.ToListAsync()) :
                          Problem("Entity set 'ObligatorioContext.Administrador'  is null.");
        }

        // GET: UAdministradores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Administrador == null)
            {
                return NotFound();
            }

            var uAdministrador = await _context.Administrador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uAdministrador == null)
            {
                return NotFound();
            }
            
            return View(uAdministrador);
        }

        // GET: UAdministradores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UAdministradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreUsuario,Contraseña,Email,NombreApellido,Telefono,Direccion,Rol")] UAdministrador uAdministrador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uAdministrador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uAdministrador);
        }

        // GET: UAdministradores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Administrador == null)
            {
                return NotFound();
            }

            var uAdministrador = await _context.Administrador.FindAsync(id);
            if (uAdministrador == null)
            {
                return NotFound();
            }
            return View(uAdministrador);
        }

        // POST: UAdministradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,Contraseña,Email,NombreApellido,Telefono,Direccion,Rol")] UAdministrador uAdministrador)
        {
            if (id != uAdministrador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uAdministrador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UAdministradorExists(uAdministrador.Id))
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
            return View(uAdministrador);
        }

        // GET: UAdministradores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Administrador == null)
            {
                return NotFound();
            }

            var uAdministrador = await _context.Administrador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uAdministrador == null)
            {
                return NotFound();
            }

            return View(uAdministrador);
        }

        // POST: UAdministradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Administrador == null)
            {
                return Problem("Entity set 'ObligatorioContext.Administrador'  is null.");
            }
            var uAdministrador = await _context.Administrador.FindAsync(id);
            if (uAdministrador != null)
            {
                _context.Administrador.Remove(uAdministrador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UAdministradorExists(int id)
        {
          return (_context.Administrador?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
