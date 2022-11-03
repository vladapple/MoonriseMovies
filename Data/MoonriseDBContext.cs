using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoonriseMovies.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MoonriseMovies.Data
{
    public class MoonriseDBContext: IdentityDbContext
    {
        public MoonriseDBContext(DbContextOptions<MoonriseDBContext> options): base(options){}
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=tcp:moonrisemovies.database.windows.net,1433;Initial Catalog=MoonriseMoviesDB;Persist Security Info=False;User ID=MoonriseMoviesDB;Password=Fs$9J0sV%87b;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }
    }
}