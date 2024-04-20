using AudioRouterApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AudioRouterApp.Pages
{
    public class RecordingsModel : PageModel
    {
        private readonly StorageManager storageManager;
        private readonly ConversionController conversionController;

        public RecordingsModel(StorageManager storageManager, ConversionController conversionController)
        {
            this.storageManager = storageManager;
            this.conversionController = conversionController;
        }

        [BindProperty]
        public string RecordingToRemoveFileName { get; set; }

        public void OnGet()
        {
        }

        public ActionResult OnGetConvert(string filename)
        {
            conversionController.StartConversion(filename);

            return RedirectToPage();
        }

        public ActionResult OnGetStopConvert()
        {
            conversionController.StopConversion();

            return RedirectToPage();
        }

        public ActionResult OnGetPlay(string filename)
        {
            var result = PhysicalFile(storageManager.GetFullPath(filename), GetMimeType(filename));
            result.EnableRangeProcessing = true;
            return result;
        }

        public ActionResult OnGetDownload(string filename)
        {
            return PhysicalFile(storageManager.GetFullPath(filename), "application/octet-stream", filename);
        }

        public IActionResult OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(RecordingToRemoveFileName))
            {
                return Page();
            }

            Console.WriteLine(RecordingToRemoveFileName);

            storageManager.DeleteRecording(RecordingToRemoveFileName);
            
            return RedirectToPage();
        }

        private static string GetMimeType(string filename)
        {
            return filename.Contains(".wav") ? "audio/wav" : "audio/mpeg";
        }
    }
}
