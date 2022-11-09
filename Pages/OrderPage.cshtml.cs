using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoonriseMovies.Data;
using MoonriseMovies.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;


namespace MoonriseMovies.Pages
{
    [Authorize]
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

        [BindProperty]
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
        public async Task<IActionResult> OnPostAsync()
        {
            screening = await db.Screenings.FindAsync(Id);

            if(!ModelState.IsValid)
            {
                return Page();
            }
            var userName = User.Identity.Name; // user's email
            var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault(); //user in database
            var newTicket = new MoonriseMovies.Models.Ticket
            {
                Client = user,
                Screening = screening,
                PurchasedAt = DateTime.Now,
                PaymentCode = PaymentMethod,
            };
            db.Tickets.Add(newTicket);
            await db.SaveChangesAsync();

            //email
            var body = $@"<p>Thank you, here are details or your ticket order:<br><br>
                            Purchase date: {DateTime.Now}<br>
                            Screening: {screening}<br>
                            Price: {screening.Price}.00$<br>
                            Date and time: {screening.Start}<br>
                            Payment Code: {PaymentMethod}<br><br>
                            Best movies,<br>
                            MoonriseMovie!";
            using(SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential credential = new NetworkCredential
                {
                    UserName = "moonrisemoviesfsd04@gmail.com",  
                    Password = "fjrknrbhcwbnyzgs"  
                };
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = credential;
                smtp.Port = 587;
                
                var message = new MailMessage();
                message.To.Add(userName);
                message.Subject = "Order";
                message.Body = body;
                message.IsBodyHtml = false;
                message.From = new MailAddress("moonrisemovies@gmail.com");
                await smtp.SendMailAsync(message);
            }
            //end email
            return RedirectToPage("/OrderSuccess");
        } 
    }
}
