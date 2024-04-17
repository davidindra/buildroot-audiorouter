using System.Diagnostics;

namespace AudioRouterApp.Services
{
    public class RecordingController
    {
        public RecordingState State { get; set; } = RecordingState.NotStarted;

        public DateTime RecordingStart { get; private set; }
        public DateTime RecordingEnd { get; private set; }

        private Process process;

        private readonly StorageManager storageManager;

        public RecordingController(StorageManager storageManager)
        {
            this.storageManager = storageManager;
        }

        public void StartRecording()
        {
            if (State == RecordingState.Recording)
                return;

            State = RecordingState.Recording;
            RecordingStart = DateTime.Now;
            RecordingEnd = DateTime.MinValue;

            // sh -c 'cat testfile > testfile2'

            var filename = storageManager.GetNewFilePath();

            process = new Process();
            //process.StartInfo.FileName = "C:\\Program Files\\PSPad editor\\pspad.exe";
            //process.StartInfo.Arguments = "";
            process.StartInfo.FileName = "/usr/bin/arecord";
            process.StartInfo.Arguments = " -D plughw:CARD=USB,DEV=0,SUBDEV=0 -f S32_LE -r 48000 -c 2 " + filename;
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            process.Start();
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            State = RecordingState.Stopped;
            RecordingEnd = DateTime.Now;
        }

        public void StopRecording()
        {
            if (State != RecordingState.Recording)
                return;

            process.Exited -= Process_Exited;
            process.Kill();

            State = RecordingState.Stopped;
            RecordingEnd = DateTime.Now;
        }
    }

    public enum RecordingState
    {
        NotStarted,
        Recording,
        Stopped
    }

    public static class RecordingStateExtensions
    {
        public static string GetString(this RecordingState state)
        {
            return state switch
            {
                RecordingState.NotStarted => "Nenahrává se",
                RecordingState.Recording => "Nahrávání...",
                RecordingState.Stopped => "Nahrávání ukončeno.",
                _ => "N/A"
            };
        }
    }
}
