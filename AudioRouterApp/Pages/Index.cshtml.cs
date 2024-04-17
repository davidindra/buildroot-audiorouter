using AudioRouterApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AudioRouterApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RecordingController recordingController;

        public IndexModel(ILogger<IndexModel> logger, RecordingController recordingController)
        {
            _logger = logger;
            this.recordingController = recordingController;
        }

        public void OnGet()
        {

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
    }
}
