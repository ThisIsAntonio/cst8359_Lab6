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
using System.ComponentModel.DataAnnotations;

namespace Lab6.Pages.Predictions
{
    public class CreateModel : PageModel
    {
        private readonly PredictionDataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        public CreateModel(PredictionDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnGet()
        {
            QuestionOptions = new SelectList(Enum.GetValues(typeof(Question)).Cast<Question>());
            return Page();
        }

        [BindProperty]
        public Prediction Prediction { get; set; } = default!;

        [BindProperty]
        [Display(Name = "Upload File")]
        public IFormFile UploadFile { get; set; }

        public SelectList QuestionOptions { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadFile != null)
            {
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(Prediction.Question == Question.Earth ? earthContainerName : computerContainerName);
                await blobContainerClient.CreateIfNotExistsAsync();

                var blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid().ToString() + "-" + UploadFile.FileName);

                using (var stream = UploadFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
                Prediction.Url = blobClient.Uri.ToString();
            }

            // Removing URL validation error manually if URL is set
            if (!ModelState.IsValid)
            {
                QuestionOptions = new SelectList(Enum.GetValues(typeof(Question)).Cast<Question>());
                return Page();
            }

            _context.Predictions.Add(Prediction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
