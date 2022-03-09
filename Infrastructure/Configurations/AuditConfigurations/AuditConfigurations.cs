using Audit.Core;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Configurations.MongoConfigurations;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.AuditConfigurations
{
    public static class AuditConfigurations
    {
        public static void AuditService(this IServiceCollection services, IConfigurationAccessor configurationAccessor)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IValuesProvider, ValuesProvider>();
            Audit.Core.Configuration.Setup().UseMongoDb(configurator => configurator
                    .ConnectionString(configurationAccessor.GetAuditMongoDbConfiguration().ConnectionString)
                    .Database(configurationAccessor.GetAuditMongoDbConfiguration().Database)
                    .Collection(configurationAccessor.GetAuditMongoDbConfiguration().Collection)
                    .SerializeAsBson(configurationAccessor.GetAuditMongoDbConfiguration().UseBson))
                .WithCreationPolicy(EventCreationPolicy.InsertOnEnd);
        }

        public static void UseAuditCustomAction(this IApplicationBuilder app,
            ICurrentUserAccessor currentUserAccessor)
        {
            Audit.Core.Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
            {
                scope.Event.CustomFields["Username"] = currentUserAccessor.GetUsername();
                scope.Event.CustomFields["UserId"] = currentUserAccessor.GetId();
                scope.Event.CustomFields["RoleLevelCode"] = currentUserAccessor.GetRoleLevelCode();
                scope.Event.CustomFields["UserRoleId"] = currentUserAccessor.GetRoleId();
            });
        }
    }
}