using System;
using System.Linq;
using System.Numerics;
using AudioAnalysisLibrary;
using AudioAnalysisLibrary.SpectralAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AudioAnalysisLibraryTests.SpectralAnalysis
{
    [TestClass]
    public class SpectralAnaylserTests
    {
        [TestMethod]
        public void SpectralAnaylserTest_PerformFFT_CorrectlyNormalised()
        {
            var sineGen = new SineWaveGenerator(1000, 100, 52100);

            var data = sineGen.GetNextXSamples(1000).SelectMany(x => x.Data);

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            //First 20 points
            var expectedData = new []
            {
                new Complex( 0.495146499549 , 0.0 ),
                new Complex( 0.496623688121 , -0.0407006202924 ),
                new Complex( 0.50110389839 , -0.0820705763766 ),
                new Complex( 0.508737124447 , -0.124816451001 ),
                new Complex( 0.519787536456 , -0.169723759968 ),
                new Complex( 0.534657178111 , -0.217708447252 ),
                new Complex( 0.553924673868 , -0.269885728482 ),
                new Complex( 0.578406338433 , -0.327667996773 ),
                new Complex( 0.60925275643 , -0.392911314212 ),
                new Complex( 0.648104449026 , -0.468144913463 ),
                new Complex( 0.697351019157 , -0.55694776582 ),
                new Complex( 0.760581685582 , -0.664598558928 ),
                new Complex( 0.843413020917 , -0.799265722731 ),
                new Complex( 0.955119828191 , -0.974348360982 ),
                new Complex( 1.11215125365 , -1.21351968778 ),
                new Complex( 1.34667848596 , -1.56298418792 ),
                new Complex( 1.73120148717 , -2.12675932178 ),
                new Complex( 2.47045436902 , -3.19844730597 ),
                new Complex( 4.45520400945 , -6.05525222361 ),
                new Complex( 26.9564012368 , -38.3272036733 )
            };

            for (var i = 0; i < expectedData.Length; i++)
            {
                Assert.AreEqual(expectedData[i].Real, fftData[i].Real, 0.0001);
                Assert.AreEqual(expectedData[i].Imaginary, fftData[i].Imaginary, 0.001);
            }
        }

        [TestMethod]
        public void SpectralAnalysis_PowerSpectrum_FromComplexFFT()
        {
            var sineGen = new SineWaveGenerator(1000, 100, 52100);

            var data = sineGen.GetNextXSamples(1000).SelectMany(x => x.Data);

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var powerSpectrum = SpectralAnaylser.PowerSpectrum(fftData).ToArray();

            //First 20 points
            var expectedData = new[]
            {
                0.245170056015 ,
                0.49658325619 ,
                0.515681392978 ,
                0.548785216463 ,
                0.597970475506 ,
                0.66651053222 ,
                0.759341701516 ,
                0.883840416896 ,
                1.05113644411 ,
                1.2783980737 ,
                1.59297851554 ,
                2.04035148994 ,
                2.70034243877 ,
                3.7232172295 ,
                5.41902088726 ,
                8.51292503252 ,
                15.0403276039 ,
                32.666419917 ,
                113.029844515 ,
                4391.24421811
            };

            for (var i = 0; i < expectedData.Length; i++)
            {
                Assert.AreEqual(expectedData[i], powerSpectrum[i], 0.0001);
            }
        }

        [TestMethod]
        public void SpectralAnalysis_PhaseSpectrum_FromComplexFFT()
        {
            var sineGen = new SineWaveGenerator(1000, 100, 52100);

            var data = sineGen.GetNextXSamples(1000).SelectMany(x => x.Data);

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var phaseSpectrum = SpectralAnaylser.PhaseSpectrum(fftData).ToArray();

            //First 20 points
            var expectedData = new[]
            {
                0.0,
                -0.0817719008353,
                -0.162338293035,
                -0.240593343449,
                -0.31561082457,
                -0.386691476229,
                -0.453375155817,
                -0.515423897005,
                -0.572786506438,
                -0.625555489858,
                -0.673924561297,
                -0.718151650664,
                -0.758529444166,
                -0.795363549908,
                -0.828957341093,
                -0.859602141888,
                -0.887571419288,
                -0.913117826042,
                -0.93647217888,
                -0.957843686925
            };

            for (var i = 0; i < expectedData.Length; i++)
            {
                Assert.AreEqual(expectedData[i], phaseSpectrum[i], 0.0001);
            }
        }

        [TestMethod]
        public void SpectralAnalysis_RMSFromPowerSpectrum()
        {
            var sineGen = new SineWaveGenerator(1000, 100, 52100);

            var d = sineGen.GetNextXSamples(1000).SelectMany(x => x.Data);

            var data = d.ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var powerSpectrum = SpectralAnaylser.PowerSpectrum(fftData).ToArray();

            var expected = Math.Sqrt(data.Select(x => x*x).Sum() / data.Count);

            var rmsPowerSpec = SpectralAnaylser.RmsFromPowerSpec(powerSpectrum);

            Assert.AreEqual(expected, rmsPowerSpec, 0.0001);
        }

        [TestMethod]
        public void SpectralAnalysis_PositiveDCOffsetTest()
        {
            const int offset = 100;
            
            var sineGen = new SineWaveGenerator(1000, 100, 52100, offset);

            var d = sineGen.GetNextXSamples(1000).SelectMany(x => x.Data);

            var data = d.ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var dcOffset = SpectralAnaylser.FindDCOffset(fftData);

            const double sampleCalculatedOffset = 100.49514649954881;

            Assert.AreEqual(sampleCalculatedOffset, dcOffset, 0.0001);
        }
        
        [TestMethod]
        public void SpectralAnalysis_NegativeDCOffsetTest()
        {
            const int offset = -100;

            var sineGen = new SineWaveGenerator(1000, 100, 52100, offset);

            var d = sineGen.GetNextXSamples(1000).SelectMany(x => x.Data);

            var data = d.ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var dcOffset = SpectralAnaylser.FindDCOffset(fftData);

            const double sampleCalculatedOffset = -99.50485350045119;

            Assert.AreEqual(sampleCalculatedOffset, dcOffset, 0.0001);
        }

        [TestMethod]
        public void SpectralAnalysis_PositiveDCOffsetTest2()
        {
            const int offset = 100;

            var sineGen = new SineWaveGenerator(1000, 100, 52100, offset);
            
            var d = sineGen.GetNextXSamples(1042).SelectMany(x => x.Data);

            var data = d.ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var dcOffset = SpectralAnaylser.FindDCOffset(fftData);

            const double sampleCalculatedOffset = 100.0;

            Assert.AreEqual(sampleCalculatedOffset, dcOffset, 0.0001);
        }

        [TestMethod]
        public void SpectralAnalysis_NegativeDCOffsetTest2()
        {
            const int offset = -100;

            var sineGen = new SineWaveGenerator(1000, 100, 52100, offset);

            var d = sineGen.GetNextXSamples(1042).SelectMany(x => x.Data);

            var data = d.ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var dcOffset = SpectralAnaylser.FindDCOffset(fftData);

            const double sampleCalculatedOffset = -100.0;

            Assert.AreEqual(sampleCalculatedOffset, dcOffset, 0.0001);
        }

        [TestMethod]
        public void SpectralAnalysis_PositiveDCOffsetTestWithPhase()
        {
            const int offset = 100;

            var sineGen = new SineWaveGenerator(1000, 100, 52100, offset, Math.PI * 0.34);

            var d = sineGen.GetNextXSamples(1042).SelectMany(x => x.Data);

            var data = d.ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var dcOffset = SpectralAnaylser.FindDCOffset(fftData);

            const double sampleCalculatedOffset = 100.0;

            Assert.AreEqual(sampleCalculatedOffset, dcOffset, 0.0001);
        }

        [TestMethod]
        public void SpectralAnalysis_NegativeDCOffsetTestWithPhase()
        {
            const int offset = -100;

            var sineGen = new SineWaveGenerator(1000, 100, 52100, offset, Math.PI * 0.34);

            var d = sineGen.GetNextXSamples(1042).SelectMany(x => x.Data);

            var data = d.ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var dcOffset = SpectralAnaylser.FindDCOffset(fftData);

            const double sampleCalculatedOffset = -100.0;

            Assert.AreEqual(sampleCalculatedOffset, dcOffset, 0.0001);
        }

        [TestMethod]
        public void SpectralAnalysis_PositiveDCOffsetTestWithPhase2()
        {
            const int offset = 100;

            var sineGen1 = new SineWaveGenerator(1000, 20, 52100, offset, Math.PI * 0.34);
            var sineGen2 = new SineWaveGenerator(1000, 10, 52100, offset, Math.PI * 0.34);

            var d1 = sineGen1.GetNextXSamples(1042).SelectMany(x => x.Data);
            var d2 = sineGen2.GetNextXSamples(1042).SelectMany(x => x.Data);

            var data = d1.Zip(d2, (i, j) => i + j).ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var dcOffset = SpectralAnaylser.FindDCOffset(fftData);

            const double sampleCalculatedOffset = 200.0;

            Assert.AreEqual(sampleCalculatedOffset, dcOffset, 0.0001);
        }

        [TestMethod]
        public void SpectralAnalysis_NegativeDCOffsetTestWithPhase2()
        {
            const int offset1 = 85;
            const int offset2 = -50;
            
            var sineGen1 = new SineWaveGenerator(1000, 20, 52100, offset1, Math.PI * 0.34);
            var sineGen2 = new SineWaveGenerator(1000, 10, 52100, offset2, Math.PI * 0.34);

            var d1 = sineGen1.GetNextXSamples(1042).SelectMany(x => x.Data);
            var d2 = sineGen2.GetNextXSamples(1042).SelectMany(x => x.Data);

            var data = d1.Zip(d2, (i, j) => i + j).ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var dcOffset = SpectralAnaylser.FindDCOffset(fftData);

            const double sampleCalculatedOffset = offset1 + offset2;

            Assert.AreEqual(sampleCalculatedOffset, dcOffset, 0.0001);
        }

        [TestMethod]
        public void SpectralAnalysis_MultipleDCOffsetTestWithPhase()
        {
            const int offset1 = 85;
            const int offset2 = -50;

            var sineGen1 = new SineWaveGenerator(1000, 20, 52100, offset1, Math.PI * 0.34);
            var sineGen2 = new SineWaveGenerator(1000, 10, 52100, offset2, Math.PI * 0.71);

            var d1 = sineGen1.GetNextXSamples(1000).SelectMany(x => x.Data);
            var d2 = sineGen2.GetNextXSamples(1000).SelectMany(x => x.Data);

            var data = d1.Zip(d2, (i, j) => i + j).ToList();

            var fftData = SpectralAnaylser.FFT(data).ToArray();

            var dcOffset = SpectralAnaylser.FindDCOffset(fftData);

            const double sampleCalculatedOffset = 35.223304466064626;

            Assert.AreEqual(sampleCalculatedOffset, dcOffset, 0.0001);
        }
    }
}