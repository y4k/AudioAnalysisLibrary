using System;
using System.Linq;
using System.Numerics;
using AudioAnalysisLibrary.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AudioAnalysisLibraryTests.Tools
{
    [TestClass]
    public class RmsCalculatorTests
    {
        private readonly Random _random = new Random();

        [TestMethod]
        public void CalculateComplexRmsTest()
        {
            var d = Enumerable.Range(0, 100).Select(x => new Complex(_random.Next(100), _random.Next(100)));

            var data = d.ToList();

            var actual = data.CalculateRms();

            var expected = Math.Sqrt(data.Select(x => x.Magnitude * x.Magnitude).Sum() / data.Count);

            Assert.AreEqual(expected, actual, 1e-8);
        }
    }
}