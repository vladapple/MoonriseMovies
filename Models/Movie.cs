using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MoonriseMovies.Models
{
    public class Movie
    {
        [Key]
        public int Id {get; set;}

        [Required, MinLength(1), MaxLength(250) ]
        public string Title {get; set;}

        [Required, MinLength(1), MaxLength(4000) ]
        public string Description {get; set;}

        [Required, MinLength(1), MaxLength(30) ]
        public string Genre {get; set;}

        [Required]
        public int DurationMin {get; set;}

        [Required]
        public byte[] Image { get; set; }

        [Required, MinLength(1), MaxLength(400), Url]
        public string TrailerURL {get; set;}
        
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal RatingAvg {get; set;}
    }
}