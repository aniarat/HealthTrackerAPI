﻿using Grpc.Core;
using HealthTrackerAPI.Models;
using HealthTrackerAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace HealthTrackerAPI.Controllers;
[Route("api/symptoms")]
[ApiController]
public class SymptomController : ControllerBase
{
    private readonly ISymptomService _symptomService;

    public SymptomController(ISymptomService symptomService)
    {
        this._symptomService = symptomService;
    }


    [HttpGet]
    public async Task<List<Symptom>> GetSymptoms()
    {
        return await _symptomService.GetAllSymptomsAsync();
    }
    [HttpGet("{symptomId}")]
    public async Task<ActionResult<Symptom>> GetSymptom(string symptomId)
    {
        var symptom = await _symptomService.GetSymptomByIdAsync(symptomId);
        if (symptom is null)
        {
            return NotFound();
        }
        return symptom;
    }
    [HttpPost]
    public async Task<Symptom> Post(Symptom symptom)
    {
        await _symptomService.AddSymptomAsync(symptom);
        return symptom;
    }
    [HttpPut("{symptomId}")]
    public async Task<IActionResult> Update(string symptomId, SymptomDto updatedSymptom)
    {
        var symptom = await _symptomService.GetSymptomByIdAsync(symptomId);
        if (symptom is null)
        {
            return NotFound();
        }
        //updatedSymptom.Id = symptom.Id;
        await _symptomService.UpdateSymptomAsync(symptomId, updatedSymptom);
        return Ok();
    }
    [HttpDelete("{symptomId}")]
    public async Task<IActionResult> Delete(string symptomId)
    {
        var symptom = await _symptomService.GetSymptomByIdAsync(symptomId);
        if (symptom is null)
        {
            return NotFound();
        }
        await _symptomService.DeleteSymptomAsync(symptomId);
        return Ok();
    }
    
    [HttpGet("history")]
    public async Task<List<SymptomHistory>> GetSymptomsHistory()
    {
        return await _symptomService.GetSymptomHistoryAsync();
    }

    [HttpGet("{symptomId}/history")]
    public async Task<List<SymptomHistory>> GetSymptomHistoryById(string symptomId)
    {
        var symptomHistory = await _symptomService.GetSymptomHistoryByIdAsync(symptomId);

        return symptomHistory;
    }



}