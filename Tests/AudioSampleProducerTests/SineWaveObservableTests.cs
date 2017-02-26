using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AudioAnalysisLibrary
{
    [TestClass]
    public class SineWaveObservableTests
    {
        [TestMethod]
        public void SineWaveObservableTest()
        {
            var swObs = new SineWaveObservable(1000, 100, 40000);
            Assert.IsNotNull(swObs);
        }

        [TestMethod]
        public void InitialiseTest()
        {
            var swObs = new SineWaveObservable(1000, 100, 40000);
            var actual = swObs.Initialise();
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void StartTest()
        {
            var swObs = new SineWaveObservable(1000, 100, 40000);
            if (swObs.Initialise())
            {
                var actual = swObs.Start();
                Assert.AreEqual(true, actual);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}