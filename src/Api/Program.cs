using Modules.Common.Application;
using Modules.Common.Presentation.Endpoints;
using Modules.Common.Infrastructure;
using SkillHive.Api.Extensions;
using Modules.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// pass the application layer assemblues here
// to use mediatr and fluent validation 
builder.Services.AddApplication(
    Modules.Users.Application.ApplicationRefrence.Assembly
    );
// pass the presentation layer assemblies here
// pass the Configuration Methods for masstransit if available in case of consuming events 
// pass the reddis connection string to use it via the cache service when needed
builder.Services.AddInfrastructure([], builder.Configuration);
// adding module config for all the modules 
builder.Configuration.AddModuleConfiguration("users");
// adding modules infrastructure
builder.Services.AddUsersModule(builder.Configuration);
var app = builder.Build();

app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].FirstOrDefault();
    Console.WriteLine($"Received Token: {token}");
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();
app.UseHttpsRedirection();
app.Run();
