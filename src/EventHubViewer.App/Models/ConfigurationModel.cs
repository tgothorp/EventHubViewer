using EventHubViewer.App.Infrastructure.Database;

namespace EventHubViewer.App.Models
{
    public class ConfigurationModel : BaseModel
    {
        public Configuration Configuration { get; set; }

        public ConfigurationModel() : base("Configuration") { }
        
        public ConfigurationModel(Configuration configuration) : base("Configuraton")
        {
            Configuration = configuration;
        }
    }
}