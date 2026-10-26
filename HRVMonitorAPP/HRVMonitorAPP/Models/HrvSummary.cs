namespace HRVMonitorAPP.Models;

public record HrvSummary(double AvgHeartRate, int MaxHeartRate, double AvgRmssd, double MaxRmssd, double AvgStress, double AvgReadiness, int Count)
{
    public static HrvSummary From(IReadOnlyCollection<HrvMeasurement> data) => data.Count == 0
        ? new HrvSummary(0, 0, 0, 0, 0, 0, 0)
        : new HrvSummary(
            Math.Round(data.Average(d => d.HeartRate), 1), data.Max(d => d.HeartRate),
            Math.Round(data.Average(d => d.RMSSD), 1), Math.Round(data.Max(d => d.RMSSD), 1),
            Math.Round(data.Average(d => d.StressScore), 1), Math.Round(data.Average(d => d.ReadinessScore), 1),
            data.Count);
}
