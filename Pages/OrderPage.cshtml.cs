using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoonriseMovies.Data;
using MoonriseMovies.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;


namespace MoonriseMovies.Pages
{
    public class OrderPageModel : PageModel
    {
        private MoonriseDBContext db;
        public OrderPageModel(MoonriseDBContext db)
        {
            this.db = db;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Ticket ticket {get; set;}
        public Movie movie {get;set;}
        public Screening screening {get;set;}
        public IdentityUser user {get; set;}
        public async Task OnGetAsync()
        {
            screening = await db.Screenings.FindAsync(Id);
            var movieid = screening.Movie;
            movie = await db.Movies.FindAsync(movieid);

            byte[] bytes;

            // bytes = movie.Image;
            // string imageBase64Data = Convert.ToBase64String(bytes);
            // string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            // ViewData["Image" + movie.Id.ToString()] = imageDataURL;
        }
        
    }
}
