using System.Diagnostics;

namespace AudioRouterApp.Services
{
    public class TimeManager
    {
        public void SetTime(DateTime dt)
        {
            var str = $"{dt.Year}-{dt.Month.ToString().PadLeft(2, '0')}-{dt.Day.ToString().PadLeft(2, '0')} {dt.Hour.ToString().PadLeft(2, '0')}:{dt.Minute.ToString().PadLeft(2, '0')}:{dt.Second.ToString().PadLeft(2, '0')}";

            var program = "/bin/date";
            var args = $"-s '{str}'";

            Console.WriteLine($"{program} {args}");

            var process = new Process();
            //process.StartInfo.FileName = "C:\\Program Files\\PSPad editor\\pspad.exe";
            //process.StartInfo.Arguments = "";
            process.StartInfo.FileName = "/bin/sh";
            process.StartInfo.Arguments = $"-c \"{program} {args}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            _ = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
    }
}
