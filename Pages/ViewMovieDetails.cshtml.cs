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

        public ViewMovieDetailsModel(MoonriseDBContext db)
        {
            this.db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Movie SelMovie { get; set; }
        public async Task OnGetAsync()
        {
            SelMovie = await db.Movies.FindAsync(Id);

            byte[] bytes;

            bytes = SelMovie.Image;
            string imageBase64Data = Convert.ToBase64String(bytes);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewData["Image" + SelMovie.Id.ToString()] = imageDataURL;
        }

    }
}
