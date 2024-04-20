using System.Diagnostics;

namespace AudioRouterApp.Services
{
    public class StreamingController
    {
        public StreamingState State { get; set; } = StreamingState.NotStarted;

        public DateTime StreamingStart { get; private set; }
        public DateTime StreamingEnd { get; private set; }

        private Process? process;

        public void StartStreaming(string target)
        {
            if (State == StreamingState.Streaming)
                return;

            State = StreamingState.Streaming;
            StreamingStart = DateTime.Now;
            StreamingEnd = DateTime.MinValue;

            process = new Process();
            //process.StartInfo.FileName = "C:\\Program Files\\PSPad editor\\pspad.exe";
            //process.StartInfo.Arguments = "";
            process.StartInfo.FileName = "/usr/bin/ffmpeg";
            process.StartInfo.Arguments = "-f alsa -i plughw:CARD=USB,DEV=0,SUBDEV=0 -acodec mp3 -b:a 320k -f rtp rtp://" + target;
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            process.Start();
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            State = StreamingState.Stopped;
            StreamingEnd = DateTime.Now;
        }

        public void StopStreaming()
        {
            if (State != StreamingState.Streaming || process == null)
                return;

            process.Exited -= Process_Exited;
            process.Kill();

            State = StreamingState.Stopped;
            StreamingEnd = DateTime.Now;
        }
    }

    public enum StreamingState
    {
        NotStarted,
        Streaming,
        Stopped
    }

    public static class StreamingStateExtensions
    {
        public static string GetString(this StreamingState state)
        {
            return state switch
            {
                StreamingState.NotStarted => "Nevysílá se",
                StreamingState.Streaming => "Vysílání...",
                StreamingState.Stopped => "Vysílání ukončeno.",
                _ => "N/A"
            };
        }
    }
}
