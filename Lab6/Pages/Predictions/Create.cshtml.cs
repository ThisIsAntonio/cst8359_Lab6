using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab6.Data;
using Lab6.Models;
using Azure.Storage.Blobs;

namespace Lab6.Pages.Predictions
{
    public class CreateModel : PageModel
    {
        private readonly Lab6.Data.PredictionDataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        public CreateModel(Lab6.Data.PredictionDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Prediction Prediction { get; set; } = default!;

        [BindProperty]
        public IFormFile File { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (File != null)
            {
                var containerName = Prediction.Question == Question.Earth ? earthContainerName : computerContainerName;
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(File.FileName);

                using (var stream = File.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                Prediction.Url = blobClient.Uri.ToString();
            }

            _context.Predictions.Add(Prediction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
