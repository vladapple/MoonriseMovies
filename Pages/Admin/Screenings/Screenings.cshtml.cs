using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoonriseMovies.Models;
using MoonriseMovies.Data;
using Microsoft.AspNetCore.Authorization;

namespace MoonriseMovies.Pages.Admin.Screenings;

[Authorize(Roles="Admin")]
public class ScreeningsModel : PageModel
{
    private readonly MoonriseDBContext db;

    public ScreeningsModel(MoonriseDBContext db)
    {
        this.db = db;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }
    
    public List<Screening> ScreeningList { get; set; } = new List<Screening>();

    public async Task OnGetAsync()
    {
        ScreeningList = await db.Screenings.Include(movie => movie.Movie).ToListAsync();  
    }
}
