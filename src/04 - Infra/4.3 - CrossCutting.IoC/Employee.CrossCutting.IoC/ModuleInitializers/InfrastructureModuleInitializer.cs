using Employee.Data;
using Employee.Data.Repositories;
using Employee.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;


namespace Employee.CrossCutting.IoC.ModuleInitializers
{
    [ExcludeFromCodeCoverage]
    public class InfrastructureModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        }
    }
}
