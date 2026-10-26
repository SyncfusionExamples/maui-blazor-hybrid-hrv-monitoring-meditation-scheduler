using SQLite;

namespace HRVMonitorAPP.Models;

/// <summary>A single heart-rate-variability reading. Maps to the <c>HRVMeasurements</c> table.</summary>
[Table("HRVMeasurements")]
public class HrvMeasurement
{
    /// <summary>Auto-generated primary key.</summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>Local time at which the reading was captured.</summary>
    [Indexed]
    public DateTime Timestamp { get; set; }

    /// <summary>Heart rate in beats per minute.</summary>
    public int HeartRate { get; set; }

    /// <summary>Root mean square of successive R-R differences, in milliseconds.</summary>
    public double RMSSD { get; set; }

    /// <summary>Standard deviation of R-R intervals, in milliseconds.</summary>
    public double SDNN { get; set; }

    /// <summary>Percentage of successive R-R intervals differing by more than 50 ms.</summary>
    public double PNN50 { get; set; }

    /// <summary>Derived stress score (0-100, higher means more stress).</summary>
    public double StressScore { get; set; }

    /// <summary>Derived readiness score (0-100, higher means more ready).</summary>
    public double ReadinessScore { get; set; }
}
