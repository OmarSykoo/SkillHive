using Modules.Common.Application;
using Modules.Common.Presentation.Endpoints;
using Modules.Common.Infrastructure;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// pass the application layer assemblues here
// to use mediatr and fluent validation 
builder.Services.AddApplication(Modules.Users.Application.ApplicationRefrence.Assembly);
// pass the presentation layer assemblies here
// to map endpoints
builder.Services.AddEndpoints();
// pass the Configuration Methods for masstransit if available in case of consuming events 
// pass the reddis connection string to use it via the cache service when needed
builder.Services.AddInfrastructure([], "");
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();
app.UseHttpsRedirection();
app.Run();
