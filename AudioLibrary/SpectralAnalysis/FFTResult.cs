using System.Numerics;

namespace AudioAnalysisLibrary.SpectralAnalysis
{
    public class FftResult
    {
        public double[] AmplitudeSpectra { get; }
        public double[] PhaseSpectra { get; }
        public double[] Frequencies { get; }
        public Complex[] FftValues{ get; }

        public FftResult(double[] frequencies, Complex[] fftValues)
        {
            FftValues = fftValues;
            Frequencies = frequencies;
        }
    }
}
