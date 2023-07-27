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
    public class UPacientesController : Controller
    {
        private readonly ObligatorioContext _context;

        public UPacientesController(ObligatorioContext context)
        {
            _context = context;
        }

        // GET: UPacientes
        public async Task<IActionResult> Index()
        {
              return _context.Paciente != null ? 
                          View(await _context.Paciente.ToListAsync()) :
                          Problem("Entity set 'ObligatorioContext.Paciente'  is null.");
        }

        // GET: UPacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Paciente == null)
            {
                return NotFound();
            }

            var uPaciente = await _context.Paciente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uPaciente == null)
            {
                return NotFound();
            }

            return View(uPaciente);
        }

        // GET: UPacientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UPacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FechaNac,TipoSangre,Observaciones,Id,NombreUsuario,Contraseña,Email,NombreApellido,Telefono,Direccion,Rol")] UPaciente uPaciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uPaciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uPaciente);
        }

        // GET: UPacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Paciente == null)
            {
                return NotFound();
            }

            var uPaciente = await _context.Paciente.FindAsync(id);
            if (uPaciente == null)
            {
                return NotFound();
            }
            return View(uPaciente);
        }

        // POST: UPacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FechaNac,TipoSangre,Observaciones,Id,NombreUsuario,Contraseña,Email,NombreApellido,Telefono,Direccion,Rol")] UPaciente uPaciente)
        {
            if (id != uPaciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uPaciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UPacienteExists(uPaciente.Id))
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
            return View(uPaciente);
        }

        // GET: UPacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Paciente == null)
            {
                return NotFound();
            }

            var uPaciente = await _context.Paciente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uPaciente == null)
            {
                return NotFound();
            }

            return View(uPaciente);
        }

        // POST: UPacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Paciente == null)
            {
                return Problem("Entity set 'ObligatorioContext.Paciente'  is null.");
            }
            var uPaciente = await _context.Paciente.FindAsync(id);
            if (uPaciente != null)
            {
                _context.Paciente.Remove(uPaciente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UPacienteExists(int id)
        {
          return (_context.Paciente?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
