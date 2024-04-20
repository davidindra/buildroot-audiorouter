using System.Reflection;

namespace AudioRouterApp.Services
{
    public class StorageManager
    {
        const string IndexFileName = ".index";

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
            return Directory.GetFiles(baseDir).Where(f => !f.Contains(IndexFileName)).Select(f =>
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
            var validDate = DateTime.Now > new DateTime(2000, 1, 1);

            var filename = validDate
                ? $"rec-{GetIndexAndIncrement()}-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.wav"
                : $"rec-{GetIndexAndIncrement()}.wav";

            return Path.Combine(baseDir, filename);
        }

        private string GetIndexAndIncrement()
        {
            var indexFilePath = Path.Combine(baseDir, IndexFileName);

            if (File.Exists(indexFilePath))
            {
                var ix = uint.Parse(File.ReadAllText(indexFilePath));
                File.WriteAllText(indexFilePath, (ix + 1).ToString());
                return ix.ToString().PadLeft(4, '0');
            }
            else
            {
                File.WriteAllText(indexFilePath, 1.ToString());
                return 0.ToString().PadLeft(4, '0');
            }
        }

        public string GetFullPath(string filename)
        {
            return GetFilesystemPath(filename);
        }
    }

    public class RecordingMetadata
    {
        public string FileName { get; set; }

        public DateTime LastModification { get; set; }
        public long SizeBytes { get; set; }
    }
}
