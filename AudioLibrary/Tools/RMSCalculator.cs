using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MathNet.Numerics.Statistics;

namespace AudioAnalysisLibrary.Tools
{
	public static class RmsCalculator
    {
        public static double CalculateRms(this IEnumerable<Complex> data)
        {
            return data.Select(x => x.Magnitude).RootMeanSquare();
        }
    }
}
