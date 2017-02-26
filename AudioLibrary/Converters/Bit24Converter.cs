using System;
using System.Linq;

namespace AudioAnalysisLibrary.Converters
{
    public static class Bit24Converter
    {
        public static int ToSignedInt24(byte[] bytes)
        {
            if (bytes.Length != 3)
            {
                throw new ArgumentException("Expected three bytes to convert to 24-bit integer.");
            }

            return ((bytes[2] << 24) | (bytes[1] << 16) | bytes[0] << 8) >> 8;
        }

        public static int ToUnsignedInt24(byte[] bytes)
        {
            if (bytes.Length != 3)
            {
                throw new ArgumentException("Expected three bytes to convert to 24-bit integer.");
            }

            return bytes[0] + (bytes[1] << 8) + (bytes[2] << 16);
        }

        public static byte[] UnsignedInt24ToBytes(int i)
        {
            var bytes = new[] { (byte)(i >> 24), (byte)(i >> 16), (byte)(i >> 8), (byte)i };
            return BitConverter.IsLittleEndian ? bytes.Skip(1).Reverse().ToArray() : bytes.Take(3).ToArray();
        }

        public static byte[] SignedInt24ToBytes(int i)
        {
            return new[] { (byte)(i >> 24), (byte)(i >> 16), (byte)(i >> 8) };
        }
    }
}
