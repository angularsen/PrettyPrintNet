using System;
using System.Collections.Generic;
using System.Linq;

namespace PrettyPrintNet
{
    internal class TimeFormat
    {
        public readonly string GroupSeparator;
        public readonly string LastGroupSeparator;
        public readonly string UnitValueSeparator;

        public TimeFormat(string groupSeparator, string lastGroupSeparator, string unitValueSeparator)
        {
            GroupSeparator = groupSeparator;
            LastGroupSeparator = lastGroupSeparator;
            UnitValueSeparator = unitValueSeparator;
        }
    }

    /// <summary>
    ///     Utilities and helper code for working with TimeSpan.
    /// </summary>
    public static class TimeSpanExtensions
    {
        private static readonly List<TimeSpanUnit> UnitsLargeToSmall =
            Enum.GetValues(typeof (TimeSpanUnit)).Cast<TimeSpanUnit>().OrderByDescending(v => v).ToList();

        private static readonly IDictionary<UnitStringRepresentation, TimeFormat> Formats;

        static TimeSpanExtensions()
        {
            Formats = new Dictionary<UnitStringRepresentation, TimeFormat>
            {
                {UnitStringRepresentation.Long, new TimeFormat(", ", " and ", " ")},
                {UnitStringRepresentation.Short, new TimeFormat(" ", " ", " ")},
                {UnitStringRepresentation.CompactWithSpace, new TimeFormat(" ", " ", "")},
                {UnitStringRepresentation.Compact, new TimeFormat("", "", "")},
            };
        }

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
        /// <param name="highestUnit">Highest unit to include in string.</param>
        /// <param name="formatProvider">Specify the formatProvider used to .ToString() the numeric values.</param>
        /// <returns>Human readable string.</returns>
        public static string ToPrettyString(this TimeSpan value,
            int maxUnitGroups,
            UnitStringRepresentation rep = UnitStringRepresentation.Long,
            TimeSpanUnit highestUnit = TimeSpanUnit.Days,
            TimeSpanUnit lowestUnit = TimeSpanUnit.Seconds,
            IFormatProvider formatProvider = null)
        {
            if (maxUnitGroups <= 0)
                throw new ArgumentException("Must be greater than zero.", "maxUnitGroups");

            TimeFormat format;
            if (!Formats.TryGetValue(rep, out format))
                throw new NotImplementedException("UnitStringRepresentation: " + rep);

            string unitValueSeparator = format.UnitValueSeparator;

            List<string> unitStrings =
                UnitsLargeToSmall.Where(t => t <= highestUnit && t >= lowestUnit)
                    .Select(u => new UnitValue(u, GetTimeSpanUnitValue(value, u)))
                    .Where(uv => uv.Value > 0)
                    .Take(maxUnitGroups)
                    .Select(uv => GetTimeSpanUnitString(uv.Value, uv.Unit, format, rep, formatProvider))
                    .ToList();

            if (!unitStrings.Any())
                return GetTimeSpanUnitString(0, lowestUnit, format, rep, formatProvider);

            if (unitStrings.Count == 1)
                return unitStrings.First();


            // 3 weeks, 4 days
            string firstParts = string.Join(format.GroupSeparator, unitStrings.Take(unitStrings.Count - 1).ToArray());

            // 2 hours
            string lastPart = unitStrings.Last();

            // 3 weeks, 4 days and 2 hours
            return firstParts + format.LastGroupSeparator + lastPart;
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

        private static string GetTimeSpanUnitString(int value, TimeSpanUnit unit, TimeFormat timeFormat,
            UnitStringRepresentation rep, IFormatProvider formatProvider)
        {
            string unitString = GetUnitString(value, unit, rep);
            return value.ToString(formatProvider) + timeFormat.UnitValueSeparator + unitString;
        }

        private static string GetUnitString(int value, TimeSpanUnit unit, UnitStringRepresentation rep)
        {
            switch (rep)
            {
                case UnitStringRepresentation.Compact:
                case UnitStringRepresentation.CompactWithSpace:
                    return GetCompactUnitString(unit);
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

        private static string GetCompactUnitString(TimeSpanUnit unit)
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