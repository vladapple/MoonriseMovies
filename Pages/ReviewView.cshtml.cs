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

        public Screening screening {get; set;}

        [BindProperty, Required, Range(0, 5)]
        public int Rating {get; set;}

        [BindProperty, Required, MinLength(1), MaxLength(4000) ]
        public string Comment {get; set;}

        public byte[] Image { get; set; }

        public DateTime CreatedAt {get; set;}

        public async Task OnGetAsync()
        {
        
        }   
    }
}
