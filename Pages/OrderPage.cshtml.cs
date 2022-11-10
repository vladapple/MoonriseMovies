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
        private readonly string _gmailUser;
        private readonly string _gmailPassword;
         public OrderPageModel(MoonriseDBContext db, IConfiguration configuration)
        {
            this.db = db;
            _gmailUser = configuration.GetValue<string>("GmailUser");
            _gmailPassword = configuration.GetValue<string>("GmailPassword");
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
            screening = db.Screenings.Include(s => s.Movie).Where(s => s.Id == Id).FirstOrDefault();
            var movieid = screening.Movie.Id;
            movie = await db.Movies.FindAsync(movieid);

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
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(_gmailUser),
                    Subject = "Order Confirmation",
                    Body = $@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                            <meta charset=""utf-8"" />
                            <title></title>
                            <body>
                            <h4 style='color:orrange'>Thank you for your purchase!</h4><br> 
                            <h5>Here are details or your ticket order:</h5>
                            <p>Purchase date: {DateTime.Now}<br>
                            Movie Title: {movie.Title}<br>
                            Screening: {screening.Code}<br>
                            Screening Date: {screening.Start}<br>
                            Price: {screening.Price}.00$<br>
                            Payment Code: {PaymentMethod}<br><br>
                            Enjoy the movie!<br><br>
                            Kind regards,<br>
                            MoonriseMovie</p>
                            </body></html>",
                    IsBodyHtml = true
                };
                mail.To.Add(userName);
                var client = new SmtpClient("smtp.gmail.com")
                {
                    UseDefaultCredentials = false,
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential(
                        _gmailUser, _gmailPassword),
                        EnableSsl = true
                };
            
                client.Send(mail);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: {1}", e.ToString(), e.Message);
            }
            //end email

            return RedirectToPage("/OrderSuccess");
        } 
    }
}
