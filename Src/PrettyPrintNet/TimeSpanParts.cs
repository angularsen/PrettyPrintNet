using System;

namespace PrettyPrintNet
{
    [Flags]
    public enum TimeSpanParts
    {
        None = 0,
        Days = 1,
        Hours = 2,
        Minutes = 4,
        Seconds = 8,
        Milliseconds = 16,
        Microseconds = 32,
        Nanoseconds = 64,
        All = Days | Hours | Minutes | Seconds | Milliseconds | Microseconds | Nanoseconds,
        HoursAndUp = Days | Hours,
        MinutesAndUp = HoursAndUp | Minutes,
        SecondsAndUp = MinutesAndUp | Seconds,
        MillisecondsAndUp = SecondsAndUp | Milliseconds,
        
    }
}