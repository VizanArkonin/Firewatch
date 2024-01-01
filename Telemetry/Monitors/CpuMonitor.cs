using Firewatch;
using Firewatch.Core.Logging;
using Firewatch.Database.Models;

namespace Firewatch.Telemetry.Monitors;

/// <summary>
/// A service, used to monitor CPU usage and report it to database.
/// </summary>
public class CpuMonitor : Monitor
{
    private LoggingChannel Log                  { get; init; }

    /// Sadly, there's no good alternative for PerformanceCounter on linux. Luckily, we can still extract most of what we need
    /// from files in /proc folder.
    private const string CPUSTAT_FILEPATH                       = "/proc/stat";

    private long? _prevIdleTime                 { get; set; }   = null;
    private long? _prevTotalTime                { get; set; }   = null;
    private decimal? CpuUsage                   { get; set; }

    public CpuMonitor(LoggingChannel loggingChannel)
    {
        this.Log = loggingChannel;
    }

    /// <summary>
    /// Main workhorse - pulls the values from /proc/stat file, processes them and inserts the data into Telemetry table.
    /// NOTE: First tick is always empty - we need 2 sets of values to calculate the CPU usage, so the first iteration gathers the first
    /// set. Second iteration is when the data flow begins
    /// </summary>
    /// <param name="timestamp">A timestamp for database. It is passed from above to make sure all telemetry entries have exactly the same entry time</param>
    public void LogCpuUsage(DateTime timestamp)
    {
        Log.Debug("Logging CPU usage");
        string cpuUsageLine = ReadLineStartingWith(CPUSTAT_FILEPATH, "cpu  ");

        if (string.IsNullOrWhiteSpace(cpuUsageLine))
        {
            Log.Warning($"Couldn't read line from '{CPUSTAT_FILEPATH}'");
            return;
        }

        // Format: "cpu  20546715 4367 11631326 215282964 96602 0 584080 0 0 0"
        var cpuNumberStrings = cpuUsageLine.Split(' ').Skip(2);

        if (cpuNumberStrings.Any(n => !long.TryParse(n, out _)))
        {
            Log.Warning($"Failed to parse '{CPUSTAT_FILEPATH}' output correctly. Line: {cpuUsageLine}");
            return;
        }

        var cpuNumbers = cpuNumberStrings.Select(long.Parse).ToArray();
        var idleTime = cpuNumbers[3];
        var iowait = cpuNumbers[4]; // Iowait is not real cpu time
        var totalTime = cpuNumbers.Sum() - iowait;

        if (_prevIdleTime is not null && _prevTotalTime is not null)
        {
            var deltaIdleTime = idleTime - _prevIdleTime;
            var deltaTotalTime = totalTime - _prevTotalTime;

            if (deltaTotalTime == 0f)
            {
                return;
            }

            var currentCpuUsage = (1.0f - deltaIdleTime / ((float)deltaTotalTime)) * 100f;

            var previousCpuUsage = CpuUsage is null ? 0.0m : CpuUsage;
            CpuUsage = Math.Round(((decimal)previousCpuUsage + 2 * (decimal)currentCpuUsage) / 3, 2);

            Log.Trace($"Current CPU usage is {CpuUsage}. Previous value was {previousCpuUsage}");

            Firewatch.DB.Add(new TickTelemetry{
                ResourceId = 1,
                MetricId = 1,
                Value = (float)CpuUsage,
                Timestamp = timestamp
            });
            Firewatch.DB.SaveChanges();
        } else {
            Log.Debug("_prevIdleTime and _prevTotalTime were null. Skipped until next cycle");
        }

        _prevIdleTime = idleTime;
        _prevTotalTime = totalTime;
    }
}