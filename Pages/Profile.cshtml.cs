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
    public class ProfileModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private SignInManager<IdentityUser> _signInManager;
        private MoonriseDBContext db;

        private readonly UserManager<IdentityUser> _userManager;

        public ProfileModel(MoonriseDBContext db, UserManager<IdentityUser> userManager, ILogger<IndexModel> logger, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            this.db = db;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string CurrentPassword { get; set; }

            [Required]
            [Display(Name = "New Password")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = " The {0} must be at least {2} and at max {1} characters long")]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public IdentityUser user{ get; set; }

        public async Task OnGetAsync()
        {
            var userName = User.Identity.Name; // user's email
            user = await _userManager.FindByNameAsync(userName);
        }   

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userName = User.Identity.Name; // user's email
            user = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);
            await db.SaveChangesAsync();
            return RedirectToPage("/PasswordSuccess"); 
        }
    }
}
