using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CacheManager.Core;
using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Configurations.RedisConfigurations;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;

namespace Infrastructure.Configurations
{
    public static class DependencyInjector
    {
        private static IConfigurationAccessor _configurationAccessor;
        public static void LoadConfigurations(IConfigurationAccessor configurationAccessor)
        {
            _configurationAccessor = configurationAccessor;
        }


        public static void IncludeBaseServices(this IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling =
                            Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                    }
                    );

            services.AddControllers(options =>
            {
                options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(Point)));
                options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(Coordinate)));
                options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(LineString)));
                options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(MultiLineString)));
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                var geoJsonConverterFactory = new GeoJsonConverterFactory();
                options.JsonSerializerOptions.Converters.Add(geoJsonConverterFactory);
            });

            services.AddSingleton(NtsGeometryServices.Instance);


            services.AddMvc();
            services.AddHttpContextAccessor();
        }


        public static void IncludeCurrentUserAccessor(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor.CurrentUserAccessor>();
        }

        public static void IncludeSqlServer<TDbContext, TUnitOfWork>(this IServiceCollection services) where TDbContext : DbContext where TUnitOfWork : IUnitOfWork
        {
            services.AddDbContext<TDbContext>(options => options.UseSqlServer(_configurationAccessor.GetConnectionString().DefaultString), ServiceLifetime.Transient);
        }

        public static void IncludeRedis<TRedisDataProvider>(this IServiceCollection services) where TRedisDataProvider : IRedisDataProvider
        {
            services.AddScoped<IRedisConfig, RedisConfig>(o => new RedisConfig(_configurationAccessor.GetRedisConfiguration().Host, _configurationAccessor.GetRedisConfiguration().Port, _configurationAccessor.GetRedisConfiguration().Password));
            services.AddScoped(typeof(IRedisDataProvider), typeof(TRedisDataProvider));
        }


        public static void IncludeDomainServices(this IServiceCollection services)
        {
            services.IncludeAutoMapper();

            var servicesTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IService).IsAssignableFrom(p) && p.IsClass && p.GetInterfaces().Any(x => x.Name == "I" + p.Name));

            foreach (var type in servicesTypes)
            {
                services.AddScoped(type.GetInterfaces().FirstOrDefault(i => i.Name == "I" + type.Name)!, type);
            }
        }

        public static void IncludeMediator(this IServiceCollection services, List<Type> pipelines)
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            foreach (var pipeline in pipelines)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), pipeline);
            }

        }


     public static void IncludeFluentValidator(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddMvc()
            //    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()))
            //    ;  // it has own pipline  => our validation pipline shall not work any more

        }

        public static void IncludeAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void IncludeOAouth(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configurationAccessor.GetJwtConfiguration().Issuer,
                        ValidAudience = _configurationAccessor.GetJwtConfiguration().Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationAccessor.GetJwtConfiguration().Secret)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        public static void IncludeCorsPolicy(this IServiceCollection services)
        {
            // allows cross origin access to the resources
            services.AddCors(o => o.AddPolicy(_configurationAccessor.GetCorsConfiguration().PolicyName, builder =>
            {
                //if (Convert.ToBoolean(_configurationAccessor.GetCorsConfiguration().AllowAnyOrigin))
                //{
                //    builder.SetIsOriginAllowed((host) => true)
                //        .AllowCredentials();
                //}

              //  builder.AllowAnyOrigin();

                if (Convert.ToBoolean(_configurationAccessor.GetCorsConfiguration().AllowAnyMethod))
                    builder.AllowAnyMethod();
                if (Convert.ToBoolean(_configurationAccessor.GetCorsConfiguration().AllowAnyHeader))
                    builder.AllowAnyHeader();
            }));
            
        }

        public static void IncludeSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_configurationAccessor.GetSwaggerConfiguration().Name, new OpenApiInfo
                {
                    Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                    Title = _configurationAccessor.GetSwaggerConfiguration().Title,
                    Description = _configurationAccessor.GetSwaggerConfiguration().Description,
                    TermsOfService = new Uri(_configurationAccessor.GetSwaggerConfiguration().TermsOfService),
                    Contact = new OpenApiContact
                    {
                        Name = _configurationAccessor.GetSwaggerConfiguration().Contact.Name,
                        Email = _configurationAccessor.GetSwaggerConfiguration().Contact.Email,
                        Url = new Uri(_configurationAccessor.GetSwaggerConfiguration().Contact.Url)
                    }
                });


                c.AddSecurityDefinition("Authorization", new()
                {
                    Description = "",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.Http
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Authorization"
                            }
                        },
                        new string[] {}
                    }
                });

                var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

      
        public static void UseCaching<TUnitOfWork>(this IServiceCollection services, Type[] types,bool isMySql = false) where TUnitOfWork : DbContext
        {
            if (isMySql)
            {
                services.AddConfiguredMsSqlDbContext<TUnitOfWork>(_configurationAccessor.GetConnectionString().MySqlString, isMySql);
            }
            else
            {
                services.AddConfiguredMsSqlDbContext<TUnitOfWork>(_configurationAccessor.GetConnectionString().DefaultString, isMySql);
            }

            var jss = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = { new SpecialTypesConverter() }
            };

            const string redisConfigurationKey = "Redis";
            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new CacheManager.Core.ConfigurationBuilder()
                    .WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
                    .WithUpdateMode(CacheUpdateMode.Up)
                    .WithRedisConfiguration(redisConfigurationKey, config =>
                    {
                        config.WithAllowAdmin()
                            .WithDatabase(1)
                            .WithEndpoint(_configurationAccessor.GetRedisConfiguration().Host, _configurationAccessor.GetRedisConfiguration().Port)
                            .WithPassword(_configurationAccessor.GetRedisConfiguration().Password)
                            .EnableKeyspaceEvents();
                    })
                    .WithMaxRetries(100)
                    .WithRetryTimeout(100)
                    .WithRedisCacheHandle(redisConfigurationKey)
                    .Build());
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));

            services.AddEFSecondLevelCache(options =>
                options.UseCacheManagerCoreProvider().DisableLogging(false).UseCacheKeyPrefix("Kia_")
                    .CacheQueriesContainingTypes(CacheExpirationMode.Sliding, TimeSpan.FromMinutes(60),
                        types)
            );
        }
    }
}