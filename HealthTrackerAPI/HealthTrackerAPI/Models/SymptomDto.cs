namespace HealthTrackerAPI.Models;

public class SymptomDto
{
    public int SeverityScale { get; set; } // Severity scale of the symptom, ranging from 1 to 10
    public int? SymptomDurationHours { get; set; } // Duration of symptom in hours

}