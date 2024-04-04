using System.Text;
using HealthTrackerAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HealthTrackerAPI.Services;

public class SymptomHistoryService : ISymptomHistoryService
{
    private readonly IMongoCollection<Symptom> _symptomsCollection;
    private readonly IMongoCollection<SymptomHistory> _symptomHistoryCollection;

    public SymptomHistoryService(IMongoDatabase database)
    {
        _symptomsCollection = database.GetCollection<Symptom>("Symptoms");
        _symptomHistoryCollection = database.GetCollection<SymptomHistory>("SymptomHistory");
    }

    public async Task<List<SymptomHistory>> GetSymptomHistoryAsync()
    {
        return await _symptomHistoryCollection.Find(_ => true).ToListAsync();
    }

    public async Task<List<SymptomHistory>> GetSymptomHistoryByIdAsync(string symptomId)
    {
        var objectId = ObjectId.Parse(symptomId); // Konwertuj string na ObjectId
        var filter = Builders<SymptomHistory>.Filter.Eq(s => s.SymptomId, objectId);
        var histories = await _symptomHistoryCollection.Find(filter).ToListAsync();
        //return await _symptomHistoryCollection.Find(x => x.SymptomId == objectId).FirstOrDefaultAsync();
        return histories.Select(h => new SymptomHistory
        {
            FieldName = h.FieldName,
            OldValue = h.OldValue,
            NewValue = h.NewValue,
            ModifiedDate = h.ModifiedDate
        }).ToList();
    }
}