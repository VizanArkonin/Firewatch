using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firewatch.Database.Models;

/// <summary>
/// Since all telemetry tables have exactly the same internal structure (the only difference is the time periods) - 
/// we're going to use the base type for all of them. Generally, that's not a good solution, but in this 
/// particular case it's totally viable - if we make a change to one table, we WILL need that change reflected in the rest
/// </summary>
public abstract class TelemetryBase
{
    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int          EntryId     { get; set; }
    [Column(Order = 2)]
    public int          ResourceId  { get; set; }
    [Column(Order = 3)]
    public int          MetricId    { get; set; }
    [Column(Order = 4)]
    public float        Value       { get; set; }
    [Column(Order = 5)]
    public DateTime     Timestamp   { get; set; }
}

[Table("Telemetry")]
public class TickTelemetry : TelemetryBase { }

[Table("HourlyTelemetry")]
public class HourlyTelemetry : TelemetryBase { }

[Table("DailyTelemetry")]
public class DailyTelemetry : TelemetryBase { }

[Table("MonthlyTelemetry")]
public class MonthlyTelemetry : TelemetryBase { }