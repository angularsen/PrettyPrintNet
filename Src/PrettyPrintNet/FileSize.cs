﻿#region Copyright
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
                    v => v == 1 ? "byte" : "bytes", 
                    v => v == 1 ? "kilobyte" : "kilobytes",
                    v => v == 1 ? "megabyte" : "megabytes", 
                    v => v == 1 ? "gigabyte" : "gigabytes", 
                    v => v == 1 ? "terabyte" : "terabytes",
                    v => v == 1 ? "petabyte" : "petabytes", 
                    //v => v == 1 ? "exaByte" : "exabytes"
                };

            GetSuffixFunc[] shortEnglishSuffixFuncs =
                {
                    v => "B", 
                    v => "KB",
                    v => "MB",
                    v => "GB",
                    v => "TB",
                    v => "PB", 
                    //v => "EB"
                };

            CultureToLongSuffixFuncs = new Dictionary<string, GetSuffixFunc[]>()
                {
                    {"en-US", longEnglishSuffixFuncs},
                    {"nb-NO", longEnglishSuffixFuncs},
                };

            CultureToShortSuffixFuncs = new Dictionary<string, GetSuffixFunc[]>()
                {
                    {"en-US", shortEnglishSuffixFuncs},
                    {"nb-NO", shortEnglishSuffixFuncs},
                };
        }


        public static string GetLongFileSize(ulong bytes, CultureInfo culture = null, string stringFormat = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            GetSuffixFunc[] suffixFuncs = GetSuffixesForCulture(culture, CultureToLongSuffixFuncs);
            return GetFileSize(bytes, suffixFuncs, culture, stringFormat);
        }

        public static string GetShortFileSize(ulong bytes, CultureInfo culture = null, string stringFormat = null)
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
        /// Get human readable text for file size, rounding to the largest unit where its value is >= 1.
        /// </summary>
        /// <param name="bytes">File size in bytes.</param>
        /// <param name="suffixes">Array of suffix functions, returning a suffix based on the value in the new unit.</param>
        /// <param name="culture">Culture used in double.ToString(<see cref="stringFormat"/>, <see cref="culture"/>).</param>
        /// <param name="stringFormat">Format used in double.ToString(<see cref="stringFormat"/>, <see cref="culture"/>).</param>
        /// <returns></returns>
        private static string GetFileSize(ulong bytes, GetSuffixFunc[] suffixes, CultureInfo culture, string stringFormat)
        {
            ulong suffixIndex = bytes == 0 ? 0 : Convert.ToUInt64(Math.Floor(Math.Log(bytes, 1024)));
            double valueInUnit = Math.Round(bytes/Math.Pow(1024, suffixIndex), 1);

            GetSuffixFunc suffixFunc = suffixes[suffixIndex];

            // Example: 5.35 MB or 5.35 megabytes
            string readable = valueInUnit.ToString(stringFormat, culture) + " " + suffixFunc(valueInUnit);
            return readable;
        }

        #endregion
    }
}