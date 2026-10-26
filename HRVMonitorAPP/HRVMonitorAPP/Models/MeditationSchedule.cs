using SQLite;

namespace HRVMonitorAPP.Models;

/// <summary>A recommended meditation session. Maps to the <c>MeditationSchedules</c> table.</summary>
[Table("MeditationSchedules")]
public class MeditationSchedule
{
    /// <summary>Auto-generated primary key.</summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>When the recommendation was generated.</summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>Start time of the recommended session.</summary>
    public DateTime RecommendedTime { get; set; }

    /// <summary>Session length in minutes.</summary>
    public int Duration { get; set; }

    /// <summary>Session name, e.g. "10 Minute Mindfulness Session".</summary>
    public string RecommendationType { get; set; } = "";

    /// <summary>Human-readable explanation of why the session was suggested.</summary>
    public string Reason { get; set; } = "";
}
