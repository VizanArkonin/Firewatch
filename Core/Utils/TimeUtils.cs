namespace Firewatch.Core.Utils;

public static class TimeUtils {
    public static long FiletimeNow  => DateTime.UtcNow.ToFileTime();
    public static DateTime UtcNow   => DateTime.UtcNow;


    /// <summary>
    /// Gets the next month timestamp.
    /// Note that it disregards hours, minutes and seconds - we need a clean timestamp.
    /// </summary>
    /// <returns>DateTime stamp for next month's 0 hours, 0 minutes and 0 seconds</returns>
    public static DateTime NextMonth()
    {
        DateTime next = UtcNow.AddMonths(1);
        next = next.Date + new TimeSpan(0, 0, 0);

        return next;
    }

    /// <summary>
    /// Gets the next week timestamp.
    /// Note that it disregards hours, minutes and seconds - we need a clean timestamp.
    /// </summary>
    /// <returns>DateTime stamp for next week's 0 hours, 0 minutes and 0 seconds</returns>
    public static DateTime NextWeek()
    {
        DateTime next = UtcNow.AddDays(7);
        next = next.Date + new TimeSpan(0, 0, 0);

        return next;
    }

    /// <summary>
    /// Gets the next hour timestamp.
    /// Note that it disregards minutes and seconds - we need a clean timestamp.
    /// </summary>
    /// <returns>DateTime stamp for next hour's 0 minutes and 0 seconds</returns>
    public static DateTime NextHour()
    {
        DateTime next = UtcNow.AddHours(1);
        next = next.Date + new TimeSpan(next.Hour, 0, 0);

        return next;
    }

    /// <summary>
    /// Gets the next minute timestamp.
    /// Note that it disregards seconds - we need a clean timestamp
    /// </summary>
    /// <returns>DateTime stamp for next minute's 0 seconds</returns>
    public static DateTime NextMinute()
    {
        DateTime next = UtcNow.AddMinutes(1);
        next = next.Date + new TimeSpan(next.Hour, next.Minute, 0);

        return next;
    }
}