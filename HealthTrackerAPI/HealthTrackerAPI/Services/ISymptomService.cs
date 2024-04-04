using HealthTrackerAPI.Models;
using MongoDB.Driver;

namespace HealthTrackerAPI.Repositories;

public interface ISymptomService
{
    public Task<Symptom> GetSymptomByIdAsync(string symptomId);
    public Task<List<Symptom>> GetAllSymptomsAsync();
    public Task<Symptom> AddSymptomAsync(Symptom symptom);
    public Task UpdateSymptomAsync(string symptomId, SymptomDto symptom);
    public Task<DeleteResult> DeleteSymptomAsync(string symptomId);
    public Task<List<SymptomHistory>> GetSymptomHistoryAsync();
    
    Task<List<SymptomHistory>> GetSymptomHistoryByIdAsync(string symptomId);
}