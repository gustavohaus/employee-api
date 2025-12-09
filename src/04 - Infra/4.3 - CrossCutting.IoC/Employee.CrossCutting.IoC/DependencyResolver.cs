using Employee.CrossCutting.IoC.ModuleInitializers;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;

namespace Employee.CrossCutting.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolver
    {
        public static void RegisterDependencies(this WebApplicationBuilder builder)
        {
            new ApplicationModuleInitializer().Initialize(builder);
            new InfrastructureModuleInitializer().Initialize(builder);
            new WebApiModuleInitializer().Initialize(builder);
        }
    }
}
