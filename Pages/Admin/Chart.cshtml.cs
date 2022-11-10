using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoonriseMovies.Models;
using MoonriseMovies.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MoonriseMovies.Pages.Admin
{
    public class ChartModel : PageModel
    {
        private readonly MoonriseDBContext db;

        public ChartModel(MoonriseDBContext db)
        {
            this.db = db;
        }
        public List<Movie> movieList { get; set; } = new List<Movie>();
        public async Task OnGetAsync()
        {
            movieList = await db.Movies.ToListAsync();  
        }
    }
}
