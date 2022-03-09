using System;
using System.Linq;
using System.Resources;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Interfaces;

namespace Infrastructure.Resources
{
    public interface IValidationFactory : IService
    {
        string Get(string key);
    }

    public interface IResourcesFactory : IService
    {
        public ResourceManager ResourceManager { get; }
    }



    public class ValidationFactory : IValidationFactory
    {
        private readonly IResourcesFactory _resourcesFactory;
        public ValidationFactory(IResourcesFactory resourcesFactory)
        {
            _resourcesFactory = resourcesFactory;
        }

        public string Get(string key)
        {
            return _resourcesFactory.ResourceManager.GetString(key);
        }
    }


    public class ResourcesFactory : IResourcesFactory
    {
        public ResourceManager ResourceManager { get; }
        public ResourcesFactory(ICurrentUserAccessor currentUserAccessor)
        {
            ResourceManager = new ResourceManager($"Infrastructure.Resources.ValidationMsg.Validations_{currentUserAccessor.GetCultureTwoIsoName()}",
                AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName != null && x.FullName.Contains("Infrastructure")));
        }
    }
}