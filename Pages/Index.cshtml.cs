using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoonriseMovies.Data;
using MoonriseMovies.Models;
namespace MoonriseMovies.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private MoonriseDBContext db;

    public IndexModel(ILogger<IndexModel> logger, MoonriseDBContext db)
    {
        _logger = logger;
        this.db = db;
    }
    public List<Screening> Screenings {get; set;}
    public List<Movie> Movies {get; set;}
    public Movie FeaturedMovie {get; set;}
    public async Task OnGetAsync()
    {       
        byte[] bytes;
        
        Movies = await db.Movies.ToListAsync();
        FeaturedMovie = Movies.ElementAt(new Random().Next(Movies.Count));

        bytes = FeaturedMovie.Image;
        string imageBase64Data = Convert.ToBase64String(bytes);
        string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
        ViewData["Image"+FeaturedMovie.Id.ToString()] = imageDataURL;
    }

   

}
