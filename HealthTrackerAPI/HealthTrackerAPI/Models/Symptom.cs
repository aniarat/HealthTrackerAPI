using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthTrackerAPI.Models;

public class Symptom
{
    //[BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
        
    public DateTime CreatedAt { get; set; } // Date and time when the symptom was added

    [Required(ErrorMessage = "PainType must be specified.")]
    public PainTypes PainType { get; set; }  // Location of the symptom (e.g., head, abdomen, back)
        
    [Range(1, 10, ErrorMessage = "Severity scale must be a number between 1 and 10.")]
    public int SeverityScale { get; set; } // Severity scale of the symptom, ranging from 1 to 10

    public int? SymptomDurationHours { get; set; } // Duration of symptom in hours
    public int OccurrenceCounter { get; set; }

    
    public enum PainTypes
    {
        Headache,
        Stomachache,
        BackPain,
        AbdominalPain,
        ChestPain,
        MusclePain
    }
}