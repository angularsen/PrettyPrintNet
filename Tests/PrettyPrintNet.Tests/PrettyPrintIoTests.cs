using System;
using System.Globalization;
using NUnit.Framework;

namespace PrettyPrintNet.Tests
{
    [TestFixture]
    public class PrettyPrintIoTests
    {
        private readonly CultureInfo _enUsCulture = new CultureInfo("en-US");

        [Test]
        public void ReturnsBytesForZeroSize()
        {
            Assert.AreEqual("0 Bytes", PrettyPrintIo.GetLongFileSize(0, _enUsCulture));
            Assert.AreEqual("0 B", PrettyPrintIo.GetShortFileSize(0, _enUsCulture));
        }

        [Test]
        public void ReturnsBytesUnder1KB()
        {
            Assert.AreEqual("1 Byte", PrettyPrintIo.GetLongFileSize(1, _enUsCulture));
            Assert.AreEqual("1 B", PrettyPrintIo.GetShortFileSize(1, _enUsCulture));
            Assert.AreEqual("500 Bytes", PrettyPrintIo.GetLongFileSize(500, _enUsCulture));
            Assert.AreEqual("500 B", PrettyPrintIo.GetShortFileSize(500, _enUsCulture));
        }

        [Test]
        public void ReturnsKiloBytesUnder1MB()
        {
            ulong KB = 1024;
            Assert.AreEqual("1 KiloByte", PrettyPrintIo.GetLongFileSize(1 * KB, _enUsCulture));
            Assert.AreEqual("1 KB", PrettyPrintIo.GetShortFileSize(1 * KB, _enUsCulture));
            Assert.AreEqual("500 KiloBytes", PrettyPrintIo.GetLongFileSize(500 * KB, _enUsCulture));
            Assert.AreEqual("500 KB", PrettyPrintIo.GetShortFileSize(500 * KB, _enUsCulture));
        }

        [Test]
        public void ReturnsMegaBytesUnder1GB()
        {
            ulong MB = Convert.ToUInt64(Math.Pow(1024, 2));
            Assert.AreEqual("1 MegaByte", PrettyPrintIo.GetLongFileSize(1 * MB, _enUsCulture));
            Assert.AreEqual("1 MB", PrettyPrintIo.GetShortFileSize(1 * MB, _enUsCulture));
            Assert.AreEqual("500 MegaBytes", PrettyPrintIo.GetLongFileSize(500 * MB, _enUsCulture));
            Assert.AreEqual("500 MB", PrettyPrintIo.GetShortFileSize(500 * MB, _enUsCulture));
        }

        [Test]
        public void ReturnsGigaBytesUnder1TB()
        {
            ulong GB = Convert.ToUInt64(Math.Pow(1024, 3));
            Assert.AreEqual("1 GigaByte", PrettyPrintIo.GetLongFileSize(1 * GB, _enUsCulture));
            Assert.AreEqual("1 GB", PrettyPrintIo.GetShortFileSize(1 * GB, _enUsCulture));
            Assert.AreEqual("500 GigaBytes", PrettyPrintIo.GetLongFileSize(500 * GB, _enUsCulture));
            Assert.AreEqual("500 GB", PrettyPrintIo.GetShortFileSize(500 * GB, _enUsCulture));
        }

        [Test]
        public void ReturnsTeraBytesUnder1PB()
        {
            ulong TB = Convert.ToUInt64(Math.Pow(1024, 4));
            Assert.AreEqual("1 TeraByte", PrettyPrintIo.GetLongFileSize(1 * TB, _enUsCulture));
            Assert.AreEqual("1 TB", PrettyPrintIo.GetShortFileSize(1 * TB, _enUsCulture));
            Assert.AreEqual("500 TeraBytes", PrettyPrintIo.GetLongFileSize(500 * TB, _enUsCulture));
            Assert.AreEqual("500 TB", PrettyPrintIo.GetShortFileSize(500 * TB, _enUsCulture));
        }

        [Test]
        public void ReturnsPetaBytesUnder1EB()
        {
            ulong PB = Convert.ToUInt64(Math.Pow(1024, 5));
            Assert.AreEqual("1 PetaByte", PrettyPrintIo.GetLongFileSize(1 * PB, _enUsCulture));
            Assert.AreEqual("1 PB", PrettyPrintIo.GetShortFileSize(1 * PB, _enUsCulture));
            Assert.AreEqual("500 PetaBytes", PrettyPrintIo.GetLongFileSize(500 * PB, _enUsCulture));
            Assert.AreEqual("500 PB", PrettyPrintIo.GetShortFileSize(500 * PB, _enUsCulture));
        }

        //[Test]
        //public void ReturnsExaBytesUnder1024EB()
        //{
        //    ulong EB = Convert.ToUInt64(Math.Pow(1024, 6));
        //    Assert.AreEqual("1 ExaByte", PrettyPrintIo.GetLongFileSize(1 * EB, _enUsCulture));
        //    Assert.AreEqual("1 EB", PrettyPrintIo.GetShortFileSize(1 * EB, _enUsCulture));
        //    Assert.AreEqual("500 ExaBytes", PrettyPrintIo.GetLongFileSize(500 * EB, _enUsCulture));
        //    Assert.AreEqual("500 EB", PrettyPrintIo.GetShortFileSize(500 * EB, _enUsCulture));
        //}
    }
}