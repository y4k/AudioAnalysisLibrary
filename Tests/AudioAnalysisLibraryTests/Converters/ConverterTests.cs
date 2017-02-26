using AudioAnalysisLibrary.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AudioAnalysisLibraryTests.Converters
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        public void ToSigned24Bit_ZeroTest()
        {
            const int expected = 0;

            var bytes = new byte[] { 0, 0, 0 };

            var actual = Bit24Converter.ToSignedInt24(bytes);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToSigned24Bit_MaxTest()
        {
            const int expected = 8388607; //Max value of 24 bit signed = 2^23 - 1

            var bytes = new byte[] { 255, 255, 127 };

            var actual = Bit24Converter.ToSignedInt24(bytes);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToSigned24Bit_MinTest()
        {
            const int expected = -8388608; //Max value of 24 bit signed = -2^23

            var bytes = new byte[] { 0, 0, 128 };

            var actual = Bit24Converter.ToSignedInt24(bytes);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToUnsigned24Bit_ZeroTest()
        {
            const int expected = 0;

            var bytes = new byte[] { 0, 0, 0 };

            var actual = Bit24Converter.ToUnsignedInt24(bytes);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToUnsigned24Bit_HalfTest()
        {
            const int expected = 8388607; //Max value of 24 bit signed = 2^24 - 1

            var bytes = new byte[] { 255, 255, 127 };

            var actual = Bit24Converter.ToUnsignedInt24(bytes);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToUnsigned24Bit_MaxTest()
        {
            const int expected = 16777215; //Max value of 24 bit signed = 2^24 - 1

            var bytes = new byte[] { 255, 255, 255 };

            var actual = Bit24Converter.ToUnsignedInt24(bytes);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnsignedToBytes_MaxTest()
        {
            var actual = Bit24Converter.UnsignedInt24ToBytes(16777215);

            var expected = new byte[] {255, 255, 255};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnsignedToBytes_ZeroTest()
        {
            var actual = Bit24Converter.UnsignedInt24ToBytes(0);

            var expected = new byte[] { 0, 0, 0 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnsignedToBytes_HalfTest()
        {
            var actual = Bit24Converter.UnsignedInt24ToBytes(8388607);

            var expected = new byte[] { 255, 255, 127 };
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}