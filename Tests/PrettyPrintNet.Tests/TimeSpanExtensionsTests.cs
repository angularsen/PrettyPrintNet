using System;
using System.Globalization;
using NUnit.Framework;

namespace PrettyPrintNet.Tests
{
    [TestFixture]
    public class TimeSpanExtensionsTests
    {
        [Test]
        public void MaxUnitGroupsOf0Throws()
        {
            var t = new TimeSpan(days: 2, hours: 3, minutes: 4, seconds: 5, milliseconds: 6);
            Assert.Throws<ArgumentException>(() => t.ToPrettyString(0));
        }

        [Test]
        public void ZeroFallsBackToLowestUnitWithSecondsAsDefault()
        {
            Assert.AreEqual("0 seconds", TimeSpan.Zero.ToPrettyString(3));
            Assert.AreEqual("0 hours", TimeSpan.Zero.ToPrettyString(3, lowestUnit: TimeSpanUnit.Hours));
        }

        [Test]
        public void ClampsToHighestAndLowestUnit()
        {
            var t = new TimeSpan(days: 2, hours: 3, minutes: 4, seconds: 5, milliseconds: 6);
            Assert.AreEqual("3 hours and 4 minutes", t.ToPrettyString(4, higestUnit: TimeSpanUnit.Hours, lowestUnit: TimeSpanUnit.Minutes));
        }
        
        [Test]
        public void DifferentUnitRepresentations()
        {
            var t = new TimeSpan(hours: 3, minutes: 4, seconds: 0);
            Assert.AreEqual("3 hours and 4 minutes", t.ToPrettyString(4));
// ReSharper disable once RedundantArgumentDefaultValue
            Assert.AreEqual("3 hours and 4 minutes", t.ToPrettyString(2, rep: UnitStringRepresentation.Long));
            Assert.AreEqual("3 hrs 4 mins", t.ToPrettyString(2, rep: UnitStringRepresentation.Short));
            Assert.AreEqual("3h4m", t.ToPrettyString(4, rep: UnitStringRepresentation.Compact));
        }

        [Test]
        public void UsesSpecifiedCulture()
        {
            // TODO: Only using integers, not sure if any cultures print that differently?
            var t = new TimeSpan(hours: 3, minutes: 4, seconds: 0);
            Assert.AreEqual("3 hours and 4 minutes", t.ToPrettyString(4, formatProvider: CultureInfo.GetCultureInfo("en-US")));
            Assert.AreEqual("3 hours and 4 minutes", t.ToPrettyString(4, formatProvider: CultureInfo.GetCultureInfo("zh-CN")));
        }

        [Test]
        public void MaxUnitGroupsOf1ReturnsLargestPart()
        {
            var hoursAndSome = new TimeSpan(days: 0, hours: 2, minutes: 3, seconds: 4, milliseconds: 5);
            var minutesAndSome = new TimeSpan(days: 0, hours: 0, minutes: 3, seconds: 5, milliseconds: 6);

            Assert.AreEqual("2 hours", hoursAndSome.ToPrettyString(1));
            Assert.AreEqual("3 minutes", minutesAndSome.ToPrettyString(1));
        }

        [Test]
        public void MaxUnitGroupsReturnsNLargestParts()
        {
            var daysAndSome = new TimeSpan(days: 2, hours: 3, minutes: 4, seconds: 5, milliseconds: 6);
            Assert.AreEqual("2 days and 3 hours", daysAndSome.ToPrettyString(2));
            Assert.AreEqual("2 days, 3 hours and 4 minutes", daysAndSome.ToPrettyString(3));

            var hoursAndSome = new TimeSpan(days: 0, hours: 3, minutes: 4, seconds: 5, milliseconds: 6);
            Assert.AreEqual("3 hours and 4 minutes", hoursAndSome.ToPrettyString(2));
            Assert.AreEqual("3 hours, 4 minutes and 5 seconds", hoursAndSome.ToPrettyString(3));
        }

        //[Test]
        //public void ReturnsMicroseconds()
        //{
        //    // 1 tick == 0.1 us, 50 ticks == 5 us
        //    var nanoTime = new TimeSpan(50);
        //    Assert.AreEqual("5 microseconds", nanoTime.ToPrettyString(TimeSpanUnit.Microseconds));
        //}

        //[Test]
        //public void ReturnsNanoseconds()
        //{
        //    // 1 tick == 100ns
        //    var nanoTime = new TimeSpan(5);
        //    Assert.AreEqual("500 nanoseconds", nanoTime.ToPrettyString(TimeSpanUnit.Nanoseconds));
        //}
    }
}