using System;
using AudioAnalysisLibrary.Enumerations;

namespace AudioAnalysisLibrary.DataAcquisition
{
    public static class NormaliseData
    {
        public static float NormaliseFloatSample(float sample, BitDepth bitDepth)
        {
            switch (bitDepth)
            {
                case BitDepth.Bit8:
                    return NormaliseFloatSample8Bit(sample);
                case BitDepth.Bit16:
                    return NormaliseFloatSample16Bit(sample);
                case BitDepth.Bit24:
                    return NormaliseFloatSample24Bit(sample);
                case BitDepth.Bit32:
                    return NormaliseFloatSample32Bit(sample);
                default:
                    throw new ArgumentOutOfRangeException(nameof(bitDepth), bitDepth, "Supplied bit depth was not an enum member.");
            }
        }

        private static float NormaliseFloatSample8Bit(float sample)
        {
            return sample / 256;
        }

        private static float NormaliseFloatSample16Bit(float sample)
        {
            return sample / 65536;
        }

        private static float NormaliseFloatSample24Bit(float sample)
        {
            return sample / 16777216;
        }

        private static float NormaliseFloatSample32Bit(float sample)
        {
            return sample / 4294967296;
        }
    }
}
