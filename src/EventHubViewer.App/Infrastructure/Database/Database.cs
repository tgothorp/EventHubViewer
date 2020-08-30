using System;

namespace EventHubViewer.App.Infrastructure.Database
{
    public abstract class Database <T> where T : DatabaseObject
    {
        public virtual string DatabaseConnection { get; set; }
    }

    public class DatabaseObject
    {
        public int Id { get; protected set; }

        public DateTime CreatedOn { get; protected set; }
        public DateTime ModifiedOn { get; protected set; }
    }
}