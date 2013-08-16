using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PrettyPrintNet
{
    /// <summary>
    /// Utilities and helper code for working with TimeSpan.
    /// </summary>
    public static class PrettyPrintTimeSpan
    {
        /// <summary>
        /// Returns a human readable string from TimeSpan, with optional parts to include.
        /// </summary>
        /// <param name="value">TimeSpan value.</param>
        /// <param name="partsToInclude">Which parts of the time span to include, such as minutes and hours. See <see cref="TimeSpanParts"/> for flag values and combinations.</param>
        /// <param name="includePartIfZero">Set to true to always include a part even if its value is zero.</param>
        /// <param name="culture">Specify the culture used to .ToString() the numeric values.</param>
        /// <returns>Human readable string.</returns>
        public static string ToFriendlyString(this TimeSpan value,
                                                   TimeSpanParts partsToInclude,
                                                   bool includePartIfZero = false,
                                                   CultureInfo culture = null)
        {
            return ToFriendlyString(value, partsToInclude,
                                  includePartIfZero,
                                  seconds => seconds == 1 ? "second" : "seconds",
                                  minutes => minutes == 1 ? "minute" : "minutes",
                                  hours => hours == 1 ? "hour" : "hours",
                                  days => days == 1 ? "day" : "days",
                                  milliseconds => milliseconds == 1 ? "millisecond" : "milliseconds",
                                  microseconds => microseconds == 1 ? "microsecond" : "microseconds",
                                  nanoseconds => nanoseconds == 1 ? "nanosecond" : "nanoseconds",
                                  culture: culture);
        }

        /// <summary>
        /// Returns a human readable string from TimeSpan, with a max number of parts to include. Units are included from largest to smallest.
        /// </summary>
        /// <param name="value">TimeSpan value.</param>
        /// <param name="maxPartsToInclude">The max number of time span parts to include. Units are included from largest to smallest.</param>
        /// <param name="culture">Specify the culture used to .ToString() the numeric values.</param>
        /// <returns>Human readable string.</returns>
        public static string ToFriendlyString(this TimeSpan value,
                                                   int maxPartsToInclude,
                                                   CultureInfo culture = null)
        {
            var partsLargestToSmallest = new List<TimeSpanParts>
            {
                TimeSpanParts.Days,
                TimeSpanParts.Hours,
                TimeSpanParts.Minutes,
                TimeSpanParts.Seconds,
                TimeSpanParts.Milliseconds,
                TimeSpanParts.Microseconds,
                TimeSpanParts.Nanoseconds
            };

            var partsToInclude = TimeSpanParts.None;
            int partsIncluded = 0;
            if (maxPartsToInclude > 0)
            {
                foreach (TimeSpanParts part in partsLargestToSmallest)
                {
                    long partValue = GetPartValue(value, part);
                    if (partValue >= 1)
                    {
                        partsToInclude |= part;
                        partsIncluded++;
                        if (partsIncluded >= maxPartsToInclude)
                            break;
                    }
                }
            }
            return ToFriendlyString(value, partsToInclude,
                                  false,
                                  seconds => seconds == 1 ? "second" : "seconds",
                                  minutes => minutes == 1 ? "minute" : "minutes",
                                  hours => hours == 1 ? "hour" : "hours",
                                  days => days == 1 ? "day" : "days",
                                  milliseconds => milliseconds == 1 ? "millisecond" : "milliseconds",
                                  microseconds => microseconds == 1 ? "microsecond" : "microseconds",
                                  nanoseconds => nanoseconds == 1 ? "nanosecond" : "nanoseconds",
                                  culture: culture);
        }

        private static long GetPartValue(TimeSpan value, TimeSpanParts part)
        {
            switch (part)
            {
                case TimeSpanParts.Days:
                    return value.Days;

                case TimeSpanParts.Hours:
                    return value.Hours;

                case TimeSpanParts.Minutes:
                    return value.Minutes;

                case TimeSpanParts.Seconds:
                    return value.Seconds;

                case TimeSpanParts.Milliseconds:
                    return value.Milliseconds;

                case TimeSpanParts.Microseconds:
                    return Convert.ToInt64(value.Ticks/10.0);

                case TimeSpanParts.Nanoseconds:
                    return Convert.ToInt64(value.Ticks*100.0);

                default:
                    throw new ArgumentException(string.Format("Flag not supported [{0}]. Note that flag must not be a combination of multiple flags.", part));
            }
        }

        /// <summary>
        /// Returns a human readable string from TimeSpan, with optional parts to include.
        /// </summary>
        /// <param name="value">TimeSpan value.</param>
        /// <param name="partsToInclude">Which parts of the time span to include, such as minutes and hours. See <see cref="TimeSpanParts"/> for flag values and combinations.</param>
        /// <param name="includePartIfZero">Set to true to always include a part even if its value is zero.</param>
        /// <param name="secondStringFunc">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="minuteStringFunc">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="hourStringFunc">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="dayStringFunc">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="millisecondStringFunc">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="microsecondStringFunc">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="nanosecondStringFunc">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="partSeparator">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="lastPartSeparator">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="valueAndUnitSeparator">Function that takes numeric value and returns its unit string representation. This allows the client code to take plurality of different cultures into account, such as "1 minute" and "2 minutes".</param>
        /// <param name="culture">Specify the culture used to .ToString() the numeric values.</param>
        /// <returns></returns>
        public static string ToFriendlyString(this TimeSpan value, TimeSpanParts partsToInclude, bool includePartIfZero, Func<long, string> secondStringFunc, Func<long, string> minuteStringFunc , Func<long, string> hourStringFunc , Func<long, string> dayStringFunc , Func<long, string> millisecondStringFunc , Func<long, string> microsecondStringFunc , Func<long, string> nanosecondStringFunc , string partSeparator = ", ", string lastPartSeparator = " and ", string valueAndUnitSeparator = " ", CultureInfo culture = null)
        {
            var parts = new List<string>();

            if (partsToInclude.HasFlag(TimeSpanParts.Days))
                AddPartString(parts, value.Days, dayStringFunc, includePartIfZero, valueAndUnitSeparator, culture);

            if (partsToInclude.HasFlag(TimeSpanParts.Hours))
                AddPartString(parts, value.Hours, hourStringFunc, includePartIfZero, valueAndUnitSeparator, culture);

            if (partsToInclude.HasFlag(TimeSpanParts.Minutes))
                AddPartString(parts, value.Minutes, minuteStringFunc, includePartIfZero, valueAndUnitSeparator, culture);

            if (partsToInclude.HasFlag(TimeSpanParts.Seconds))
                AddPartString(parts, value.Seconds, secondStringFunc, includePartIfZero, valueAndUnitSeparator, culture);

            if (partsToInclude.HasFlag(TimeSpanParts.Milliseconds))
                AddPartString(parts, value.Milliseconds, millisecondStringFunc, includePartIfZero, valueAndUnitSeparator, culture);

            var timeSpanRoundedToMilliseconds = new TimeSpan(value.Days, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);
            long remainderTicks = (value - timeSpanRoundedToMilliseconds).Ticks;

            if (partsToInclude.HasFlag(TimeSpanParts.Microseconds))
            {
                long microseconds = Convert.ToInt64(remainderTicks / 10.0);
                remainderTicks -= microseconds * 10;
                AddPartString(parts, microseconds, microsecondStringFunc, includePartIfZero, valueAndUnitSeparator, culture);
            }

            if (partsToInclude.HasFlag(TimeSpanParts.Nanoseconds))
            {
                long nanoseconds = Convert.ToInt64(remainderTicks * 100.0);
                AddPartString(parts, nanoseconds, nanosecondStringFunc, includePartIfZero, valueAndUnitSeparator, culture);
            }

            if (parts.Count == 0)
                return "0 seconds";

            if (parts.Count == 1)
                return parts.First();

            string firstPartsText = string.Join(partSeparator, parts.Take(parts.Count - 1).ToArray());
            string lastPart = parts.Last();
            string result = firstPartsText + lastPartSeparator + lastPart;
            return result;
        }


        private static void AddPartString(List<string> parts, long partValue, Func<long, string> partStringFunc, bool includePartIfZero, string valueAndUnitSeparator, CultureInfo culture)
        {
            if (partStringFunc == null)
                throw new ArgumentNullException("partStringFunc", "Func cannot be null if the corresponding TimeSpanParts flag is set.");

            if (partValue == 0 && !includePartIfZero)
                return;

            parts.Add(partValue.ToString(culture) + valueAndUnitSeparator + partStringFunc(partValue));
        }

    }
}