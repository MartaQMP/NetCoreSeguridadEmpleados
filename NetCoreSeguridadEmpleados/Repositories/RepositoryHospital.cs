using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Models;

namespace NetCoreSeguridadEmpleados.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> GetEmpleadoByIdAsync(int id)
        {
            return await this.context.Empleados.Where(e => e.IdEmpleado == id).FirstOrDefaultAsync();
        }

        public async Task<List<Empleado>> GetEmpleadosDepartamentoAsync(int id)
        {
            return await this.context.Empleados.Where(e => e.IdDepartamento == id).ToListAsync();
        }

        public async Task UpdateSalarioEmpleadosAsync(int idDept, int incremento)
        {
            List<Empleado> empleados = await this.GetEmpleadosDepartamentoAsync(idDept);
            foreach(Empleado e in empleados)
            {
                e.Salario += incremento;
            }
            await this.context.SaveChangesAsync();
        }

        public async Task<Empleado> LogInEmpleadoAsync(string apellido, int idEmpleado)
        {
            return await this.context.Empleados.Where(e => e.IdEmpleado == idEmpleado && e.Apellido == apellido).FirstOrDefaultAsync();
        }
    }
}
