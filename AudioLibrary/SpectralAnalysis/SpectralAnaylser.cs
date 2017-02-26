using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;

namespace AudioAnalysisLibrary.SpectralAnalysis
{
    public class SpectralAnaylser
    {
        /// <summary>
        /// Calculates the Fast Fourier Transform of a data sample. Returns the complex FFT.
        /// </summary>
        /// <param name="sample">IEnumerable of double values.</param>
        /// <returns>IEnumerable of Complex values.</returns>
        public static IEnumerable<Complex> FFT(IEnumerable<double> sample)
        {
            var real = sample.ToArray();
            var imag = new double[real.Length];

            Array.Clear(imag,0,imag.Length);

            Fourier.Forward(real, imag, FourierOptions.NoScaling);

            return real.Zip(imag, (r,i) => new Complex(r/real.Length,i/imag.Length));
        }

        /// <summary>
        /// Expects normalised FFT Complex Enumerable
        /// </summary>
        /// <param name="fftData"></param>
        /// <returns></returns>
        public static IEnumerable<double> PowerSpectrum(IEnumerable<Complex> fftData)
        {
            var data = fftData.Select(x => (x*x.Conjugate()).Real).ToList();
            var numSamples = data.Count;

            var dcValue = data[0];

            var singleSide = data.Skip(1).Take(numSamples / 2 - 1).Select(x => x * 2);

            var r = new List<double>
            {
                dcValue
            };

            r.AddRange(singleSide);

            return r;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fftData"></param>
        /// <returns></returns>
        public static IEnumerable<double> PhaseSpectrum(IEnumerable<Complex> fftData)
        {
            var data = fftData.Select(x => Math.Atan2(x.Imaginary, x.Real)).ToList();
            var numSamples = data.Count;

            return data.Take(numSamples / 2 - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="powerSpectrum"></param>
        /// <returns></returns>
        public static double RmsFromPowerSpec(IEnumerable<double> powerSpectrum)
        {
            return Math.Sqrt(powerSpectrum.Sum());
        }

        // ReSharper disable once InconsistentNaming
        public static double FindDCOffset(IEnumerable<Complex> fftData)
        {
            if (fftData == null)
            {
                throw new ArgumentNullException(nameof(fftData), "Power Spectrum was null");
            }

            var data = fftData.ToList();
            var dcValue = data[0];

            var dcMagnitude = dcValue.Magnitude;

            var dcPhaseAngle = Math.Atan2(dcValue.Imaginary, dcValue.Real) * 180 / Math.PI;

            return Math.Abs(dcPhaseAngle) < 0.1 ? dcMagnitude : -dcMagnitude;
        }
    }
}