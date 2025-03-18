using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.Domain.Exceptions;
using Modules.Common.Infrastructure.interceptors;
using Modules.Common.Presentation.Endpoints;
using Modules.Users.Application;
using Modules.Users.Application.Abstractions;
using Modules.Users.Domain;
using Modules.Users.Domain.Repositories;
using Modules.Users.Infrastructure.Authentication;
using Modules.Users.Infrastructure.Data;
using Modules.Users.Infrastructure.Repositories;
using Qdrant.Client;

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
        string QuadrantGrpc = configuration.GetConnectionString("QuadrantGrpc")!;
        services.AddDbContext<UserDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                DbConnectionString,
                options
                    => options.MigrationsAssembly(
                        Modules.Users.Infrastructure.AssemblyRefrence.Assembly)
            );
            options.AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptors>());
        });
        services.AddScoped<QdrantClient>(sp =>
        {
            if (Uri.TryCreate(QuadrantGrpc, UriKind.Absolute, out var result))
            {
                return new QdrantClient(result.Host, result.Port);
            }
            throw new SkillHiveException("Couldn't parse the url of the QdrantClient Grpc connection string in the Users module");
        });
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshRepository, RefreshTokenRepository>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IDbConnectionFactory>(sp => new DbConnectionFactory(DbConnectionString));
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UserDbContext>());
        services.AddScoped<IFaceEmbedingRepository, FaceEmbedingRepository>();
    }
}
