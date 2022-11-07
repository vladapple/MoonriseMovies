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

namespace MOONRISEMOVIES.Pages
{
    public class ProfileModel : PageModel
    {/*
        private MoonriseDBContext db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserEditModel(MoonriseDBContext db, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            this.db = db;
        }
        public IdentityUser user{ get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            user = await db.Users.FindAsync(Id);
            user = await _userManager.ChangePasswordAsync(user, "oldpassword", "newpassword");
        }*/

        public void OnGet()
        {
        }
    }
}
