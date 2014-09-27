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
            Assert.AreEqual("0 Bytes", FileSize.GetLongFileSize(0, _enUsCulture));
            Assert.AreEqual("0 B", FileSize.GetShortFileSize(0, _enUsCulture));
        }

        [Test]
        public void ReturnsBytesUnder1KB()
        {
            Assert.AreEqual("1 Byte", FileSize.GetLongFileSize(1, _enUsCulture));
            Assert.AreEqual("1 B", FileSize.GetShortFileSize(1, _enUsCulture));
            Assert.AreEqual("500 Bytes", FileSize.GetLongFileSize(500, _enUsCulture));
            Assert.AreEqual("500 B", FileSize.GetShortFileSize(500, _enUsCulture));
        }

        [Test]
        public void ReturnsKiloBytesUnder1MB()
        {
            ulong KB = 1024;
            Assert.AreEqual("1 KiloByte", FileSize.GetLongFileSize(1 * KB, _enUsCulture));
            Assert.AreEqual("1 KB", FileSize.GetShortFileSize(1 * KB, _enUsCulture));
            Assert.AreEqual("500 KiloBytes", FileSize.GetLongFileSize(500 * KB, _enUsCulture));
            Assert.AreEqual("500 KB", FileSize.GetShortFileSize(500 * KB, _enUsCulture));
        }

        [Test]
        public void ReturnsMegaBytesUnder1GB()
        {
            ulong MB = Convert.ToUInt64(Math.Pow(1024, 2));
            Assert.AreEqual("1 MegaByte", FileSize.GetLongFileSize(1 * MB, _enUsCulture));
            Assert.AreEqual("1 MB", FileSize.GetShortFileSize(1 * MB, _enUsCulture));
            Assert.AreEqual("500 MegaBytes", FileSize.GetLongFileSize(500 * MB, _enUsCulture));
            Assert.AreEqual("500 MB", FileSize.GetShortFileSize(500 * MB, _enUsCulture));
        }

        [Test]
        public void ReturnsGigaBytesUnder1TB()
        {
            ulong GB = Convert.ToUInt64(Math.Pow(1024, 3));
            Assert.AreEqual("1 GigaByte", FileSize.GetLongFileSize(1 * GB, _enUsCulture));
            Assert.AreEqual("1 GB", FileSize.GetShortFileSize(1 * GB, _enUsCulture));
            Assert.AreEqual("500 GigaBytes", FileSize.GetLongFileSize(500 * GB, _enUsCulture));
            Assert.AreEqual("500 GB", FileSize.GetShortFileSize(500 * GB, _enUsCulture));
        }

        [Test]
        public void ReturnsTeraBytesUnder1PB()
        {
            ulong TB = Convert.ToUInt64(Math.Pow(1024, 4));
            Assert.AreEqual("1 TeraByte", FileSize.GetLongFileSize(1 * TB, _enUsCulture));
            Assert.AreEqual("1 TB", FileSize.GetShortFileSize(1 * TB, _enUsCulture));
            Assert.AreEqual("500 TeraBytes", FileSize.GetLongFileSize(500 * TB, _enUsCulture));
            Assert.AreEqual("500 TB", FileSize.GetShortFileSize(500 * TB, _enUsCulture));
        }

        [Test]
        public void ReturnsPetaBytesUnder1EB()
        {
            ulong PB = Convert.ToUInt64(Math.Pow(1024, 5));
            Assert.AreEqual("1 PetaByte", FileSize.GetLongFileSize(1 * PB, _enUsCulture));
            Assert.AreEqual("1 PB", FileSize.GetShortFileSize(1 * PB, _enUsCulture));
            Assert.AreEqual("500 PetaBytes", FileSize.GetLongFileSize(500 * PB, _enUsCulture));
            Assert.AreEqual("500 PB", FileSize.GetShortFileSize(500 * PB, _enUsCulture));
        }

        //[Test]
        //public void ReturnsExaBytesUnder1024EB()
        //{
        //    ulong EB = Convert.ToUInt64(Math.Pow(1024, 6));
        //    Assert.AreEqual("1 ExaByte", FileSize.GetLongFileSize(1 * EB, _enUsCulture));
        //    Assert.AreEqual("1 EB", FileSize.GetShortFileSize(1 * EB, _enUsCulture));
        //    Assert.AreEqual("500 ExaBytes", FileSize.GetLongFileSize(500 * EB, _enUsCulture));
        //    Assert.AreEqual("500 EB", FileSize.GetShortFileSize(500 * EB, _enUsCulture));
        //}
    }
}