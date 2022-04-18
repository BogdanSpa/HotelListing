using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Romania"
                },
                new Country
                {
                    Id=2,
                    Name ="Jamaica"
                },
                new Country
                {
                    Id = 3,
                    Name = "Franta"
                }
                );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Continental",
                    Address = "Sector1",
                    CountryId = 1,
                    Rating = 3.2
                },

                new Hotel
                {
                    Id = 2,
                    Name = "Belvedere",
                    Address = "Cetatuie",
                    CountryId = 1,
                    Rating = 7.2
                },

                new Hotel
                {
                    Id = 3,
                    Name = "Melody",
                    Address = "Paris",
                    CountryId = 3,
                    Rating = 8.1
                },

                new Hotel
                {
                    Id = 4,
                    Name = "Sandals Resort",
                    Address = "Negril",
                    CountryId = 2,
                    Rating = 5.1
                }
                );
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        
    }
}
