using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.Common.Application.Caching;
using Modules.Common.Application.EventBus;
using Modules.Common.Infrastructure.Caching.DistributedCache;
using Modules.Common.Infrastructure.interceptors;
using Modules.Common.Infrastructure.Authentication;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Modules.Common.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static void AddInfrastructure(
            this IServiceCollection services,
            Action<IRegistrationConfigurator>[] moduleConfigureConsumers,
            string redisConnectionString)
        {
            #region redis cache Server setup
            //IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect( redisConnectionString );
            //services.AddStackExchangeRedisCache( 
            //    options => 
            //    options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer) );
            //services.TryAddSingleton<ICacheService, DistributedCacheService>();
            #endregion

            #region memory cache
            services.AddDistributedMemoryCache();
            services.TryAddSingleton<ICacheService, CacheService>();
            #endregion

            #region Jwt Configure
            services.ConfigureOptions<JwtBearerOptionsSetup>();
            services.ConfigureOptions<JwtOptionsSetup>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddAuthorization();
            #endregion 

            // Interceptor That Publishes Domain Events
            services.TryAddSingleton<PublishDomainEventsInterceptors>();

            // Event bus interface for massTransit can be linked to RabbitMq when needed
            services.TryAddSingleton<IEventBus, EventBus>();
            services.AddMassTransit(config =>
            {
                foreach (var consumer in moduleConfigureConsumers)
                    consumer(config);
                config.SetKebabCaseEndpointNameFormatter();
                config.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
