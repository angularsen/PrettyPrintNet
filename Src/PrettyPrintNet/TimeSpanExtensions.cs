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
        /// <summary>
        ///     All but the last unit should be rounded down. This way 3.5 hours becomes 3h 30m instead of 4h 30m. Same behavior as
        ///     the Hours, Minutes and Seconds properties of TimeSpan.
        /// </summary>
        private const IntegerRounding DefaultRounding = IntegerRounding.Down;

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
                {UnitStringRepresentation.Compact, new TimeFormat("", "", "")}
            };
        }

        public static string ToTimeRemainingString(this TimeSpan value,
            int maxUnitGroups = 1,
            UnitStringRepresentation rep = UnitStringRepresentation.Long,
            TimeSpanUnit highestUnit = TimeSpanUnit.Days,
            TimeSpanUnit lowestUnit = TimeSpanUnit.Seconds,
            IFormatProvider formatProvider = null)
        {
            return ToPrettyString(value, maxUnitGroups, rep, highestUnit, lowestUnit, IntegerRounding.Up, formatProvider);
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
        /// <param name="lowestUnitRounding">Rounding behavior of <paramref name="lowestUnit" />.</param>
        /// <param name="formatProvider">Specify the formatProvider used to .ToString() the numeric values.</param>
        /// <returns>Human readable string.</returns>
        public static string ToPrettyString(this TimeSpan value,
            int maxUnitGroups = 1,
            UnitStringRepresentation rep = UnitStringRepresentation.Long,
            TimeSpanUnit highestUnit = TimeSpanUnit.Days,
            TimeSpanUnit lowestUnit = TimeSpanUnit.Seconds,
            IntegerRounding lowestUnitRounding = IntegerRounding.ToNearestOrUp,
            IFormatProvider formatProvider = null)
        {
            if (maxUnitGroups <= 0)
                maxUnitGroups = 1;

            TimeFormat timeFormat;
            if (!Formats.TryGetValue(rep, out timeFormat))
                throw new NotImplementedException("UnitStringRepresentation: " + rep);

            if (value == TimeSpan.Zero)
            {
                return GetUnitValueString(0, lowestUnit, timeFormat, rep, formatProvider);
            }

            //int days = value.Days,
            //    hours = value.Hours,
            //    minutes = value.Minutes,
            //    seconds = value.Seconds,
            //    milliseconds = value.Milliseconds;
            //if (highestUnit < TimeSpanUnit.Days || lowestUnit > TimeSpanUnit.Days)
            //    days = 0;
            //if (highestUnit < TimeSpanUnit.Hours || lowestUnit > TimeSpanUnit.Hours)
            //    hours = 0;
            //if (highestUnit < TimeSpanUnit.Minutes || lowestUnit > TimeSpanUnit.Minutes)
            //    minutes = 0;
            //if (highestUnit < TimeSpanUnit.Seconds || lowestUnit > TimeSpanUnit.Seconds)
            //    seconds = 0;
            //if (highestUnit < TimeSpanUnit.Milliseconds || lowestUnit > TimeSpanUnit.Milliseconds)
            //    milliseconds = 0;

            // Trim off any values outside range
            TimeSpan trimmedValue = value; //new TimeSpan(days, hours, minutes, seconds, milliseconds);

            //var roundedValue = GetRoundedValue(trimmedValue, highestUnit, lowestUnit, lowestUnitRounding, maxUnitGroups);
            IList<UnitValue> nonZeroUnitValues =
                GetNonZeroUnitValues(trimmedValue, highestUnit, lowestUnit, lowestUnitRounding, maxUnitGroups)
                    .RoundUnitsUp();

            if (!nonZeroUnitValues.Any())
            {
                // Value is zero or near zero. Fall back to lowest unit.
                // Example: 0.1 seconds when lowest unit is seconds
                int nearZeroValueRounded = GetInteger(GetDouble(value, lowestUnit), lowestUnitRounding);
                return GetUnitValueString(nearZeroValueRounded, lowestUnit, timeFormat, rep, formatProvider);
            }

            List<string> unitStrings =
                nonZeroUnitValues
                    .Select((uv, i) => GetUnitValueString(uv.Value, uv.Unit, timeFormat, rep, formatProvider))
                    .ToList();

            // Example: "3 hours"
            if (unitStrings.Count == 1)
                return unitStrings.First();

            // Example: "1 weeks, 2 days"
            string firstParts = string.Join(timeFormat.GroupSeparator, unitStrings.Take(unitStrings.Count - 1).ToArray());

            // Example: "3 hours"
            string lastPart = unitStrings.Last();

            // Example: "1 weeks, 2 days and 3 hours"
            return firstParts + timeFormat.LastGroupSeparator + lastPart;
        }

        /// <summary>
        ///     Rounds 60 seconds to 1 minutes and similar.
        /// </summary>
        /// <param name="unitValues"></param>
        /// <returns></returns>
        private static IList<UnitValue> RoundUnitsUp(this IEnumerable<UnitValue> unitValues)
        {
            int days = 0, hours = 0, minutes = 0, seconds = 0, milliseconds = 0;
            foreach (UnitValue uv in unitValues)
            {
                if (uv.Unit == TimeSpanUnit.Days)
                    days = uv.Value;
                else if (uv.Unit == TimeSpanUnit.Hours)
                    hours = uv.Value;
                else if (uv.Unit == TimeSpanUnit.Minutes)
                    minutes = uv.Value;
                else if (uv.Unit == TimeSpanUnit.Seconds)
                    seconds = uv.Value;
                else if (uv.Unit == TimeSpanUnit.Milliseconds)
                    milliseconds = uv.Value;
                else
                    throw new NotImplementedException("TimeSpanUnit: " + uv.Unit);
            }

            var timeSpan = new TimeSpan(days, hours, minutes, seconds, milliseconds);

            List<UnitValue> result = new[]
            {
                GetUnitValue(timeSpan, DefaultRounding, TimeSpanUnit.Days),
                GetUnitValue(timeSpan, DefaultRounding, TimeSpanUnit.Hours),
                GetUnitValue(timeSpan, DefaultRounding, TimeSpanUnit.Minutes),
                GetUnitValue(timeSpan, DefaultRounding, TimeSpanUnit.Seconds),
                GetUnitValue(timeSpan, DefaultRounding, TimeSpanUnit.Milliseconds)
            }.Where(uv => uv.Value >= 1)
                .ToList();

            return result;
        }

        private static IList<UnitValue> GetNonZeroUnitValues(TimeSpan value, TimeSpanUnit highestUnit,
            TimeSpanUnit lowestUnit, IntegerRounding lowestUnitRounding, int maxUnitGroups)
        {
            value = GetTimeSpanWithLowestUnitRounded(value, lowestUnit, lowestUnitRounding);

            IList<UnitValue> unitValues = UnitsLargeToSmall
                .Where(unit => unit <= highestUnit && unit >= lowestUnit)
                .Select(unit => GetUnitValue(value, DefaultRounding, unit))
                .Where(unitValue => unitValue.Value >= 1)
                .Take(maxUnitGroups)
                .ToList();

            if (!unitValues.Any())
            {
                return unitValues;
            }

            UnitValue last = unitValues.Last();

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (last.DoubleValue == 0) return unitValues;

            List<UnitValue> unitValuesWithLastUnitRoundedUp = unitValues
                .Take(unitValues.Count - 1)
                .Concat(new[] {GetUnitValue(value, lowestUnitRounding, last.Unit)})
                .ToList();

            return unitValuesWithLastUnitRoundedUp;
        }

        private static TimeSpan GetTimeSpanWithLowestUnitRounded(TimeSpan value, TimeSpanUnit lowestUnit,
            IntegerRounding lowestUnitRounding)
        {
            int lowestUnitValueRounded = GetInteger(GetDouble(value, lowestUnit), lowestUnitRounding);

            // Round the lowest unit, then reconstruct TimeSpan to round 60 seconds to 1 minute etc.
            int days = value.Days,
                hours = value.Hours,
                minutes = value.Minutes,
                seconds = value.Seconds,
                milliseconds = value.Milliseconds;

            switch (lowestUnit)
            {
                case TimeSpanUnit.Days:
                    days = lowestUnitValueRounded;
                    hours = minutes = seconds = milliseconds = 0;
                    break;
                case TimeSpanUnit.Hours:
                    hours = lowestUnitValueRounded;
                    minutes = seconds = milliseconds = 0;
                    break;
                case TimeSpanUnit.Minutes:
                    minutes = lowestUnitValueRounded;
                    seconds = milliseconds = 0;
                    break;
                case TimeSpanUnit.Seconds:
                    seconds = lowestUnitValueRounded;
                    milliseconds = 0;
                    break;
                case TimeSpanUnit.Milliseconds:
                    milliseconds = lowestUnitValueRounded;
                    break;
            }

            value = new TimeSpan(days, hours, minutes, seconds, milliseconds);
            return value;
        }

        private static UnitValue GetUnitValue(TimeSpan value, IntegerRounding rounding, TimeSpanUnit unit)
        {
            double doubleValue = GetDouble(value, unit);
            //int valueRoundedDown = GetInteger(doubleValue, DefaultRounding);
            //int valueWithLowestUnitRounding = GetInteger(doubleValue, rounding);
            int valueRounded = GetInteger(doubleValue, rounding);
            return new UnitValue(doubleValue, unit, valueRounded); //Down, valueWithLowestUnitRounding);
        }

        private static int GetInteger(double doubleValue, IntegerRounding rounding)
        {
            switch (rounding)
            {
                case IntegerRounding.Down:
                    return (int) doubleValue;
                case IntegerRounding.ToNearestOrUp:
                    return Convert.ToInt32(doubleValue);
                case IntegerRounding.Up:
                    return (int) Math.Ceiling(doubleValue);
                default:
                    throw new NotImplementedException("IntegerRounding: " + rounding);
            }
        }

        private static double GetDouble(TimeSpan value, TimeSpanUnit unit)
        {
            switch (unit)
            {
                case TimeSpanUnit.Days:
                    return (double) value.Ticks/TimeSpan.TicksPerDay;

                case TimeSpanUnit.Hours:
                    return ((double) value.Ticks/TimeSpan.TicksPerHour)%24;

                case TimeSpanUnit.Minutes:
                    return ((double) value.Ticks/TimeSpan.TicksPerMinute)%60;

                case TimeSpanUnit.Seconds:
                    return ((double) value.Ticks/TimeSpan.TicksPerSecond)%60;

                case TimeSpanUnit.Milliseconds:
                    return ((double) value.Ticks/TimeSpan.TicksPerMillisecond)%1000;

                //case TimeSpanUnit.Microseconds:
                //return ((double)value.Ticks/10)%1000;

                //case TimeSpanUnit.Nanoseconds:
                //return ((double)value.Ticks*100)%1000;

                default:
                    throw new ArgumentException(
                        string.Format(
                            "Flag not supported [{0}]. Note that flag must not be a combination of multiple flags.",
                            unit));
            }
        }

        private static string GetUnitValueString(int value, TimeSpanUnit unit, TimeFormat timeFormat,
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
            public readonly double DoubleValue;
            public readonly TimeSpanUnit Unit;
            //public readonly int ValueRoundedDown;
            //public readonly int ValueWithLowestUnitRounding;
            public readonly int Value;

            public UnitValue(double doubleValue, TimeSpanUnit unit, int value) //, int valueWithLowestUnitRounding)
            {
                DoubleValue = doubleValue;
                Unit = unit;
                Value = value;
                //ValueRoundedDown = value;
                //ValueWithLowestUnitRounding = valueWithLowestUnitRounding;
            }
        }
    }
}