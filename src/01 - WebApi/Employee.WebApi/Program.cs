using Employee.Application;
using Employee.Application.Employee.CreateEmployee;
using Employee.Common.Logging;
using Employee.Common.Security;
using Employee.Common.Validation;
using Employee.CrossCutting.IoC;
using Employee.Data;
using Employee.WebApi.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DefaultContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddJwtAuthentication(builder.Configuration);

builder.RegisterDependencies();

// Fluent:
builder.Services.AddScoped<IValidator<CreateEmployeeCommand>, CreateEmployeeValidator>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Explicitly register the Mediatr files. (fix this)
builder.Services.AddTransient<IRequestHandler<CreateEmployeeCommand, CreateEmployeeResult>, CreateEmployeeHandler>();


var app = builder.Build();
app.UseMiddleware<ValidationExceptionMiddleware>();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.Run();
