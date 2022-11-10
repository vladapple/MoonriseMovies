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

namespace MoonriseMovies.Pages.Admin.Users
{
    [Authorize(Roles="Admin")]
    public class UserEditModel : PageModel
    {
        private MoonriseDBContext db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserEditModel(MoonriseDBContext db, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            this.db = db;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleId { get; set; }
    
        public IdentityUser user{ get; set; }

        public List<IdentityRole> roleList { get; set; } = new List<IdentityRole>();

        public IdentityUserRole<string> userRole { get; set; }

        public IdentityRole role { get; set; }

        public async Task OnGetAsync()
        {
            user = await db.Users.FindAsync(Id);
            userRole = await db.UserRoles.Where(user => user.UserId == Id).FirstOrDefaultAsync();
            role = await db.Roles.Where(role => role.Id == userRole.RoleId).FirstOrDefaultAsync();
            roleList = await db.Roles.ToListAsync();
        }   

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            user = await db.Users.FindAsync(Id);
            userRole = await db.UserRoles.Where(user => user.UserId == Id).FirstOrDefaultAsync();
            role = await db.Roles.Where(role => role.Id == userRole.RoleId).FirstOrDefaultAsync();
            string oldRoleName = role.Name;
            
            role = await db.Roles.FindAsync(Request.Form["roleId"]);
            string newRoleName = role.Name;
            
            if(oldRoleName != newRoleName)
            {
                await _userManager.RemoveFromRoleAsync(user, oldRoleName);
                await _userManager.AddToRoleAsync(user, newRoleName);
                await db.SaveChangesAsync();
            }
            return RedirectToPage("../../Admin/Users/Users"); 
        }
    }
}
