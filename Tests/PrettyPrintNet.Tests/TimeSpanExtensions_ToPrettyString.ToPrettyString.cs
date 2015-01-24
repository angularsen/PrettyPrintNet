using System;
using System.Globalization;
using NUnit.Framework;

namespace PrettyPrintNet.Tests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class TimeSpanExtensions_ToPrettyString
    {
        [Test]
        public void ShowsAtLeastOneUnitValue()
        {
            var t = new TimeSpan(2, 3, 4, 5, 6);
            Assert.AreEqual("2 days", t.ToPrettyString(0));
            Assert.AreEqual("2 days", t.ToPrettyString(-1));
        }

        [Test]
        public void RepresentsTimeSpanZeroInLowestConfiguredUnit()
        {
            Assert.AreEqual("0 seconds", TimeSpan.Zero.ToPrettyString(3));
            Assert.AreEqual("0 hours", TimeSpan.Zero.ToPrettyString(3, lowestUnit: TimeSpanUnit.Hours));
        }

        [Test]
        public void ClampsToHighestAndLowestUnit()
        {
            var t = new TimeSpan(2, 3, 4, 5, 6);
            Assert.AreEqual("3 hours and 4 minutes",
                t.ToPrettyString(4, highestUnit: TimeSpanUnit.Hours, lowestUnit: TimeSpanUnit.Minutes));
        }

        [TestCase(UnitStringRepresentation.Long, Result = "3 hours and 4 minutes")]
        [TestCase(UnitStringRepresentation.Short, Result = "3 hrs 4 mins")]
        [TestCase(UnitStringRepresentation.CompactWithSpace, Result = "3h 4m")]
        [TestCase(UnitStringRepresentation.Compact, Result = "3h4m")]
        public string ShowsUnitValuesInConfiguredUnitRepresentation(UnitStringRepresentation rep)
        {
            var t = new TimeSpan(3, 4, 0);
            return t.ToPrettyString(2, rep);
        }

        [Test]
        public void UsesSpecifiedCulture()
        {
            // TODO: Not sure if any cultures print integers differently?
            var t = new TimeSpan(3, 4, 0);
            Assert.AreEqual("3 hours and 4 minutes",
                t.ToPrettyString(4, formatProvider: CultureInfo.GetCultureInfo("en-US")));
            Assert.AreEqual("3 hours and 4 minutes",
                t.ToPrettyString(4, formatProvider: CultureInfo.GetCultureInfo("zh-CN")));
        }

        [Test]
        [TestCase(1, Result = "2 days")]
        [TestCase(2, Result = "2 days and 3 hours")]
        [TestCase(3, Result = "2 days, 3 hours and 5 seconds")]
        [TestCase(4, Result = "2 days, 3 hours, 5 seconds and 6 milliseconds")]
        public string ReturnsNoMoreThanMaxUnitValues(int maxUnitsGroups)
        {
            var t = new TimeSpan(2, 3, 0, 5, 6);
            return t.ToPrettyString(maxUnitsGroups, lowestUnit: TimeSpanUnit.Milliseconds);
        }

        [TestCase(3.5, IntegerRounding.Down, Result = "3 hours")]
        [TestCase(2.9, IntegerRounding.Down, Result = "2 hours")]
        [TestCase(3.5, IntegerRounding.Up, Result = "4 hours")]
        [TestCase(3.4, IntegerRounding.Up, Result = "4 hours")]
        [TestCase(3.5, IntegerRounding.ToNearestOrUp, Result = "4 hours")]
        [TestCase(3.4, IntegerRounding.ToNearestOrUp, Result = "3 hours")]
        public string RoundsLowestUnitAsSpecified(double hours, IntegerRounding rounding)
        {
            return TimeSpan.FromHours(hours).ToPrettyString(lowestUnitRounding: rounding);
        }

        [TestCase(24*3600 - 1, Result = "1 day")]
        [TestCase(3600 - 1, Result = "1 hour")]
        [TestCase(60 - 0.1, Result = "1 minute")]
        public string RoundsUnitUp(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).ToPrettyString(lowestUnitRounding: IntegerRounding.Up);
        }

        [TestCase(3600 - 1, Result = "59 minutes")]
        [TestCase(59.9, Result = "59 seconds")]
        [TestCase(0.9, Result = "0 seconds")]
        public string RoundsUnitDown(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).ToPrettyString(lowestUnitRounding: IntegerRounding.Down);
        }
    }
}