using System;
using System.Globalization;
using NUnit.Framework;

namespace PrettyPrintNet.Tests
{
    [TestFixture]
    public class FileSizeTests
    {
        private readonly CultureInfo _enUsCulture = new CultureInfo("en-US");

        [Test]
        public void ReturnsBytesForZeroSize()
        {
            Assert.AreEqual("0 bytes", FileSize.ToLongString(0, _enUsCulture));
            Assert.AreEqual("0 B", FileSize.ToShortString(0, _enUsCulture));
        }

        [Test]
        public void ReturnsBytesUnder1KB()
        {
            Assert.AreEqual("1 byte", FileSize.ToLongString(1, _enUsCulture));
            Assert.AreEqual("1 B", FileSize.ToShortString(1, _enUsCulture));
            Assert.AreEqual("500 bytes", FileSize.ToLongString(500, _enUsCulture));
            Assert.AreEqual("500 B", FileSize.ToShortString(500, _enUsCulture));
        }

        [Test]
        public void ReturnsKiloBytesUnder1MB()
        {
            ulong KB = 1024;
            Assert.AreEqual("1 kilobyte", FileSize.ToLongString(1 * KB, _enUsCulture));
            Assert.AreEqual("1 KB", FileSize.ToShortString(1 * KB, _enUsCulture));
            Assert.AreEqual("500 kilobytes", FileSize.ToLongString(500 * KB, _enUsCulture));
            Assert.AreEqual("500 KB", FileSize.ToShortString(500 * KB, _enUsCulture));
        }

        [Test]
        public void ReturnsMegaBytesUnder1GB()
        {
            ulong MB = Convert.ToUInt64(Math.Pow(1024, 2));
            Assert.AreEqual("1 megabyte", FileSize.ToLongString(1 * MB, _enUsCulture));
            Assert.AreEqual("1 MB", FileSize.ToShortString(1 * MB, _enUsCulture));
            Assert.AreEqual("500 megabytes", FileSize.ToLongString(500 * MB, _enUsCulture));
            Assert.AreEqual("500 MB", FileSize.ToShortString(500 * MB, _enUsCulture));
        }

        [Test]
        public void ReturnsGigaBytesUnder1TB()
        {
            ulong GB = Convert.ToUInt64(Math.Pow(1024, 3));
            Assert.AreEqual("1 gigabyte", FileSize.ToLongString(1 * GB, _enUsCulture));
            Assert.AreEqual("1 GB", FileSize.ToShortString(1 * GB, _enUsCulture));
            Assert.AreEqual("500 gigabytes", FileSize.ToLongString(500 * GB, _enUsCulture));
            Assert.AreEqual("500 GB", FileSize.ToShortString(500 * GB, _enUsCulture));
        }

        [Test]
        public void ReturnsTeraBytesUnder1PB()
        {
            ulong TB = Convert.ToUInt64(Math.Pow(1024, 4));
            Assert.AreEqual("1 terabyte", FileSize.ToLongString(1 * TB, _enUsCulture));
            Assert.AreEqual("1 TB", FileSize.ToShortString(1 * TB, _enUsCulture));
            Assert.AreEqual("500 terabytes", FileSize.ToLongString(500 * TB, _enUsCulture));
            Assert.AreEqual("500 TB", FileSize.ToShortString(500 * TB, _enUsCulture));
        }

        [Test]
        public void ReturnsPetaBytesUnder1EB()
        {
            ulong PB = Convert.ToUInt64(Math.Pow(1024, 5));
            Assert.AreEqual("1 petabyte", FileSize.ToLongString(1 * PB, _enUsCulture));
            Assert.AreEqual("1 PB", FileSize.ToShortString(1 * PB, _enUsCulture));
            Assert.AreEqual("500 petabytes", FileSize.ToLongString(500 * PB, _enUsCulture));
            Assert.AreEqual("500 PB", FileSize.ToShortString(500 * PB, _enUsCulture));
        }

        //[Test]
        //public void ReturnsExaBytesUnder1024EB()
        //{
        //    ulong EB = Convert.ToUInt64(Math.Pow(1024, 6));
        //    Assert.AreEqual("1 ExaByte", FileSize.ToLongString(1 * EB, _enUsCulture));
        //    Assert.AreEqual("1 EB", FileSize.ToShortString(1 * EB, _enUsCulture));
        //    Assert.AreEqual("500 ExaBytes", FileSize.ToLongString(500 * EB, _enUsCulture));
        //    Assert.AreEqual("500 EB", FileSize.ToShortString(500 * EB, _enUsCulture));
        //}
    }
}