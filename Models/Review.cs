using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MoonriseMovies.Models
{
    public class Review
    {
        [Key]
        public int Id {get; set;}

        [Required]
        public IdentityUser Client {get; set;}

        [Required]
        public Movie Movie {get; set;}

        [Required]
        public int Rating {get; set;}

        [Required, MinLength(1), MaxLength(4000) ]
        public string Comment {get; set;}

        [Required]
        public DateTime CreatedAt {get; set;}
    }
}