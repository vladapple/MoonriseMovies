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
    public class ScreeningAddModel : PageModel
    {
        private readonly MoonriseDBContext db;

        public ScreeningAddModel(MoonriseDBContext db) {
            this.db = db;
        }
        
        [BindProperty, Required, MinLength(1), MaxLength(2) ]
        public string Code {get; set;}

        public Movie movie {get; set;}

        [BindProperty, Required]
        public DateTime Start {get; set;}

        public DateTime End {get; set;}

        [BindProperty, Required, Range(0, 50)]
        public int Price {get; set;}

        public List<Movie> MovieList { get; set; } = new List<Movie>();

        public async Task OnGetAsync()
        {
            MovieList = await db.Movies.ToListAsync();  
        }

        public async Task<IActionResult> OnPostAsync()
        {
            movie = await db.Movies.FindAsync(int.Parse(Request.Form["movieId"]));

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var newScreening = new MoonriseMovies.Models.Screening
            {
                Code = Code,
                Movie = movie,
                Start = Start,
                End = Start.AddMinutes(movie.DurationMin),
                Price = Price
            };
            db.Screenings.Add(newScreening);
            await db.SaveChangesAsync();

            return RedirectToPage("../AddSuccess");
        }
    }
}
