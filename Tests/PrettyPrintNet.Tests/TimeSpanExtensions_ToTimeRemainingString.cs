using System;
using NUnit.Framework;

namespace PrettyPrintNet.Tests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class TimeSpanExtensions_ToTimeRemainingString
    {
        [TestCase(3661, Result = "1 hour and 2 minutes")]
        [TestCase(3659, Result = "1 hour and 59 seconds")]
        [TestCase(3601, Result = "1 hour and 1 second")]
        [TestCase(3600.5, Result = "1 hour and 1 second")]
        [TestCase(3600, Result = "1 hour")]
        [TestCase(3599.9, Result = "1 hour")]
        [TestCase(60.1, Result = "1 minute and 1 second")]
        [TestCase(60, Result = "1 minute")]
        [TestCase(59.9, Result = "1 minute")]
        [TestCase(2, Result = "2 seconds")]
        [TestCase(1.1, Result = "2 seconds")]
        [TestCase(1, Result = "1 second")]
        [TestCase(0.1, Result = "1 second")]
        [TestCase(0, Result = "0 seconds")]
        [Test]
        public string RoundsSmallestUnitUp(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).ToTimeRemainingString(2);
        }
    }
}