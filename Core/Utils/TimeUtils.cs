namespace Firewatch.Core.Utils;

public static class TimeUtils {
    public static long FiletimeNow  => DateTime.UtcNow.ToFileTime();
    public static DateTime UtcNow   => DateTime.UtcNow;

    public static class EVETime {
        public static long
        mSecond     = 1000L,        //1000
        Second      = 10000000L,    //10000000
        Minute      = (Second * 60L),
        Hour        = (Minute * 60L),
        Day         = (Hour * 24L),
        Week        = (Day * 7L),
        Month       = (Day * 30L),
        Year        = (Day * 365L);
    }
}