using MathNet.Numerics;

namespace AudioAnalysisLibrary.Windowing
{
    public class WindowService
    {
        public static double[] CreateFlatTopWindow(int width)
        {
            return Window.FlatTop(width);
        }
    }
}
