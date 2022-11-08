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
    public class ReviewViewModel : PageModel
    {
        private readonly MoonriseDBContext db;
        private readonly ILogger<IndexModel> _logger;
        private SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewViewModel(MoonriseDBContext db, UserManager<IdentityUser> userManager, ILogger<IndexModel> logger, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            this.db = db;
            _signInManager = signInManager;
        }
        
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Movie movie {get; set;}

        public Review review { get; set; }

        public List<Review> reviewList { get; set; }

        public byte[] Image { get; set; }

        public async Task OnGetAsync()
        {
            reviewList = await db.Reviews.Include(u => u.Client).Include(r => r.Movie).Where(r => r.Movie.Id == Id).ToListAsync();
            movie = await db.Movies.FindAsync(Id);
            byte[] bytes;
            bytes = movie.Image;  
            string imageBase64Data = Convert.ToBase64String(bytes);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewData["Image"+movie.Id.ToString()] = imageDataURL;
        }   
    }
}
