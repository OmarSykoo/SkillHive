using System.Net;
using System.Net.Mail;
using System.Text.Json;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.Common.Infrastructure.interceptors;
using Modules.Common.Presentation.Endpoints;
using Modules.Users.Application;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Services;
using Modules.Users.Domain;
using Modules.Users.Domain.Repositories;
using Modules.Users.Infrastructure.Authentication;
using Modules.Users.Infrastructure.Data;
using Modules.Users.Infrastructure.Interceptors;
using Modules.Users.Infrastructure.Options;
using Modules.Users.Infrastructure.OutBox;
using Modules.Users.Infrastructure.Repositories;
using Modules.Users.Infrastructure.Services;
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
        string DbConnectionString = configuration["Users:ConnectionStrings:UsersDb"]!;
        string QuadrantGrpc = configuration["Users:ConnectionStrings:QuadrantGrpc"]!;
        string FaceNetApi = configuration["Users:ConnectionStrings:FaceNetApi"]!;
        services.AddOptions<GmailSmtpOptions>()
            .BindConfiguration("Users:GmailConfig")
            .ValidateDataAnnotations();
        services.AddOptions<OutBoxOptions>()
            .BindConfiguration("Users:OutboxOptions")
            .ValidateDataAnnotations();
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
        var GmailOptions = configuration.GetSection("Users:GmailConfig");

        services
            .AddFluentEmail(GmailOptions["SenderEmail"])
            .AddSmtpSender(
                GmailOptions["SmtpServer"],
                GmailOptions.GetValue<int>("Port"),
                GmailOptions["SenderEmail"],
                GmailOptions["SenderPassword"]);


        services.AddDbContext<UserDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                DbConnectionString,
                options
                    => options.MigrationsAssembly(
                        Modules.Users.Infrastructure.AssemblyRefrence.Assembly)
            );
            options.AddInterceptors(sp.GetRequiredService<PublishOutboxMessagesInterceptor>());
        });

        services.AddScoped<QdrantClient>(sp =>
        {
            return new QdrantClient("localhost", 6334);
        });
        services.TryAddSingleton<PublishOutboxMessagesInterceptor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
        services.AddScoped<IRefreshRepository, RefreshTokenRepository>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IDbConnectionFactory>(sp => new DbConnectionFactory(DbConnectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUnverifiedUserRepository, UnverifiedUserRepository>();
        services.AddScoped<IFaceEmbedingRepository, FaceEmbedingRepository>();
        services.AddScoped<IFaceModelService>(sp => new FaceModelService(FaceNetApi));
        services.AddScoped<IEmailService, EmailService>();
        services.Decorate(typeof(INotificationHandler<>), typeof(OutboxIdempotentDomainEventHandlerDecorator<>));
    }
}
