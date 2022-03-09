namespace Infrastructure.ConfigurationAccessor.Models
{
    public class SwaggerConfigurationModel
    {
        public string XmlPath { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public SwaggerContactConfigurationModel Contact { get; set; }

    }
}