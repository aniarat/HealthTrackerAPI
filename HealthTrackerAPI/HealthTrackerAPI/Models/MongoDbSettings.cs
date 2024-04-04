namespace HealthTrackerAPI.Models;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string SymptomCollectionName { get; set; } = null!;
    public string SymptomHistoryCollectionName { get; set; } = null!;

}