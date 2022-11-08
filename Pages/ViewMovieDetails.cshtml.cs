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
    public class ViewMovieDetailsModel : PageModel
    {
        private SignInManager<IdentityUser> signInManager;
        private MoonriseDBContext db;

        public ViewMovieDetailsModel(MoonriseDBContext db, SignInManager<IdentityUser> signInManager)
        {
            this.db = db;
            this.signInManager = signInManager;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Movie SelMovie { get; set; }
        public List<Screening> Screenings { get; set; }
        public List<Screening> Friday = new List<Screening>();
        public List<Screening> Saturday = new List<Screening>();
        public List<Screening> Sunday = new List<Screening>();
        public async Task OnGetAsync()
        {

            SelMovie = await db.Movies.FindAsync(Id);
            Screenings = await db.Screenings.Where(x => x.Movie.Id == Id).ToListAsync();

            byte[] bytes;

            bytes = SelMovie.Image;
            string imageBase64Data = Convert.ToBase64String(bytes);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewData["Image" + SelMovie.Id.ToString()] = imageDataURL;

            foreach (var s in Screenings)
            {
                if (s.Start.DayOfWeek.Equals(DayOfWeek.Friday))
                {
                    Friday.Add(s);
                }
                else if (s.Start.DayOfWeek.Equals(DayOfWeek.Saturday))
                {
                    Saturday.Add(s);
                }
                else if (s.Start.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    Sunday.Add(s);
                }
            }
        }

    }
}
