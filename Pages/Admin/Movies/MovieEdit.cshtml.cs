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
    public class MovieEditModel : PageModel
    {
        private MoonriseDBContext db;
        public MovieEditModel(MoonriseDBContext db) => this.db = db;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty, Required, MinLength(1), MaxLength(250)]
        public string Title { get; set; }

        [BindProperty, Required, MinLength(1), MaxLength(4000) ]
        public string Description {get; set;}

        [BindProperty, Required, MinLength(1), MaxLength(30) ]
        public string Genre {get; set;}

        [BindProperty, Required]
        public int DurationMin {get; set;}

        public byte[] Image { get; set; }

        [BindProperty, Required, MinLength(1), MaxLength(400), Url]
        public string TrailerURL {get; set;}

        public Movie Movie { get; set; }

        public async Task OnGetAsync()
        {
            Movie = await db.Movies.FindAsync(Id);
            byte[] bytes;
            bytes = Movie.Image;  
            string imageBase64Data = Convert.ToBase64String(bytes);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewData["Image"+Movie.Id.ToString()] = imageDataURL;
        }   

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Movie = await db.Movies.FindAsync(Id);
         
            var file = "/Temp/"+Request.Form["file"];
            if(file != null)
            {
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
                Movie.Image = fileContent;
            }
         
            Movie.Title = Title;
            Movie.Description = Description;
            Movie.Genre = Genre;
            Movie.DurationMin = DurationMin;
            Movie.TrailerURL = TrailerURL;

            db.SaveChanges();
            return RedirectToPage("../../Admin/Movies/Movies"); 
        }
    }
}
