using System.Reflection;

namespace AudioRouterApp.Services
{
    public class StorageManager
    {
        private readonly string baseDir;

        public StorageManager()
        {
            baseDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "recordings");
            
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }
        }

        public ICollection<RecordingMetadata> GetRecordings()
        {
            return Directory.GetFiles(baseDir).Select(f =>
            {
                var modified = File.GetLastWriteTime(f);
                var size = new FileInfo(f).Length;
                return new RecordingMetadata
                {
                    FileName = Path.GetFileName(f),
                    LastModification = modified,
                    SizeBytes = size
                };
            }).ToArray();
        }

        private string GetFilesystemPath(string filename)
        {
            var path = Path.Combine(baseDir, filename);

            if (!path.Contains(baseDir))
                throw new InvalidOperationException("Security check triggered.");

            return path;
        }

        public FileStream GetFileStream(string filename)
        {
            var path = GetFilesystemPath(filename);

            if (!File.Exists(path))
                throw new InvalidOperationException("File does not exist.");
            
            return File.OpenRead(path);
        }

        public void DeleteRecording(string filename)
        {
            var path = GetFilesystemPath(filename);

            if (File.Exists(path))
                File.Delete(path);
        }

        public string GetNewFilePath()
        {
            var filename = $"rec-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.wav";

            return Path.Combine(baseDir, filename);
        }
    }

    public class RecordingMetadata
    {
        public string FileName { get; set; }

        public DateTime LastModification { get; set; }
        public long SizeBytes { get; set; }
    }
}
