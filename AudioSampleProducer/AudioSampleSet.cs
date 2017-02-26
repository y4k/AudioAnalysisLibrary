using System;
using System.Linq;

namespace AudioAnalysisLibrary
{
    public class AudioSampleSet
    {
        public double[] Data { get; }
        public int DataChannelCount => Data.Length;

        public AudioSampleSet(double[] data)
        {
            Data = data;
        }

        public override bool Equals(object obj)
        {
            var item = obj as AudioSampleSet;

            return Equals(item);
        }

        public override int GetHashCode()
        {
            return Data?.GetHashCode() ?? 0;
        }

        public bool Equals(AudioSampleSet obj)
        {
            return obj != null && obj.Data.Zip(Data, (i, j) => Math.Abs(i - j) < 1e-6).All(x => x);
        }
    }
}