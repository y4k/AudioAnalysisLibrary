using AudioAnalysisLibrary.Windowing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AudioAnalysisLibraryTests.Windowing
{
    [TestClass]
    public class WindowServiceTests
    {
        [TestMethod]
        public void CreateFlatTopWindowTest()
        {
            var x = WindowService.CreateFlatTopWindow(10);
        }
    }
}