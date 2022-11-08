using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoonriseMovies.Data;
using MoonriseMovies.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;


namespace MoonriseMovies.Pages
{
    public class OrderPageModel : PageModel
    {
        private MoonriseDBContext db;
        public OrderPageModel(MoonriseDBContext db)
        {
            this.db = db;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Ticket ticket {get; set;}
        public Movie movie {get;set;}
        public Screening screening {get;set;}
        public IdentityUser user {get; set;}
        [BindProperty, Required]
        public string PaymentMethod {get; set;}
        public async Task OnGetAsync()
        {
            screening = db.Screenings.Include(s => s.Movie).Where(s => s.Id == Id).FirstOrDefault();
            var movieid = screening.Movie.Id;
            movie = await db.Movies.FindAsync(movieid);

            byte[] bytes;

            bytes = movie.Image;
            string imageBase64Data = Convert.ToBase64String(bytes);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewData["Image" + movie.Id.ToString()] = imageDataURL;
        }
        public async Task OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                RedirectToPage("/Index");
            }else
            {
                var newTicket = new MoonriseMovies.Models.Ticket
                {
                    Client = user,
                    Screening = screening,
                    PurchasedAt = DateTime.Now,
                    PaymentCode = PaymentMethod,

                };
                db.Tickets.Add(newTicket);
                await db.SaveChangesAsync();
            }
        }
        
    }
}
