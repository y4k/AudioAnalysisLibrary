using System;
using System.Reactive.Linq;
using AudioAnalysisLibrary.Interfaces;

namespace AudioAnalysisLibrary
{
    public class AudioSampleStreamProcessor
    {
        private IAudioSampleGenerator Generator { get; }
        private IObservable<AudioSampleSet> AudioObservable { get; }

        private AudioSampleStreamProcessor(IAudioSampleGenerator generator)
        {
            Generator = generator;
            AudioObservable = CreateObservable(Generator);

            AudioObservable.SampleChannelCount().Subscribe(Console.WriteLine);
        }

        public static AudioSampleStreamProcessor FromAudioGenerator(IAudioSampleGenerator generator)
        {
            return new AudioSampleStreamProcessor(generator);
        }

        private IObservable<AudioSampleSet> CreateObservable(IAudioSampleGenerator generator)
        {
            var audioObservable = Observable.FromEvent<AudioSampleEventHandler, AudioSampleEventArgs>(handler =>
            {
                AudioSampleEventHandler audiohandler = (sender, e) =>
                {
                    handler?.Invoke(e);
                };

                return audiohandler;
            },
            audiohandler => generator.NewSampleEvent += audiohandler,
            audiohandler => generator.NewSampleEvent -= audiohandler).Select(x => x.Data);

            return audioObservable;
        }
    }
}
