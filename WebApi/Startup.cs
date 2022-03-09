using System;
using System.Collections.Generic;
using System.IO;
using Application.Behaviors;
using Domain.Entities;
using Domain.Entities.reverse;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Configurations;
using Infrastructure.Configurations.MongoConfigurations;
using Infrastructure.Identity;
using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Persistence.AuditProvider;
using Persistence.Context;
using Persistence.MongoDb;
using Persistence.Redis;
using Persistence.SqlServer;
using Service.AuditMonitoring.AuditMonitoring;
using Service.Interfaces;
using Service.Services.BaseValue;
using Service.Services.BaseValueType;
using Service.Services.Employee;
using Service.Services.Identity;
using Service.Services.Permission;
using Service.Services.Person;
using Service.Services.Position;
using Service.Services.Project;
using Service.Services.Role;
using Service.Services.RolePermission;
using Service.Services.SmsService;
using Service.Services.Unit;
using Service.Services.UnitPosition;
using Service.Services.User;
using Service.Services.UserRole;

namespace WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddResponseCaching();

            services.IncludeMediator(new List<Type>
            {
                typeof(RequestValidationBehavior<,>),
                typeof(RepositoryBehavior<,>)
            });

            services.IncludeFluentValidator();


            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            //services.AuditService(new ConfigurationAccessor(_configuration));

            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();
            DependencyInjector.LoadConfigurations(new ConfigurationAccessor(_configuration));
            services.IncludeBaseServices();


            services.IncludeCurrentUserAccessor();

            services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();

            services.UseCaching<ApplicationUnitOfWork>(new[]
            {
                typeof(BaseValue),typeof(BaseValueType)
            });

            //services.AddDbContext<ApplicationUnitOfWork>(options => options.UseSqlServer(new ConfigurationAccessor(_configuration).GetConnectionString().DefaultString,opt=>opt.UseNetTopologySuite()), ServiceLifetime.Transient);

            services.AddScoped<IHierarchicalController, HierarchicalController>();
            services.AddScoped<IRepository, Repository>();
            services.IncludeRedis<RedisDataProvider>();
            services.IncludeAutoMapper();

            // services.AddScoped<IReportingService, ReportingService>();
            services.AddScoped<IConfigurationAccessor, ConfigurationAccessor>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IMongoDataProvider, MongoDataProvider>();
            services.AddScoped<IAuditMontiroringRepository, AuditMonitoringRepository>();
            services.AddScoped<IAuditMonitoringService, AuditMonitoringService>();

            services.AddScoped<IUpLoader, UpLoader>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUnitPositionService, UnitPositionService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IBaseValueService, BaseValueService>();
            services.AddScoped<IBaseValueTypeService, BaseValueTypeService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IApplicationUnitOfWorkProcedures, ApplicationUnitOfWorkProcedures>();

            services.IncludeCorsPolicy();
            services.IncludeOAouth();
            services.IncludeSwagger();
            services.IncludeDomainServices();
            services.IncludeAutoMapper();
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.IncludeAutoMapper();
            services.AddDataProtection()
                .ProtectKeysWithDpapi(protectToLocalMachine: true).PersistKeysToFileSystem(new DirectoryInfo(@"c:\temp-keys"));
        }

        public void Configure(IApplicationBuilder app, IHttpContextAccessor contextAccessor, IWebHostEnvironment env)
        {
            AppSettings.LoadConfigurations(new ConfigurationAccessor(_configuration));
            AppSettings.LoadHttpContext(contextAccessor);
            app.IncludeBuffering();
            // app.UseAuditMiddleware();
            // app.UseAuditCustomAction(new CurrentUserAccessor(new HttpContextAccessor()));
            app.IncludeExceptionMiddleware();
            app.IncludeCorsPolicy();

            //app.UseResponseCaching();
            //app.Use(async (context, next) =>
            //{
            //    context.Response.GetTypedHeaders().CacheControl =
            //        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //        {
            //            Public = true,
            //            MaxAge = TimeSpan.FromSeconds(10)
            //        };
            //    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            //        new string[] { "CensusesSp" };

            //    await next();
            //});


            app.IncludeSwagger();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Data")),
                RequestPath = "/assets"
            });


            app.IncludeRouting();
            app.IncludeAuthentication();
            app.IncludeAuthorization();
            app.IncludeEndpoint();
            app.IncludeRequestResourcesLocalization();
        }
    }
}
