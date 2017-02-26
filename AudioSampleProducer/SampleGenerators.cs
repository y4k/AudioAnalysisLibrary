using System.Collections.Generic;
using System.Linq;
using AudioAnalysisLibrary.Interfaces;
using MathNet.Filtering.DataSources;

namespace AudioAnalysisLibrary
{
    public class MultipleSineWaveGenerator : IAudioSampleGenerator
    {
        public readonly int Channels;

        public double[] Frequencies { get; }
        public double[] Amplitudes { get; }
        public double SampleRate { get; }

        private readonly List<SinusoidalSource> _sources = new List<SinusoidalSource>();

        public event AudioSampleEventHandler NewSampleEvent;

        public MultipleSineWaveGenerator(double[] frequencies, double[] amplitudes, double sampleRate)
        {
            Frequencies = frequencies;
            Amplitudes = amplitudes;
            SampleRate = sampleRate;
            Channels = Frequencies.Length;
            
            for (var i = 0; i < Channels; i++)
            {
                _sources.Add(new SinusoidalSource(sampleRate,frequencies[i], amplitudes[i]));
            }
        }
        
        protected virtual void OnSampleEvent(AudioSampleEventArgs args)
        {
            NewSampleEvent?.Invoke(this, args);
        }

        public AudioSampleSet[] GetNextXSamples(int x)
        {
            var samples = new List<AudioSampleSet>();
            for (var i = 0; i < x; i++)
            {
                samples.Add(GetNextSample());
            }

            return samples.ToArray();
        }

        public AudioSampleSet GetNextSample()
        {
            return new AudioSampleSet(_sources.Select(x => x.ReadNextSample()).ToArray());
        }
    }

    public class SineWaveGenerator : IAudioSampleGenerator
    {
        private readonly SinusoidalSource _source;

        public event AudioSampleEventHandler NewSampleEvent;

        public SineWaveGenerator(double frequency, double amplitude, double sampleRate, double offset = 0, double phase = 0)
        {
            _source = new SinusoidalSource(sampleRate, frequency, amplitude, phase, offset, 0);
        }

        public AudioSampleSet[] GetNextXSamples(int x)
        {
            var samples = new List<AudioSampleSet>();
            for (var i = 0; i < x; i++)
            {
                samples.Add(GetNextSample());
            }

            return samples.ToArray();
        }

        public AudioSampleSet GetNextSample()
        {
            return new AudioSampleSet(new [] {_source.ReadNextSample() });
        }

        protected virtual void OnNewSampleEvent(AudioSampleEventArgs args)
        {
            NewSampleEvent?.Invoke(this, args);
        }
    }
}
