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
    [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
    public List<Screening> Screenings {get; set;}
    public List<Screening> Friday = new List<Screening>();
    public List<Screening> Saturday = new List<Screening>();
    public List<Screening> Sunday = new List<Screening>();
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

        Screenings = await db.Screenings.ToListAsync();
        foreach(var s in Screenings)
        {
            if(s.Start.DayOfWeek.Equals(DayOfWeek.Friday))
            {
                Friday.Add(s);
            }
            else if(s.Start.DayOfWeek.Equals(DayOfWeek.Saturday))
            {
                Saturday.Add(s);
            }
            else if(s.Start.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                Sunday.Add(s);
            }
        }
    }
}
