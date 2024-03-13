namespace Catalog.Models
{
    public class MongoDBSettings
    {
        // bliver brugt i appsettings.json til mongodb connection
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }
}