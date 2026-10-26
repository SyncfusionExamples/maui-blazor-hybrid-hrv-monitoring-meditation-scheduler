namespace HRVMonitorAPP.Models;

/// <summary>Aggregate statistics over a set of HRV readings.</summary>
/// <param name="AvgHeartRate">Average heart rate (bpm).</param>
/// <param name="MaxHeartRate">Maximum heart rate (bpm).</param>
/// <param name="AvgRmssd">Average RMSSD (ms).</param>
/// <param name="MaxRmssd">Maximum RMSSD (ms).</param>
/// <param name="AvgStress">Average stress score.</param>
/// <param name="AvgReadiness">Average readiness score.</param>
/// <param name="Count">Number of readings summarised.</param>
public record HrvSummary(double AvgHeartRate, int MaxHeartRate, double AvgRmssd, double MaxRmssd, double AvgStress, double AvgReadiness, int Count)
{
    /// <summary>Computes a summary; returns all zeros for an empty collection.</summary>
    /// <param name="data">Readings to aggregate.</param>
    public static HrvSummary From(IReadOnlyCollection<HrvMeasurement> data) => data.Count == 0
        ? new HrvSummary(0, 0, 0, 0, 0, 0, 0)
        : new HrvSummary(
            Math.Round(data.Average(d => d.HeartRate), 1), data.Max(d => d.HeartRate),
            Math.Round(data.Average(d => d.RMSSD), 1), Math.Round(data.Max(d => d.RMSSD), 1),
            Math.Round(data.Average(d => d.StressScore), 1), Math.Round(data.Average(d => d.ReadinessScore), 1),
            data.Count);
}
