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
using Microsoft.AspNetCore.Identity;

namespace MoonriseMovies.Pages
{
    [Authorize]
    public class ReviewModel : PageModel
    {
        private readonly MoonriseDBContext db;
        private readonly ILogger<IndexModel> _logger;
        private SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewModel(MoonriseDBContext db, UserManager<IdentityUser> userManager, ILogger<IndexModel> logger, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            this.db = db;
            _signInManager = signInManager;
        }
        
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Movie movie {get; set;}

        public Screening screening {get; set;}

        [BindProperty, Required, Range(0, 5)]
        public int Rating {get; set;}

        [BindProperty, Required, MinLength(1), MaxLength(4000) ]
        public string Comment {get; set;}

        public byte[] Image { get; set; }

        public DateTime CreatedAt {get; set;}

        public Review review { get; set; }

        public List<Review> reviewList { get; set; }

        public async Task OnGetAsync()
        {
            
            screening = db.Screenings.Include(s => s.Movie).Where(s => s.Id == Id).FirstOrDefault();
            int movieId = screening.Movie.Id;
            movie = await db.Movies.FindAsync(movieId);
            byte[] bytes;
            bytes = movie.Image;  
            string imageBase64Data = Convert.ToBase64String(bytes);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewData["Image"+movie.Id.ToString()] = imageDataURL;
        }   
        public async Task<IActionResult> OnPostAsync()
        {
            screening = db.Screenings.Include(s => s.Movie).Where(s => s.Id == Id).FirstOrDefault();
            int movieId = screening.Movie.Id;
            movie = await db.Movies.FindAsync(movieId);
               
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userName = User.Identity.Name; // user's email
            var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault(); //user in database
            var newReview = new MoonriseMovies.Models.Review
            {
                Client = user,
                Movie = movie,
                Rating = Rating,
                Comment = Comment,
                CreatedAt = DateTime.Now
            };
            db.Reviews.Add(newReview);
            await db.SaveChangesAsync();

            reviewList = await db.Reviews.Include(r => r.Movie).Where(r => r.Movie.Id == movieId).ToListAsync();
            int sum = 0;
            int i = 0;
            foreach(var r in reviewList)
            {
                sum += r.Rating;
                i++;
            }

            decimal average = Convert.ToDecimal(sum)/i;
            movie.RatingAvg = average;
            db.SaveChanges();

            return RedirectToPage("/AddReviewSuccess");
        }
    }
}
