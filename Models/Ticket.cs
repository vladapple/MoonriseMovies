using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MoonriseMovies.Models
{
    public class Ticket
    {
        [Key]
        public int Id {get; set;}

        [Required]
        public IdentityUser Client {get; set;}

        [Required]
        public Screening Screening {get; set;}

        [Required]
        public DateTime PurchasedAt {get; set;}

        public string PaymentCode {get; set;}
    }
}