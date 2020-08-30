using Microsoft.Extensions.Configuration;

namespace EventHubViewer.App.Infrastructure.Configuration
{
    public interface IDatabaseConfiguration
    {
        string ConfigurationDatabase { get; set; }
    }
    
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        public DatabaseConfiguration(IConfiguration configuration)
        {
            configuration.Bind("Database", this);
        }
        
        public string ConfigurationDatabase { get; set; }
    }
}