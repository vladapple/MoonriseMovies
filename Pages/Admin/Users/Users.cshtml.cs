using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoonriseMovies.Models;
using MoonriseMovies.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MoonriseMovies.Pages.Admin.Users;

[Authorize(Roles="Admin")]
public class UsersModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly MoonriseDBContext db;

    public UsersModel(MoonriseDBContext db)
    {
        this.db = db;
    }

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    
    public List<IdentityUser> userList { get; set; } = new List<IdentityUser>();

    public List<IdentityRole> roleList { get; set; } = new List<IdentityRole>();

    public List<IdentityUserRole<string>> userRoleList { get; set; } = new List<IdentityUserRole<string>>();

    public List<string> combinedList { get; set; } = new List<string>();

    public async Task OnGetAsync()
    {
        userList = await db.Users.ToListAsync();
        userRoleList = await db.UserRoles.ToListAsync(); 
        roleList = await db.Roles.ToListAsync();                    
    }
}
