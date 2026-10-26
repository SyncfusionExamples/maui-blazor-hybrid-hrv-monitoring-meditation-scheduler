using HRVMonitorAPP.Models;

namespace HRVMonitorAPP.Services;

/// <summary>Meditation recommendation engine and schedule management.</summary>
public interface IMeditationService
{
    /// <summary>Creates and stores a recommendation from recent HRV readings; null when no data exists.</summary>
    Task<MeditationSchedule?> GenerateRecommendationAsync();

    /// <summary>Returns all stored recommendations, newest first.</summary>
    Task<List<MeditationSchedule>> GetSchedulesAsync();

    /// <summary>Persists changes to an existing recommendation.</summary>
    /// <param name="s">The updated recommendation.</param>
    Task UpdateAsync(MeditationSchedule s);

    /// <summary>Deletes a recommendation.</summary>
    /// <param name="id">Primary key of the recommendation.</param>
    Task DeleteAsync(int id);
}

/// <summary>Mock rule-based engine: RMSSD &lt; 30 recovery, 30-50 mindfulness, &gt; 50 maintenance.</summary>
public sealed class MeditationService : IMeditationService
{
    private readonly IHrvRepository _repo;
    /// <summary>Creates the service.</summary>
    /// <param name="repo">Persistence layer.</param>
    public MeditationService(IHrvRepository repo) => _repo = repo;

    /// <inheritdoc />
    public async Task<MeditationSchedule?> GenerateRecommendationAsync()
    {
        var recent = await _repo.GetLatestAsync(60);
        if (recent.Count == 0) return null;
        var avg = Math.Round(recent.Average(r => r.RMSSD), 1);
        var now = DateTime.Now;
        var (type, minutes, reason) = avg switch
        {
            < 30 => ("20 Minute Stress Recovery Session", 20, $"Average RMSSD {avg} ms is below 30 (elevated stress)."),
            <= 50 => ("10 Minute Mindfulness Session", 10, $"Average RMSSD {avg} ms is in the 30-50 range (moderate recovery)."),
            _ => ("Maintenance Session", 10, $"Average RMSSD {avg} ms is above 50 (good recovery).")
        };
        var s = new MeditationSchedule
        {
            CreatedDate = now,
            RecommendedTime = now.AddMinutes(30),
            Duration = minutes,
            RecommendationType = type,
            Reason = reason + $" Based on last {recent.Count} readings."
        };
        await _repo.AddScheduleAsync(s);
        return s;
    }

    /// <inheritdoc />
    public Task<List<MeditationSchedule>> GetSchedulesAsync() => _repo.GetSchedulesAsync();
    /// <inheritdoc />
    public Task UpdateAsync(MeditationSchedule s) => _repo.UpdateScheduleAsync(s);
    /// <inheritdoc />
    public Task DeleteAsync(int id) => _repo.DeleteScheduleAsync(id);
}
