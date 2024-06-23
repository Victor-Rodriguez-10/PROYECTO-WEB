using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentaDeVehiculos.Contexto;
using VentaDeVehiculos.Models;

namespace VentaDeVehiculos.Controllers
{
    public class VentasController : Controller
    {
        private readonly MyContext _context;

        public VentasController(MyContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var myContext = _context.Ventas.Include(v => v.Cliente).Include(v => v.Usuario).Include(v => v.Vehiculos);
            return View(await myContext.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Vehiculos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Inf");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,ClienteId,VehiculoId,Total")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                venta.Fecha = DateOnly.FromDateTime(DateTime.Now);
                venta.Num_recibo = GetNumero();
                var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(m => m.Id == venta.VehiculoId);
                if (vehiculo.Stock > 1)
                {
                    vehiculo.Stock = vehiculo.Stock - 1;
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                    _context.Add(venta);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                TempData["VentaError"] = "No se tiene stock de este vehiculo ( Vehiculos no disponibles)";
                return RedirectToAction("Create", "Ventas");
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", venta.ClienteId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", venta.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id", venta.VehiculoId);
            return View(venta);
        }

        private Vehiculo GetVehiculo(Venta venta)
        {
            var vehiculo = _context.Vehiculos.Where(X => X.Id == venta.VehiculoId);
            
            return (Vehiculo)vehiculo;
        }

        private int GetNumero()
        {
            if (_context.Ventas.ToList().Count > 0)
                return _context.Ventas.Max(i => i.Num_recibo) + 1;
            return 1;
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Inf");
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,ClienteId,VehiculoId,Total,FotoFile")] Venta venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingVenta = await _context.Ventas.FindAsync(id);

                if (existingVenta == null)
                {
                    return NotFound();
                }

                venta.Fecha = DateOnly.FromDateTime(DateTime.Now);
                venta.Num_recibo = GetNumero();

                existingVenta.UsuarioId = venta.UsuarioId;
                existingVenta.ClienteId = venta.ClienteId;
                existingVenta.VehiculoId = venta.VehiculoId;
                existingVenta.Total = venta.Total;
                existingVenta.Fecha = venta.Fecha;
                existingVenta.Num_recibo = venta.Num_recibo;
                try
                {
                    _context.Attach(existingVenta);
                    _context.Entry(existingVenta).State = EntityState.Modified; 
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", venta.ClienteId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", venta.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id", venta.VehiculoId);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Vehiculos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
    }
}
