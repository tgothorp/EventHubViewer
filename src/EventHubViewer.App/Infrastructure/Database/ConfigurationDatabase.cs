using System;
using EventHubViewer.App.Infrastructure.Configuration;
using LiteDB;

namespace EventHubViewer.App.Infrastructure.Database
{
    public sealed class ConfigurationDatabase : Database<Configuration>
    {
        public ConfigurationDatabase(IDatabaseConfiguration databaseConfiguration)
        {
            DatabaseConnection = databaseConfiguration.ConfigurationDatabase;
        }

        public Configuration GetConfiguration()
        {
            using (var context = new LiteDatabase(DatabaseConnection))
            {
                var collection = context.GetCollection<Configuration>(nameof(Configuration));
                if (collection.Count() == 0)
                {
                    var configuration = new Configuration();
                    collection.Insert(configuration);
                    return configuration;
                }

                return collection.FindOne(x => x.Id == 1);
            }
        }

        public void UpdateConfiguration(Configuration configuration)
        {
            using (var context = new LiteDatabase(DatabaseConnection))
            {
                var collection = context.GetCollection<Configuration>(nameof(Configuration));
                collection.Update(1, configuration);
            }
        }
    }

    public class Configuration : DatabaseObject
    {
        public string EventHubConnectionString { get; set; }
        public string EvenHubName { get; set; }

        public string BlobStorageConnectionString { get; set; }
        public string BlobContainerName { get; set; }
        
        public Configuration()
        {
            Id = 1;
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }

        public bool CheckConfigurationIsValid()
        {
            // TODO
            return true;
        }
        
    }
}