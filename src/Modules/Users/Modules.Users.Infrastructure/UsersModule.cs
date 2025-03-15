using System.Data.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modules.Common.Infrastructure.interceptors;
using Modules.Common.Presentation.Endpoints;
using Modules.Users.Application;
using Modules.Users.Application.Abstractions;
using Modules.Users.Infrastructure.Authentication;
using Modules.Users.Infrastructure.Data;

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
        // setting up the database 
        string UsersDbConnectionString = configuration.GetConnectionString("UsersDb")!;
        services.AddDbContext<UserDbContext>((sp, Options) =>
        {
            Options.UseSqlServer(UsersDbConnectionString);
            Options.AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptors>());
        });
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<IUnitOfWork, UserDbContext>();
    }
}
