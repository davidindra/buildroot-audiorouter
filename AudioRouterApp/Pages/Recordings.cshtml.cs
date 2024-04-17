using AudioRouterApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AudioRouterApp.Pages
{
    public class RecordingsModel : PageModel
    {
        private readonly StorageManager storageManager;

        public RecordingsModel(StorageManager storageManager)
        {
            this.storageManager = storageManager;
        }

        [BindProperty]
        public string RecordingToRemoveFileName { get; set; }

        public void OnGet()
        {
        }

        public ActionResult OnGetDownload(string filename)
        {
            return File(storageManager.GetFileStream(filename), "application/octet-stream", filename);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(RecordingToRemoveFileName))
            {
                return Page();
            }

            Console.WriteLine(RecordingToRemoveFileName);

            storageManager.DeleteRecording(RecordingToRemoveFileName);
            
            return RedirectToPage();
        }
    }
}
