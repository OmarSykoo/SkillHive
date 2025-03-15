using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.Infrastructure.interceptors;
using Modules.Common.Presentation.Endpoints;
using Modules.Users.Application;
using Modules.Users.Application.Abstractions;
using Modules.Users.Domain;
using Modules.Users.Infrastructure.Authentication;
using Modules.Users.Infrastructure.Data;
using Modules.Users.Infrastructure.Repositories;

namespace Modules.Users.Infrastructure;

public static class UsersModule
{
    public static void AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(Modules.Users.Presentation.AssemblyRefrence.Assembly);
        services.AddInfrastructure(configuration);
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string DbConnectionString = configuration.GetConnectionString("UsersDb")!;
        services.AddDbContext<UserDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                DbConnectionString,
                options
                    => options.MigrationsAssembly(
                        Modules.Users.Infrastructure.AssemblyRefrence.Assembly
                        )
            );
            options.AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptors>());
        });
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshRepository, RefreshTokenRepository>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IDbConnectionFactory>(sp => new DbConnectionFactory(DbConnectionString));
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UserDbContext>());
    }
}
