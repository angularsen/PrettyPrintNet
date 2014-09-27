using System;
using NUnit.Framework;

namespace PrettyPrintNet.Tests
{
    [TestFixture]
    public class TimeSpanExtensionsTests
    {
        [Test]
        public void MaxParts0Returns0Seconds()
        {
            var t = new TimeSpan(2, 3, 4, 5, 6);
            Assert.AreEqual("0 seconds", t.ToPrettyString(0));
        }

        [Test]
        public void MaxParts1ReturnsLargestPart()
        {
            var hours = new TimeSpan(0, 2, 3, 4, 5);
            var minutes = new TimeSpan(0, 0, 3, 5, 6);

            Assert.AreEqual("2 hours", hours.ToPrettyString(1));
            Assert.AreEqual("3 minutes", minutes.ToPrettyString(1));
        }

        [Test]
        public void MaxPartsNReturnsNLargestParts()
        {
            var daysAndSome = new TimeSpan(2, 3, 4, 5, 6);
            Assert.AreEqual("2 days and 3 hours", daysAndSome.ToPrettyString(2));
            Assert.AreEqual("2 days, 3 hours and 4 minutes", daysAndSome.ToPrettyString(3));

            var hoursAndSome = new TimeSpan(0, 3, 4, 5, 6);
            Assert.AreEqual("3 hours and 4 minutes", hoursAndSome.ToPrettyString(2));
            Assert.AreEqual("3 hours, 4 minutes and 5 seconds", hoursAndSome.ToPrettyString(3));
        }

        [Test]
        public void OmitsZeroByDefault()
        {
            var t = new TimeSpan(0, 0, 4, 0, 6);
            Assert.AreEqual("4 minutes and 6 milliseconds", t.ToPrettyString(TimeSpanParts.All));
        }

        [Test]
        public void OnlyExplicitlySpecifiedPartsAreReturned()
        {
            var t = new TimeSpan(2, 3, 4, 5, 6);
            Assert.AreEqual("2 days and 4 minutes", t.ToPrettyString(TimeSpanParts.Days | TimeSpanParts.Minutes));
            Assert.AreEqual("4 minutes and 6 milliseconds",
                t.ToPrettyString(TimeSpanParts.Minutes | TimeSpanParts.Milliseconds));
        }

        [Test]
        public void ReturnsMicroseconds()
        {
            // 1 tick == 0.1 us, 50 ticks == 5 us
            var nanoTime = new TimeSpan(50);
            Assert.AreEqual("5 microseconds", nanoTime.ToPrettyString(TimeSpanParts.Microseconds));
        }

        [Test]
        public void ReturnsNanoseconds()
        {
            // 1 tick == 100ns
            var nanoTime = new TimeSpan(5);
            Assert.AreEqual("500 nanoseconds", nanoTime.ToPrettyString(TimeSpanParts.Nanoseconds));
        }

        [Test]
        public void ReturnsZeroIfExplicitlySpecified()
        {
            var t = new TimeSpan(0, 0, 4, 0, 6);
            Assert.AreEqual("0 days, 0 hours, 4 minutes, 0 seconds and 6 milliseconds",
                t.ToPrettyString(TimeSpanParts.MillisecondsAndUp, true));
        }
    }
}