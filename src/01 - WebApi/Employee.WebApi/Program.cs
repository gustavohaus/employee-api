using Employee.Application;
using Employee.Application.Employee.CreateEmployee;
using Employee.Common.Logging;
using Employee.Common.Security;
using Employee.Common.Validation;
using Employee.CrossCutting.IoC;
using Employee.Data;
using Employee.WebApi.Middleware;
using MediatR;



var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<DefaultContext>();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.RegisterDependencies();

//builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeHandler).Assembly));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


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
