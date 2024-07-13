using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;

namespace Lab6.Pages.Predictions
{
    public class IndexModel : PageModel
    {
        private readonly Lab6.Data.PredictionDataContext _context;

        public IndexModel(Lab6.Data.PredictionDataContext context)
        {
            _context = context;
        }

        public IList<Prediction> Prediction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Prediction = await _context.Predictions.ToListAsync();
        }
    }
}
