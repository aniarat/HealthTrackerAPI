using HealthTrackerAPI.Models;
using MongoDB.Driver;


namespace HealthTrackerAPI.Services;

public interface ISymptomHistoryService
{
    public Task<List<SymptomHistory>> GetSymptomHistoryAsync();
    //public Task<SymptomHistory> GetSymptomByIdAsync(string symptomId);
    Task<List<SymptomHistory>> GetSymptomHistoryByIdAsync(string symptomId);



}