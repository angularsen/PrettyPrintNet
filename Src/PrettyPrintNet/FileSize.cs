#region Copyright

// 
// Copyright © 2013-2013 by Initial Force AS.  All rights reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;

namespace PrettyPrintNet
{
    public delegate string GetSuffixFunc(double value);

    public static class FileSize
    {
        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");
        private static readonly Dictionary<string, GetSuffixFunc[]> CultureToLongSuffixFuncs;
        private static readonly Dictionary<string, GetSuffixFunc[]> CultureToShortSuffixFuncs;

        static FileSize()
        {
            GetSuffixFunc[] longEnglishSuffixFuncs =
            {
// ReSharper disable CompareOfFloatsByEqualityOperator
                value => value == 1 ? "byte" : "bytes",
                value => value == 1 ? "kilobyte" : "kilobytes",
                value => value == 1 ? "megabyte" : "megabytes",
                value => value == 1 ? "gigabyte" : "gigabytes",
                value => value == 1 ? "terabyte" : "terabytes",
                value => value == 1 ? "petabyte" : "petabytes",
                value => value == 1 ? "exabyte" : "exabytes"
// ReSharper restore CompareOfFloatsByEqualityOperator
            };

            GetSuffixFunc[] shortEnglishSuffixFuncs =
            {
                value => "B",
                value => "KB",
                value => "MB",
                value => "GB",
                value => "TB",
                value => "PB",
                value => "EB"
            };

            CultureToLongSuffixFuncs = new Dictionary<string, GetSuffixFunc[]>
            {
                {"en-US", longEnglishSuffixFuncs},
                {"nb-NO", longEnglishSuffixFuncs}
            };

            CultureToShortSuffixFuncs = new Dictionary<string, GetSuffixFunc[]>
            {
                {"en-US", shortEnglishSuffixFuncs},
                {"nb-NO", shortEnglishSuffixFuncs}
            };
        }

        public static string ToLongString(ulong bytes, CultureInfo culture = null, string stringFormat = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            GetSuffixFunc[] suffixFuncs = GetSuffixesForCulture(culture, CultureToLongSuffixFuncs);
            return GetFileSize(bytes, suffixFuncs, culture, stringFormat);
        }

        public static string ToShortString(ulong bytes, CultureInfo culture = null, string stringFormat = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            GetSuffixFunc[] suffixFuncs = GetSuffixesForCulture(culture, CultureToShortSuffixFuncs);
            return GetFileSize(bytes, suffixFuncs, culture, stringFormat);
        }

        #region Private

        private static GetSuffixFunc[] GetSuffixesForCulture(CultureInfo culture,
            Dictionary<string, GetSuffixFunc[]> cultureToSuffixFuncs)
        {
            GetSuffixFunc[] suffixFuncs;
            if (!cultureToSuffixFuncs.TryGetValue(culture.Name, out suffixFuncs))
                suffixFuncs = cultureToSuffixFuncs[DefaultCulture.Name];

            return suffixFuncs;
        }

        /// <summary>
        ///     Get human readable text for file size, rounding to the largest unit where its value is >= 1.
        /// </summary>
        /// <param name="bytes">File size in bytes.</param>
        /// <param name="suffixes">Array of suffix functions, returning a suffix based on the value in the new unit.</param>
        /// <param name="culture">Culture used in double.ToString(<see cref="valueStringFormat" />, <see cref="culture" />).</param>
        /// <param name="valueStringFormat">
        ///     Format used in double.ToString(<see cref="valueStringFormat" />, <see cref="culture" />
        ///     ).
        /// </param>
        /// <returns></returns>
        private static string GetFileSize(ulong bytes, IList<GetSuffixFunc> suffixes, IFormatProvider culture,
            string valueStringFormat)
        {
            int unitSuffixIndex = bytes == 0 ? 0 : Convert.ToInt32(Math.Floor(Math.Log(bytes, 1000)));
            double valueInUnit = bytes/Math.Pow(1000, unitSuffixIndex);

            if (valueStringFormat == null)
                valueStringFormat = GetDefaultStringFormat(valueInUnit);

            GetSuffixFunc suffixFunc = suffixes[unitSuffixIndex];

            // Example: 5.35 MB or 5.35 megabytes
            string readable = valueInUnit.ToString(valueStringFormat, culture) + " " + suffixFunc(valueInUnit);
            return readable;
        }

        private static string GetDefaultStringFormat(double valueInUnit)
        {
            string stringFormat;
            double abs = Math.Abs(valueInUnit);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (abs == 0)
                stringFormat = "0";
            else if (abs < 10)
                stringFormat = "0.##";
            else if (abs < 100)
                stringFormat = "0.#";
            else
            {
                stringFormat = "0";
            }
            return stringFormat;
        }

        #endregion
    }
}