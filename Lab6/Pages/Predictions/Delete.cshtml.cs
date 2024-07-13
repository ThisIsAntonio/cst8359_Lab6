using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab6.Data;
using Lab6.Models;
using Azure.Storage.Blobs;

namespace Lab6.Pages.Predictions
{
    public class DeleteModel : PageModel
    {
        private readonly Lab6.Data.PredictionDataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        public DeleteModel(Lab6.Data.PredictionDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        [BindProperty]
        public Prediction Prediction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Prediction = await _context.Predictions.FindAsync(id);

            if (Prediction == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Prediction = await _context.Predictions.FindAsync(id);

            if (Prediction != null)
            {
                var containerName = Prediction.Question == Question.Earth ? earthContainerName : computerContainerName;
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(Prediction.FileName);

                await blobClient.DeleteIfExistsAsync();

                _context.Predictions.Remove(Prediction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
