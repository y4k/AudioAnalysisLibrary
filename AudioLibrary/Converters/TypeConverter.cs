using System.Collections.Generic;
using System.Linq;

namespace AudioAnalysisLibrary.Converters
{
    public static class TypeConverter
    {
        public static IEnumerable<double> ToDouble(this IEnumerable<float> samples)
        {
            return samples.Select(x => (double) x);
        }
    }
}
