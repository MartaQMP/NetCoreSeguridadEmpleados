using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryHospital repo;

        public ManagedController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string name, string password)
        {
            int idEmpleado = int.Parse(password);
            Empleado e = await this.repo.LogInEmpleadoAsync(name, idEmpleado);
            if(e != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                /* EMPLEADO ARROYO: 7499 SERA NUESTRO ADMIN */
                if(e.IdEmpleado == 7499)
                {
                    Claim claimAdmin = new Claim("Admin", "Soy el amo de la empresa");
                    identity.AddClaim(claimAdmin);
                }
                
                Claim claimName = new Claim(ClaimTypes.Name, name);
                identity.AddClaim(claimName);
                /* COMO POR AHORA NO VAMOS A USAR ROLES, NO LO INDICAMOS */
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, e.IdEmpleado.ToString());
                identity.AddClaim(claimId);
                /* COMO ROL USAMOS EL OFICIO */
                Claim claimRol = new Claim(ClaimTypes.Role, e.Oficio);
                identity.AddClaim(claimRol);
                Claim claimSalario = new Claim("Salario", e.Salario.ToString());
                identity.AddClaim(claimSalario);
                Claim claimDept = new Claim("Departamento", e.IdDepartamento.ToString());
                identity.AddClaim(claimDept);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                if (TempData["id"] != null)
                {
                    string id = TempData["id"].ToString();
                    return RedirectToAction(action, controller, new { id = id });
                }
                else
                {
                    /* POR AHORA LO ENVIAMOS A UNA VISTA QUE HAREMOS EN BREVE */
                    return RedirectToAction(action, controller);
                }
            }
            else
            {
                ViewBag.Mensaje = "Credenciales incorrectas";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Empleado");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
