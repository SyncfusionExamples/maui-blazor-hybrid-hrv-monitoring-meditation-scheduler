using SQLite;

namespace HRVMonitorAPP.Models;

[Table("MeditationSchedules")]
public class MeditationSchedule
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RecommendedTime { get; set; }
    public int Duration { get; set; }
    public string RecommendationType { get; set; } = "";
    public string Reason { get; set; } = "";
}
