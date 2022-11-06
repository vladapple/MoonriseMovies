using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoonriseMovies.Models;
using MoonriseMovies.Data;
using Microsoft.AspNetCore.Authorization;

namespace MoonriseMovies.Pages.Admin.Movies;

[Authorize(Roles="Admin")]
public class MoviesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly MoonriseDBContext db;

    public MoviesModel(MoonriseDBContext db)
    {
        this.db = db;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }
    
    public List<Movie> MovieList { get; set; } = new List<Movie>();

    public async Task OnGetAsync()
    {
        MovieList = await db.Movies.ToListAsync();  
        byte[] bytes;
        foreach (var movie in MovieList)
        {
            bytes = movie.Image;  
            string imageBase64Data = Convert.ToBase64String(bytes);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewData["Image"+movie.Id.ToString()] = imageDataURL;
        } 
    }
}
