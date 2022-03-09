using System;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations
{
    public static class MsSqlServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredMsSqlDbContext<TUnitOfWork>(this IServiceCollection services, string connectionString,bool isMySql) where TUnitOfWork : DbContext
        {
            if (isMySql)
            {
                services.AddDbContextPool<TUnitOfWork>((serviceProvider, optionsBuilder) =>
                    optionsBuilder
                        .UseMySQL(
                            connectionString,
                            sqlServerOptionsBuilder =>
                            {
                                sqlServerOptionsBuilder
                                    .CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
                                    .MigrationsAssembly(typeof(MsSqlServiceCollectionExtensions).Assembly.FullName);
                            })
                        .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));
                return services;
            }
            else
            {
                services.AddDbContextPool<TUnitOfWork>((serviceProvider, optionsBuilder) =>
                    optionsBuilder
                        .UseSqlServer(
                            connectionString,
                            sqlServerOptionsBuilder =>
                            {
                                sqlServerOptionsBuilder
                                    .CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
                                    .EnableRetryOnFailure()
                                    .UseNetTopologySuite()
                                    .MigrationsAssembly(typeof(MsSqlServiceCollectionExtensions).Assembly.FullName);
                                    
                            })
                        .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));
                return services;
            }
        }
    }
}