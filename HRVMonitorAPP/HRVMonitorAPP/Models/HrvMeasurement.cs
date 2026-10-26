using SQLite;

namespace HRVMonitorAPP.Models;

[Table("HRVMeasurements")]
public class HrvMeasurement
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Indexed]
    public DateTime Timestamp { get; set; }
    public int HeartRate { get; set; }
    public double RMSSD { get; set; }
    public double SDNN { get; set; }
    public double PNN50 { get; set; }
    public double StressScore { get; set; }
    public double ReadinessScore { get; set; }
}
