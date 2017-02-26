using System;
using System.Reactive.Linq;
using MathNet.Filtering.DataSources;

namespace AudioAnalysisLibrary
{
    public class SineWaveObservable
    {
        public IObservable<AudioSampleSet> SampleStream { get; private set; }

        private SinusoidalSource _source;
        private bool _initialised;
        private bool _started;
        private readonly double _sampleRate;
        private readonly double _frequency;
        private readonly double _amplitude;

        public SineWaveObservable(double frequency, double amplitude, double sampleRate)
        {
            _frequency = frequency;
            _amplitude = amplitude;
            _sampleRate = sampleRate;
        }

        public bool Start()
        {
            SampleStream = Observable.Generate(
                0,
                i => true,
                i => i + 1,
                i => new AudioSampleSet(new []{_source.ReadNextSample()}),
                i => TimeSpan.FromMilliseconds(1000 / _sampleRate)
                );
            _started = true;
            return _started;
        }

        public void Stop()
        {
            
        }

        public bool Initialise()
        {
            _source = new SinusoidalSource(_sampleRate, _frequency, _amplitude);

            SampleStream = Observable.Empty<AudioSampleSet>();
            
            _initialised = true;
            return _initialised;
        }
    }
}
