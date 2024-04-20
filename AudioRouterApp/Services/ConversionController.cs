using System.Diagnostics;

namespace AudioRouterApp.Services
{
    public class ConversionController
    {
        public ConversionState State { get; set; } = ConversionState.NotStarted;

        public string ConvertedFilename { get; private set; } = null!;

        private Process? process;

        private readonly StorageManager storageManager;

        public ConversionController(StorageManager storageManager)
        {
            this.storageManager = storageManager;
        }

        public void StartConversion(string filename)
        {
            if (State == ConversionState.Converting)
                return;

            State = ConversionState.Converting;
            ConvertedFilename = filename;

            var fullname = storageManager.GetFullPath(filename).Replace(".wav", "");
            var sampleRate = 44100;
            var channels = 2;
            var vbrQuality = 2;

            process = new Process();
            //process.StartInfo.FileName = "C:\\Program Files\\PSPad editor\\pspad.exe";
            //process.StartInfo.Arguments = "";
            process.StartInfo.FileName = "/usr/bin/ffmpeg";
            process.StartInfo.Arguments = $" -i {fullname}.wav -vn -ar {sampleRate} -ac {channels} -q:a {vbrQuality} {fullname}.mp3";
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            process.Start();
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            State = ConversionState.Done;
            ConvertedFilename = null!;
        }

        public void StopConversion()
        {
            if (State != ConversionState.Converting || process == null)
                return;

            process.Exited -= Process_Exited;
            process.Kill();

            State = ConversionState.NotStarted;
            ConvertedFilename = null!;
        }
    }

    public enum ConversionState
    {
        NotStarted,
        Converting,
        Done
    }
}
