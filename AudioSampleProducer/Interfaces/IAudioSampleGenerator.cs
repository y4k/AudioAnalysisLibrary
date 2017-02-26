namespace AudioAnalysisLibrary.Interfaces
{
    public interface IAudioSampleGenerator
    {
        event AudioSampleEventHandler NewSampleEvent;

        AudioSampleSet[] GetNextXSamples(int x);

        AudioSampleSet GetNextSample();
    }
}
