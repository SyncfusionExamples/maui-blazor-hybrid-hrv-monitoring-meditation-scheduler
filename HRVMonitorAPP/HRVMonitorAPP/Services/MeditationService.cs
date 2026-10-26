using HRVMonitorAPP.Models;

namespace HRVMonitorAPP.Services;

public interface IMeditationService
{
    Task<MeditationSchedule?> GenerateRecommendationAsync();
    Task<List<MeditationSchedule>> GetSchedulesAsync();
}

public sealed class MeditationService : IMeditationService
{
    private readonly IHrvRepository _repo;
    public MeditationService(IHrvRepository repo) => _repo = repo;

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

    public Task<List<MeditationSchedule>> GetSchedulesAsync() => _repo.GetSchedulesAsync();
}
