using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Redis;
using Service.Interfaces;
using Service.Services.SmsService;
using VoipServer.Data.Context.MySql;
using VoipServer.Hubs;
using VoipServer.Worker;
using Repository = VoipServer.Data.Repository.Repository;

namespace VoipServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
           // services.AuditService(new ConfigurationAccessor(_configuration));
            DependencyInjector.LoadConfigurations(new ConfigurationAccessor(_configuration));
            services.IncludeBaseServices();
            services.IncludeCurrentUserAccessor();

            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });

            services.AddSignalR(x => { x.EnableDetailedErrors = true; });

            services.AddScoped<IScopedProcessingService, DefaultScopedProcessingService>();

            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
                hubOptions.MaximumParallelInvocationsPerClient = 10;
                hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(5);
            });

            services.AddScoped<IVoipMySqlUnitOfWork, VoipMySqlUnitOfWork>();
            services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();
            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddDbContext<VoipMySqlUnitOfWork>(options => options.UseMySQL(new ConfigurationAccessor(_configuration).GetConnectionString().MySqlString), ServiceLifetime.Transient);

            //services.UseCaching<VoipMySqlUnitOfWork>(new[]
            //{
            //    typeof(crm), typeof(cdr)
            //}, true);

            //services.UseCaching<VoipUnitOfWork>(new[]
            //{
            //    typeof(Operator), typeof(Customer), typeof(Person)
            //});


            services.AddDbContext<ApplicationUnitOfWork>(options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString().DefaultString, opt => opt.UseNetTopologySuite()), ServiceLifetime.Transient);

            services.AddScoped<IHierarchicalController, HierarchicalController>();

            services.AddScoped<IRepository, Repository>();

            services.AddHostedService<VoipServiceBackgroundWorker>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.SetIsOriginAllowed((host) => true)
                    .AllowCredentials();

               // builder.AllowAnyOrigin();

                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            }));
            //services.IncludeCorsPolicy();
            services.IncludeOAouth();
            services.IncludeSwagger();
            services.IncludeRedis<RedisDataProvider>();
            services.IncludeDomainServices();
            services.AddDataProtection()
                .ProtectKeysWithDpapi(protectToLocalMachine: true).PersistKeysToFileSystem(new DirectoryInfo(@"c:\temp-keys"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor contextAccessor)
        {
            AppSettings.LoadConfigurations(new ConfigurationAccessor(_configuration));
            AppSettings.LoadHttpContext(contextAccessor);
            app.IncludeBuffering();
            //app.IncludeAuditMiddleware();
            app.IncludeExceptionMiddleware();
            app.IncludeCorsPolicy();
            app.IncludeSwagger();
            app.IncludeRouting();
            app.UseFileServer();

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.IncludeAuthentication();
            app.IncludeAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapHub<VoipHub>("/voipHub"); });
            app.IncludeEndpoint();
        }
    }
}
