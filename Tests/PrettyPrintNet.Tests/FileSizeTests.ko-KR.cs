using System;
using System.Globalization;
using NUnit.Framework;

namespace PrettyPrintNet.Tests
{
    [TestFixture]
    public class FileSizeTests_ko_KR
    {
        private readonly CultureInfo _koKRCulture = new CultureInfo("ko-KR");

        [Test]
        public void ReturnsBytesForZeroSize()
        {
            Assert.AreEqual("0 바이트", FileSize.ToLongString(0, _koKRCulture));
            Assert.AreEqual("0 B", FileSize.ToShortString(0, _koKRCulture));
        }

        [Test]
        public void ReturnsBytesUnder1KB()
        {
            Assert.AreEqual("1 바이트", FileSize.ToLongString(1, _koKRCulture));
            Assert.AreEqual("1 B", FileSize.ToShortString(1, _koKRCulture));
            Assert.AreEqual("500 바이트", FileSize.ToLongString(500, _koKRCulture));
            Assert.AreEqual("500 B", FileSize.ToShortString(500, _koKRCulture));
        }

        [Test]
        public void ReturnsKiloBytesUnder1MB()
        {
            ulong KB = 1000;
            Assert.AreEqual("1 키로바이트", FileSize.ToLongString(1*KB, _koKRCulture));
            Assert.AreEqual("1 KB", FileSize.ToShortString(1*KB, _koKRCulture));
            Assert.AreEqual("500 키로바이트", FileSize.ToLongString(500*KB, _koKRCulture));
            Assert.AreEqual("500 KB", FileSize.ToShortString(500*KB, _koKRCulture));
        }

        [Test]
        public void ReturnsMegaBytesUnder1GB()
        {
            ulong MB = Convert.ToUInt64(Math.Pow(1000, 2));
            Assert.AreEqual("1 메가바이트", FileSize.ToLongString(1*MB, _koKRCulture));
            Assert.AreEqual("1 MB", FileSize.ToShortString(1*MB, _koKRCulture));
            Assert.AreEqual("500 메가바이트", FileSize.ToLongString(500*MB, _koKRCulture));
            Assert.AreEqual("500 MB", FileSize.ToShortString(500*MB, _koKRCulture));
        }

        [Test]
        public void ReturnsGigaBytesUnder1TB()
        {
            ulong GB = Convert.ToUInt64(Math.Pow(1000, 3));
            Assert.AreEqual("1 기가바이트", FileSize.ToLongString(1*GB, _koKRCulture));
            Assert.AreEqual("1 GB", FileSize.ToShortString(1*GB, _koKRCulture));
            Assert.AreEqual("500 기가바이트", FileSize.ToLongString(500*GB, _koKRCulture));
            Assert.AreEqual("500 GB", FileSize.ToShortString(500*GB, _koKRCulture));
        }

        [Test]
        public void ReturnsTeraBytesUnder1PB()
        {
            ulong TB = Convert.ToUInt64(Math.Pow(1000, 4));
            Assert.AreEqual("1 테라바이트", FileSize.ToLongString(1*TB, _koKRCulture));
            Assert.AreEqual("1 TB", FileSize.ToShortString(1*TB, _koKRCulture));
            Assert.AreEqual("500 테라바이트", FileSize.ToLongString(500*TB, _koKRCulture));
            Assert.AreEqual("500 TB", FileSize.ToShortString(500*TB, _koKRCulture));
        }

        [Test]
        public void ReturnsPetaBytesUnder1EB()
        {
            ulong PB = Convert.ToUInt64(Math.Pow(1000, 5));
            Assert.AreEqual("1 페라바이트", FileSize.ToLongString(1*PB, _koKRCulture));
            Assert.AreEqual("1 PB", FileSize.ToShortString(1*PB, _koKRCulture));
            Assert.AreEqual("500 페라바이트", FileSize.ToLongString(500*PB, _koKRCulture));
            Assert.AreEqual("500 PB", FileSize.ToShortString(500*PB, _koKRCulture));
        }

        [Test]
        public void ReturnsExaBytesUnder1000EB()
        {
            // 64-bit unsigned long can only hold 4.6 exabytes of bytes
            ulong EB = Convert.ToUInt64(Math.Pow(1000, 6));
            Assert.AreEqual("1 엑사바이트", FileSize.ToLongString(1*EB, _koKRCulture));
            Assert.AreEqual("1 EB", FileSize.ToShortString(1*EB, _koKRCulture));
            Assert.AreEqual("4 엑사바이트", FileSize.ToLongString(4*EB, _koKRCulture));
            Assert.AreEqual("4 EB", FileSize.ToShortString(4*EB, _koKRCulture));
        }

        [Test]
        public void DefaultStringFormatUseFewerDecimalsForLargerValues()
        {
            ulong MB = Convert.ToUInt64(Math.Pow(1000, 2));
            Assert.AreEqual("0 바이트", FileSize.ToLongString(0, _koKRCulture));
            Assert.AreEqual("1.23 메가바이트", FileSize.ToLongString(Convert.ToUInt64(1.23456*MB), _koKRCulture));
            Assert.AreEqual("10.2 메가바이트", FileSize.ToLongString(Convert.ToUInt64(10.23456*MB), _koKRCulture));
            Assert.AreEqual("100 메가바이트", FileSize.ToLongString(Convert.ToUInt64(100.23456*MB), _koKRCulture));
        }

        [Test]
        public void AppliesCustomStringFormatToValue()
        {
            ulong MB = Convert.ToUInt64(Math.Pow(1000, 2));
            const string stringFormat = "0.000";
            Assert.AreEqual("0.000 바이트", FileSize.ToLongString(0, _koKRCulture, stringFormat));
            Assert.AreEqual("1.235 메가바이트",
                FileSize.ToLongString(Convert.ToUInt64(1.23456*MB), _koKRCulture, stringFormat));
            Assert.AreEqual("10.235 메가바이트",
                FileSize.ToLongString(Convert.ToUInt64(10.23456*MB), _koKRCulture, stringFormat));
            Assert.AreEqual("100.235 메가바이트",
                FileSize.ToLongString(Convert.ToUInt64(100.23456*MB), _koKRCulture, stringFormat));
        }
    }
}