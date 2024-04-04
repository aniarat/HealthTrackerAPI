using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthTrackerAPI.Models;

public class SymptomHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)] 
    public ObjectId SymptomId { get; set; }

    public string FieldName { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }
    
    public DateTime ModifiedDate { get; set; }
}