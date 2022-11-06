using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoonriseMovies.Models;
using MoonriseMovies.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;


namespace MoonriseMovies.Pages.Admin.Movies
{
    [Authorize(Roles="Admin")]
    public class MovieAddModel : PageModel
    {
        private readonly MoonriseDBContext db;

        public MovieAddModel(MoonriseDBContext db) {
            this.db = db;
        }
        
        [BindProperty, Required, MinLength(1), MaxLength(250)]
        public string Title { get; set; }

        [BindProperty, Required, MinLength(1), MaxLength(4000) ]
        public string Description {get; set;}

        [BindProperty, Required, MinLength(1), MaxLength(30) ]
        public string Genre {get; set;}

        [BindProperty, Required]
        public int DurationMin {get; set;}

        [Required]
        public byte[] Image { get; set; }

        [BindProperty, Required, MinLength(1), MaxLength(400), Url]
        public string TrailerURL {get; set;}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var file = "/Temp/"+Request.Form["file"];
            string fileName = Path.GetFileName(file);
            string ext = Path.GetExtension(fileName);
            if(ext != ".jpg" && ext != ".JPG")
            {
               return Page();         
            }
            byte[] fileContent = null;  
            System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);  
            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);  
            long byteLength = new System.IO.FileInfo(file).Length;  
            fileContent = binaryReader.ReadBytes((Int32)byteLength);  
            fs.Close();  
            fs.Dispose();  
            binaryReader.Close();  

            var newMovie = new MoonriseMovies.Models.Movie
            {
                Title = Title,
                Description = Description,
                Genre = Genre,
                DurationMin = DurationMin,
                Image = fileContent,
                TrailerURL = TrailerURL
            };
            db.Movies.Add(newMovie);
            await db.SaveChangesAsync();

            return RedirectToPage("../AddSuccess");
        }
    }
}
