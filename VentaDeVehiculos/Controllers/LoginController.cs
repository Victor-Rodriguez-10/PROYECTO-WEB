using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.AspNetCore.Mvc;
using VentaDeVehiculos.Contexto;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using VentaDeVehiculos.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
                await SetAuthenticationInCookie(usuario);   
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // un mensaje temporal 
                TempData["LoginError"] = "Correo o nombre son incorrectos ";
                return RedirectToAction("Index", "Login");
            }
        }

        private async Task SetAuthenticationInCookie(Usuario? user)
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Email!),
                    new Claim("Cuenta", user.Email!),
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Rol!.ToString()),
                };
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        public async Task<IActionResult> Catalogo()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        {
            return RedirectToAction("Index", "Catalogo");
        }



    }
}
