using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;


namespace Employee.CrossCutting.IoC.ModuleInitializers
{
    public class WebApiModuleInitializer : IModuleInitializer
    {
        [ExcludeFromCodeCoverage]
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
        }
    }
}
