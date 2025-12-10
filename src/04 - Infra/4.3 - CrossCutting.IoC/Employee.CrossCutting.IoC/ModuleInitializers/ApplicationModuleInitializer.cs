using Employee.Common.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Employee.CrossCutting.IoC.ModuleInitializers
{
    [ExcludeFromCodeCoverage]
    public class ApplicationModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
        }
    }
}
