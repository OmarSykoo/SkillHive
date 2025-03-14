using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.Presentation.Endpoints;
using Modules.Users.Application.Abstractions;
using Modules.Users.Infrastructure.Authentication;

namespace Modules.Users.Infrastructure;

public static class UsersModule
{
    public static void AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(Modules.Users.Presentation.AssemblyRefrence.Assembly);
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Adding Jwt Provider Service for Authentication
        services.AddScoped<IJwtProvider, JwtProvider>();
    }
}
