using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MoonriseMovies.Models
{
    public class Screening
    {
        [Key]
        public int Id {get; set;}

        [Required, MinLength(1), MaxLength(2) ]
        public string Code {get; set;}

        [Required]
        public Movie Movie {get; set;}

        [Required]
        public DateTime Start {get; set;}

        [Required]
        public DateTime End {get; set;}

        [Required]
        public int Price {get; set;}
    }
}