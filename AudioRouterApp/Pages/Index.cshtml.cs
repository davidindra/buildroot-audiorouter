using AudioRouterApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AudioRouterApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RecordingController recordingController;
        private readonly StreamingController streamingController;
        private readonly TimeManager timeManager;

        public IndexModel(ILogger<IndexModel> logger, RecordingController recordingController, StreamingController streamingController, TimeManager timeManager)
        {
            _logger = logger;
            this.recordingController = recordingController;
            this.streamingController = streamingController;
            this.timeManager = timeManager;
        }

        public void OnGet()
        {

        }

        public ActionResult OnGetSynchronizeDateTime(long timestamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timestamp / 1000).ToLocalTime();
            
            timeManager.SetTime(dateTime);

            return RedirectToPage();
        }

        public ActionResult OnGetStartRecording()
        {
            recordingController.StartRecording();

            return RedirectToPage();
        }

        public ActionResult OnGetStopRecording()
        {
            recordingController.StopRecording();

            return RedirectToPage();
        }

        public ActionResult OnGetStartStreaming(string target)
        {
            streamingController.StartStreaming(target);

            return RedirectToPage();
        }

        public ActionResult OnGetStopStreaming()
        {
            streamingController.StopStreaming();

            return RedirectToPage();
        }
    }
}
