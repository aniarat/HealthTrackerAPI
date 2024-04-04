using System.Text;
using HealthTrackerAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HealthTrackerAPI.Services;

public class SymptomService : ISymptomService
{
    
    private readonly IMongoCollection<Symptom> _symptomsCollection;
    private readonly IMongoCollection<SymptomHistory> _symptomHistoryCollection;


    public SymptomService(
        IOptions<MongoDbSettings> mongoDbSettings, IMongoClient client)
    {
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _symptomsCollection = database.GetCollection<Symptom>(mongoDbSettings.Value.SymptomCollectionName);
        _symptomHistoryCollection = database.GetCollection<SymptomHistory>(mongoDbSettings.Value.SymptomHistoryCollectionName);
    }

    public async Task<Symptom> GetSymptomByIdAsync(string symptomId)
    {
        var objectId = ObjectId.Parse(symptomId); 

        return await _symptomsCollection.Find(x => x.Id == objectId).FirstOrDefaultAsync();
    }

    public async Task<List<Symptom>> GetAllSymptomsAsync()
    {
        return await _symptomsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Symptom> AddSymptomAsync(Symptom symptom)
    {
        var existingSymptomsOfType = await _symptomsCollection.Find(s => s.PainType == symptom.PainType).ToListAsync();
        var occurrenceCounter = existingSymptomsOfType.Count + 1;
        symptom.OccurrenceCounter = occurrenceCounter;

       await _symptomsCollection.InsertOneAsync(symptom);
       return symptom;
    }
    

    public async Task<DeleteResult> DeleteSymptomAsync(string symptomId)
    {
        var objectId = ObjectId.Parse(symptomId); 
        return await _symptomsCollection.DeleteOneAsync(x => x.Id == objectId);
    }
    
    public async Task UpdateSymptomAsync(string symptomId, SymptomDto updatedSymptom)
    {
        var objectId = ObjectId.Parse(symptomId); // Konwertuj string na ObjectId

        var filter = Builders<Symptom>.Filter.Eq(s => s.Id, objectId);
        var oldSymptom = await _symptomsCollection.Find(filter).FirstOrDefaultAsync();
        if (oldSymptom != null)
        {
            var modifiedFields = new List<string>();

            if (oldSymptom.SeverityScale != updatedSymptom.SeverityScale)
                modifiedFields.Add("SeverityScale");
            if (oldSymptom.SymptomDurationHours != updatedSymptom.SymptomDurationHours)
                modifiedFields.Add("SymptomDurationHours");

            foreach (var fieldName in modifiedFields)
            {
                var oldValue = oldSymptom.GetType().GetProperty(fieldName)?.GetValue(oldSymptom)?.ToString();
                var newValue = updatedSymptom.GetType().GetProperty(fieldName)?.GetValue(updatedSymptom)?.ToString();

                await SaveSymptomHistoryAsync(objectId, fieldName, oldValue, newValue);
            }

            // Update symptom fields
            oldSymptom.SeverityScale = updatedSymptom.SeverityScale;
            oldSymptom.SymptomDurationHours = updatedSymptom.SymptomDurationHours;

            // Save changes to symptoms collection
            await _symptomsCollection.ReplaceOneAsync(filter, oldSymptom);
        }
        else
        {
            throw new Exception("Symptom not found");
        }
    }

    private async Task SaveSymptomHistoryAsync(ObjectId symptomId, string fieldName, string oldValue, string newValue)
    {

        var history = new SymptomHistory
        {
            SymptomId = symptomId,
            FieldName = fieldName,
            OldValue = oldValue,
            NewValue = newValue,
            ModifiedDate = DateTime.Now
        };

        await _symptomHistoryCollection.InsertOneAsync(history);
    }
    
    public async Task<List<SymptomHistory>> GetSymptomHistoryAsync()
    {
        return await _symptomHistoryCollection.Find(_ => true).ToListAsync();
    }

    public async Task<List<SymptomHistory>> GetSymptomHistoryByIdAsync(string symptomId)
    {
        var objectId = ObjectId.Parse(symptomId); 
        var filter = Builders<SymptomHistory>.Filter.Eq(s => s.SymptomId, objectId);
        var histories = await _symptomHistoryCollection.Find(filter).ToListAsync();
        return histories.Select(h => new SymptomHistory
        {
            FieldName = h.FieldName,
            OldValue = h.OldValue,
            NewValue = h.NewValue,
            ModifiedDate = h.ModifiedDate
        }).ToList();
    }
}