using Firewatch.Core.Utils;
using Firewatch.Core.Logging;
using Firewatch.Telemetry.Monitors;

namespace Firewatch.Telemetry;

/// <summary>
/// Tick processor is the main workhorse for retrieving the telemetry - it operates on sub-minute timer and 
/// is the only processor used to gather the data from sources.
/// </summary>
public class TickProcessor : TelemetryProcessor
{
    private LoggingChannel  Log         { get; init; }
    private CpuMonitor      Monitor     { get; init; }

    public TickProcessor(LoggingChannel loggingChannel)
    {
        this.Log = loggingChannel;
        this.Log.Debug("Initializing Tick processor");
        if (Firewatch.Config.EnableCPUMonitoring)
            Monitor = new CpuMonitor(Firewatch.Logger.GetLogger("CpuMonitor"));
    }


    /// <summary>
    /// Tick processor has no need for fixed time value, so we simply set the timer once with cyclic Process invocation
    /// </summary>
    public override void InitializeTimer()
    {
        Log.Debug($"Initializing timer with {Firewatch.Config.PollingRateInSeconds} seconds cycle time");
        ProcessTimer ??= new Timer(x => Process(), null, 0, Firewatch.Config.PollingRateInSeconds * 1000);
    }

    public override void Process()
    {
        Log.Debug("Process tick called");
        DateTime timestamp = TimeUtils.UtcNow;
        if (Firewatch.Config.EnableCPUMonitoring)
        {
            Monitor.LogCpuUsage(timestamp);
        }
    }
}