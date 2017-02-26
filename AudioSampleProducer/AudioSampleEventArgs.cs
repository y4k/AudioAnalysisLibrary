namespace AudioAnalysisLibrary
{
    public delegate void AudioSampleEventHandler(object sender, AudioSampleEventArgs args);

    public class AudioSampleEventArgs
    {
        public AudioSampleSet Data { get; }

        public AudioSampleEventArgs(AudioSampleSet data)
        {
            Data = data;
        }
    }
}