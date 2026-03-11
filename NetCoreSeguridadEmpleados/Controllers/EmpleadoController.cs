using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class EmpleadoController : Controller
    {
        private RepositoryHospital repo;

        public EmpleadoController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int id)
        {
            Empleado e = await this.repo.GetEmpleadoByIdAsync(id);
            return View(e);
        }

        [AuthorizeEmpleados]
        public IActionResult PerfilEmpleado()
        {
            return View();
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Compis()
        {
            /* RECUPERAMOS EL CLAIM DEL USUARIO VALIDADO */
            string dato = HttpContext.User.FindFirstValue("Departamento");
            int idDept = int.Parse(dato);
            List<Empleado> emps = await this.repo.GetEmpleadosDepartamentoAsync(idDept);
            return View(emps);
        }
    }
}
