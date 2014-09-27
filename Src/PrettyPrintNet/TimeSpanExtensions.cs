using System;
using System.Collections.Generic;
using System.Linq;

namespace PrettyPrintNet
{
    /// <summary>
    ///     Utilities and helper code for working with TimeSpan.
    /// </summary>
    public static class TimeSpanExtensions
    {
        private static readonly List<TimeSpanUnit> UnitsLargeToSmall =
            Enum.GetValues(typeof (TimeSpanUnit)).Cast<TimeSpanUnit>().OrderByDescending(v => v).ToList();

        /// <summary>
        ///     Returns a human readable string from TimeSpan, with a max number of parts to include. Units are included from
        ///     largest to smallest.
        /// </summary>
        /// <param name="value">Time span value.</param>
        /// <param name="maxUnitGroups">
        ///     The max number of timespan units to use.
        /// </param>
        /// <param name="lowestUnit">Lowest unit to include in string.</param>
        /// <param name="rep"></param>
        /// <param name="higestUnit">Highest unit to include in string.</param>
        /// <param name="formatProvider">Specify the formatProvider used to .ToString() the numeric values.</param>
        /// <returns>Human readable string.</returns>
        public static string ToPrettyString(this TimeSpan value,
            int maxUnitGroups,
            UnitStringRepresentation rep = UnitStringRepresentation.Long,
            TimeSpanUnit higestUnit = TimeSpanUnit.Days,
            TimeSpanUnit lowestUnit = TimeSpanUnit.Seconds,
            IFormatProvider formatProvider = null)
        {
            if (maxUnitGroups <= 0)
                throw new ArgumentException("Must be greater than zero.", "maxUnitGroups");

            string unitValueSeparator = GetUnitValueSeparator(rep);

            List<string> unitStrings =
                UnitsLargeToSmall.Where(t => t <= higestUnit && t >= lowestUnit)
                    .Select(u => new UnitValue(u, GetTimeSpanUnitValue(value, u)))
                    .Where(uv => uv.Value > 0)
                    .Take(maxUnitGroups)
                    .Select(uv => GetTimeSpanUnitString(uv.Value, uv.Unit, unitValueSeparator, rep, formatProvider))
                    .ToList();

            if (!unitStrings.Any())
                return GetTimeSpanUnitString(0, lowestUnit, unitValueSeparator, rep, formatProvider);

            if (unitStrings.Count == 1)
                return unitStrings.First();


            string groupSeparator = GetGroupSeparator(rep);
            string lastPartSeparator = GetLastGroupSeparator(rep);

            // 3 weeks, 4 days
            string firstParts = string.Join(groupSeparator, unitStrings.Take(unitStrings.Count - 1).ToArray());

            // 2 hours
            string lastPart = unitStrings.Last();

            // 3 weeks, 4 days and 2 hours
            return firstParts + lastPartSeparator + lastPart;
        }

        private static string GetGroupSeparator(UnitStringRepresentation rep)
        {
            switch (rep)
            {
                case UnitStringRepresentation.Compact:
                    return "";
                case UnitStringRepresentation.Long:
                    return ", ";
                case UnitStringRepresentation.Short:
                    return " ";
                default:
                    throw new NotImplementedException("UnitStringRepresentation: " + rep);
            }
        }

        private static string GetLastGroupSeparator(UnitStringRepresentation rep)
        {
            switch (rep)
            {
                case UnitStringRepresentation.Compact:
                    return "";
                case UnitStringRepresentation.Long:
                    return " and ";
                case UnitStringRepresentation.Short:
                    return " ";
                default:
                    throw new NotImplementedException("UnitStringRepresentation: " + rep);
            }
        }

        private static int GetTimeSpanUnitValue(TimeSpan value, TimeSpanUnit unit)
        {
            switch (unit)
            {
                case TimeSpanUnit.Days:
                    return value.Days;

                case TimeSpanUnit.Hours:
                    return value.Hours;

                case TimeSpanUnit.Minutes:
                    return value.Minutes;

                case TimeSpanUnit.Seconds:
                    return value.Seconds;

                case TimeSpanUnit.Milliseconds:
                    return value.Milliseconds;

                    //case TimeSpanUnit.Microseconds:
                    //    return Convert.ToInt32(value.Ticks / 10.0);

                    //case TimeSpanUnit.Nanoseconds:
                    //    return Convert.ToInt32(value.Ticks * 100.0);

                default:
                    throw new ArgumentException(
                        string.Format(
                            "Flag not supported [{0}]. Note that flag must not be a combination of multiple flags.",
                            unit));
            }
        }

        private static string GetTimeSpanUnitString(int value, TimeSpanUnit unit, string unitValueSeparator,
            UnitStringRepresentation rep, IFormatProvider formatProvider)
        {
            string unitString = GetUnitString(value, unit, rep);
            return value.ToString(formatProvider) + unitValueSeparator + unitString;
        }

        private static string GetUnitString(int value, TimeSpanUnit unit, UnitStringRepresentation rep)
        {
            switch (rep)
            {
                case UnitStringRepresentation.Compact:
                    return GetCompactUnitString(value, unit);
                case UnitStringRepresentation.Long:
                    return GetLongUnitString(value, unit);
                case UnitStringRepresentation.Short:
                    return GetShortUnitString(value, unit);
                default:
                    throw new NotImplementedException("TimeSpanUnit: " + unit);
            }
        }

        private static string GetLongUnitString(int value, TimeSpanUnit unit)
        {
            switch (unit)
            {
                case TimeSpanUnit.Days:
                    return value == 1 ? "day" : "days";

                case TimeSpanUnit.Hours:
                    return value == 1 ? "hour" : "hours";

                case TimeSpanUnit.Minutes:
                    return value == 1 ? "minute" : "minutes";

                case TimeSpanUnit.Seconds:
                    return value == 1 ? "second" : "seconds";

                case TimeSpanUnit.Milliseconds:
                    return value == 1 ? "millisecond" : "milliseconds";

                    //case TimeSpanUnit.Microseconds:
                    //    return value == 1 ? "microsecond" : "microseconds";

                    //case TimeSpanUnit.Nanoseconds:
                    //    return value == 1 ? "nanosecond" : "nanoseconds";

                default:
                    throw new NotImplementedException("TimeSpanUnit: " + unit);
            }
        }

        private static string GetShortUnitString(int value, TimeSpanUnit unit)
        {
            switch (unit)
            {
                case TimeSpanUnit.Days:
                    return value == 1 ? "day" : "days";

                case TimeSpanUnit.Hours:
                    return value == 1 ? "hr" : "hrs";

                case TimeSpanUnit.Minutes:
                    return value == 1 ? "min" : "mins";

                case TimeSpanUnit.Seconds:
                    return value == 1 ? "sec" : "secs";

                case TimeSpanUnit.Milliseconds:
                    return value == 1 ? "msec" : "msecs";

                    //case TimeSpanUnit.Microseconds:
                    //    return value == 1 ? "µsec" : "µsecs";

                    //case TimeSpanUnit.Nanoseconds:
                    //    return value == 1 ? "nsec" : "nsecs";

                default:
                    throw new NotImplementedException("TimeSpanUnit: " + unit);
            }
        }

        private static string GetCompactUnitString(int value, TimeSpanUnit unit)
        {
            switch (unit)
            {
                case TimeSpanUnit.Days:
                    return "d";

                case TimeSpanUnit.Hours:
                    return "h";

                case TimeSpanUnit.Minutes:
                    return "m";

                case TimeSpanUnit.Seconds:
                    return "s";

                case TimeSpanUnit.Milliseconds:
                    return "ms";

                    //case TimeSpanUnit.Microseconds:
                    //    return value == 1 ? "microsecond" : "microseconds";

                    //case TimeSpanUnit.Nanoseconds:
                    //    return value == 1 ? "nanosecond" : "nanoseconds";

                default:
                    throw new NotImplementedException("TimeSpanUnit: " + unit);
            }
        }

        private static string GetUnitValueSeparator(UnitStringRepresentation rep)
        {
            switch (rep)
            {
                case UnitStringRepresentation.Long:
                    return " ";
                case UnitStringRepresentation.Short:
                    return " ";
                case UnitStringRepresentation.Compact:
                    return "";
                default:
                    throw new NotImplementedException("UnitStringRepresentation: " + rep);
            }
        }

        private struct UnitValue
        {
            public readonly TimeSpanUnit Unit;
            public readonly int Value;

            public UnitValue(TimeSpanUnit unit, int value)
            {
                Unit = unit;
                Value = value;
            }
        }
    }
}