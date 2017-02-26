using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reactive.Linq;
using AudioAnalysisLibrary.SpectralAnalysis;
using MathNet.Numerics.IntegralTransforms;

namespace AudioAnalysisLibrary
{
    public static class AudioSampleProcessingExtensions
    {
        public static IObservable<int> SampleChannelCount(this IObservable<AudioSampleSet> obs)
        {
            return obs.Select(x => x.DataChannelCount);
        }

        public static IObservable<IList<AudioSampleSet>> CollateSamples(this IObservable<AudioSampleSet> obs, int numSamples)
        {
            return obs.Buffer(numSamples);
        }

        public static IObservable<double[]> GetChannel(this IObservable<IList<AudioSampleSet>> obs, int channel)
        {
            return obs.Select(list => list.Select(x => x.Data[channel]).ToArray());
        }

        public static IObservable<FftResult> AudioSampleFft(this IObservable<double[]> obs, double sampleRate)
        {
            return obs.Select(x =>
            {
                var copy = x.Select(y => new Complex(y, 0)).ToArray();
                Fourier.Forward(copy,FourierOptions.AsymmetricScaling);

                return new FftResult(
                    Fourier.FrequencyScale(x.Length, sampleRate),
                    copy
                    );
            });
        }
    }
}
