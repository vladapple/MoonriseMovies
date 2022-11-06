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

namespace MoonriseMovies.Pages.Admin.Movies
{
    [Authorize(Roles="Admin")]
    public class MovieDeleteModel : PageModel
    {
        private MoonriseDBContext db;
        public MovieDeleteModel(MoonriseDBContext db) => this.db = db;

        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }

        public Movie Movie { get; set; }

        public async Task OnGetAsync()
        {
            Movie = await db.Movies.FindAsync(Id);
        }  

        public async Task<IActionResult> OnPostAsync()
        {
            Movie = await db.Movies.FindAsync(Id); 
            if(Movie != null)
            {
                db.Movies.Remove(Movie);
                db.SaveChanges();
            }
            return RedirectToPage("../../Admin/Movies/Movies");
            
        }
    }
}
