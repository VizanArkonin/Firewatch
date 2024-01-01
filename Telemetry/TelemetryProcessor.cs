namespace Firewatch.Telemetry;

public abstract class TelemetryProcessor
{
    /// <summary>
    /// Callback timer - waits for next tick timestamp then shoots the callback function - Process.
    /// NOTE: Timer is not self-reset. It must be done in InitializeTimer method. Since timespans differ
    /// from processor to processor - we implement them in target type.
    /// </summary>
    /// <value></value>
    protected Timer? ProcessTimer {get; set;} = null;

    public abstract void InitializeTimer();
    public abstract void Process();
}