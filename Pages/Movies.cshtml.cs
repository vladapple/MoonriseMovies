using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoonriseMovies.Data;
using MoonriseMovies.Models;

namespace MoonriseMovies.Pages
{
    public class MoviesModel : PageModel
    {
        private MoonriseDBContext db;
        public MoviesModel(MoonriseDBContext db)
        {
            this.db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<Movie> MovieList { get; set; }
        public async Task OnGetAsync()
        {
            MovieList = await db.Movies.ToListAsync();
            byte[] bytes;
            foreach (var movie in MovieList)
            {
                bytes = movie.Image;
                string imageBase64Data = Convert.ToBase64String(bytes);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                ViewData["Image" + movie.Id.ToString()] = imageDataURL;
            }
        }
    }
}
