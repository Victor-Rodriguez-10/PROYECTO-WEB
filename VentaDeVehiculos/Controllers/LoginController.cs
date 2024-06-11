using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.AspNetCore.Mvc;
using VentaDeVehiculos.Contexto;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace VentaDeVehiculos.Controllers
{
    public class LoginController : Controller
    {
        MyContext _context;

        public LoginController(MyContext context)
        {
            //inyeccion de dependencias
            this._context = context;
        }
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string email, string password)
        {
            var usuario = _context.Usuarios.
                Where(X => X.Email == email).
                Where(X => X.Password == password).
                FirstOrDefault();

            if (usuario != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // un mensaje temporal 
                TempData["LoginError"] = "Correo o nombre son incorrectos ";
                return RedirectToAction("Index", "Login");
            }
        }

        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Login");
        }



    }
}
