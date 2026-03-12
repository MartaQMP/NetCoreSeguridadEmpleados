using Microsoft.AspNetCore.Authorization;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Policies
{
    public class OverSubordinadosRequirement: AuthorizationHandler<OverSalarioRequirement>, IAuthorizationRequirement
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OverSalarioRequirement requirement)
        {
            var httpContext = context.Resource as DefaultHttpContext;
            if(httpContext != null)
            {
                int id = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var repo = httpContext.RequestServices.GetRequiredService<RepositoryHospital>();
                List<Empleado> data = await repo.GetSubordinados(id);
                
                if (data.Count() > 0)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}
