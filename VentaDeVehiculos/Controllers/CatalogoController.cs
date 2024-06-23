using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VentaDeVehiculos.Contexto;

namespace VentaDeVehiculos.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly MyContext _context;
        IWebHostEnvironment _webHostEnvironment;

        public CatalogoController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Vehiculoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehiculos.ToListAsync());
        }

        public async Task<IActionResult> Login()
        {
            return RedirectToAction("Index", "Login");
        }

    }
}
