using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AudioAnalysisLibrary
{
    [TestClass]
    public class MultipleSineWaveGeneratorTests
    {
        [TestMethod]
        public void MultipleSineWaveGeneratorTest()
        {
            var actual = new MultipleSineWaveGenerator(new [] {1000.0, 2000.0}, new [] {100.0,100.0}, 52100);
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void GetNextSampleTest()
        {
            var producer = new MultipleSineWaveGenerator(new[] { 1000.0, 2000.0 }, new[] { 100.0, 100.0 }, 52100);

            var actual = producer.GetNextSample();

            var expected = new AudioSampleSet( new [] { 0.0, 0.0 } );

            CollectionAssert.AreEqual(expected.Data, actual.Data);
        }

        [TestMethod]
        public void GetNextXSamplesTest()
        {
            var producer = new MultipleSineWaveGenerator(new[] { 1000.0, 2000.0 }, new[] { 100.0, 100.0 }, 52100);

            var actual = producer.GetNextXSamples(10);

            var expected = new[]
            {
                new AudioSampleSet(new[] { 0.000000, 0.000000}),
                new AudioSampleSet(new[] {12.030644,23.886527}),
                new AudioSampleSet(new[] {23.886527,46.390155}),
                new AudioSampleSet(new[] {35.395425,66.208044}),
                new AudioSampleSet(new[] {46.390155,82.192848}),
                new AudioSampleSet(new[] {56.711003,93.419132}),
                new AudioSampleSet(new[] {66.208044,99.236957}),
                new AudioSampleSet(new[] {74.743322,99.309500}),
                new AudioSampleSet(new[] {82.192848,93.632564}),
                new AudioSampleSet(new[] {88.448408,82.534811})
            };

            Assert.AreEqual(expected.Length, actual.Length);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].DataChannelCount, actual[i].DataChannelCount);
                for (var j = 0; j < expected[i].DataChannelCount; j++)
                {
                    Assert.AreEqual(expected[i].Data[j], actual[i].Data[j], 1e-6,
                        $"Expected {expected[i].Data} and actual {actual[i].Data}");
                }
            }
        }
    }
}