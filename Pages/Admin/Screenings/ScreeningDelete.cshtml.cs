using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoonriseMovies.Models;
using MoonriseMovies.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace MoonriseMovies.Pages.Admin.Screenings
{
    [Authorize(Roles="Admin")]
    public class ScreeningDeleteModel : PageModel
    {
        private MoonriseDBContext db;
        public ScreeningDeleteModel(MoonriseDBContext db) => this.db = db;

        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }

        public Screening Screening { get; set; }

        public async Task OnGetAsync()
        {
            Screening = await db.Screenings.FindAsync(Id);
        }  

        public async Task<IActionResult> OnPostAsync()
        {
            Screening = await db.Screenings.FindAsync(Id); 
            if(Screening != null)
            {
                db.Screenings.Remove(Screening);
                db.SaveChanges();
            }
            return RedirectToPage("../../Admin/Screenings/Screenings");
        }
    }
}
