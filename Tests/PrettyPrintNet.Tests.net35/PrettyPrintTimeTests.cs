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
            Assert.AreEqual("0 seconds", t.ToFriendlyString(0));
        }

        [Test]
        public void MaxParts1ReturnsLargestPart()
        {
            var hours = new TimeSpan(0, 2, 3, 4, 5);
            var minutes = new TimeSpan(0, 0, 3, 5, 6);

            Assert.AreEqual("2 hours", hours.ToFriendlyString(1));
            Assert.AreEqual("3 minutes", minutes.ToFriendlyString(1));
        }

        [Test]
        public void MaxPartsNReturnsNLargestParts()
        {
            var daysAndSome = new TimeSpan(2, 3, 4, 5, 6);
            Assert.AreEqual("2 days and 3 hours", daysAndSome.ToFriendlyString(2));
            Assert.AreEqual("2 days, 3 hours and 4 minutes", daysAndSome.ToFriendlyString(3));

            var hoursAndSome = new TimeSpan(0, 3, 4, 5, 6);
            Assert.AreEqual("3 hours and 4 minutes", hoursAndSome.ToFriendlyString(2));
            Assert.AreEqual("3 hours, 4 minutes and 5 seconds", hoursAndSome.ToFriendlyString(3));
        }

        [Test]
        public void OmitsZeroByDefault()
        {
            var t = new TimeSpan(0, 0, 4, 0, 6);
            Assert.AreEqual("4 minutes and 6 milliseconds", t.ToFriendlyString(TimeSpanParts.All));
        }

        [Test]
        public void OnlyExplicitlySpecifiedPartsAreReturned()
        {
            var t = new TimeSpan(2, 3, 4, 5, 6);
            Assert.AreEqual("2 days and 4 minutes", t.ToFriendlyString(TimeSpanParts.Days | TimeSpanParts.Minutes));
            Assert.AreEqual("4 minutes and 6 milliseconds",
                t.ToFriendlyString(TimeSpanParts.Minutes | TimeSpanParts.Milliseconds));
        }

        [Test]
        public void ReturnsMicroseconds()
        {
            // 1 tick == 0.1 us, 50 ticks == 5 us
            var nanoTime = new TimeSpan(50);
            Assert.AreEqual("5 microseconds", nanoTime.ToFriendlyString(TimeSpanParts.Microseconds));
        }

        [Test]
        public void ReturnsNanoseconds()
        {
            // 1 tick == 100ns
            var nanoTime = new TimeSpan(5);
            Assert.AreEqual("500 nanoseconds", nanoTime.ToFriendlyString(TimeSpanParts.Nanoseconds));
        }

        [Test]
        public void ReturnsZeroIfExplicitlySpecified()
        {
            var t = new TimeSpan(0, 0, 4, 0, 6);
            Assert.AreEqual("0 days, 0 hours, 4 minutes, 0 seconds and 6 milliseconds",
                t.ToFriendlyString(TimeSpanParts.MillisecondsAndUp, true));
        }
    }
}