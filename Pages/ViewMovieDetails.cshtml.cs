using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoonriseMovies.Data;
using MoonriseMovies.Models;

namespace MoonriseMovies.Pages
{
    public class ViewMovieDetailsModel : PageModel
    {
        private MoonriseDBContext db;
        public MoviesModel(MoonriseDBContext db)
    {
        this.db = db;
    }

        [BindProperty(SupportsGet = true)] 
        public int Id { get; set; }

        public Movie movie {get; set;}
        public async Task OnGetAsync() => movie = await db.Movies.FindAsync(Id);

    }
}
