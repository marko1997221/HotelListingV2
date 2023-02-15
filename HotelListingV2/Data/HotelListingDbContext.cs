using Microsoft.EntityFrameworkCore;

namespace HotelListingV2.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel {
                    Id = 1,
                    Name = "Markov Hotel",
                    Rating = 10,
                    CountryId = 1
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Marijin Hotel",
                    Rating = 9.9,
                    CountryId = 2,
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Milanov Hotel",
                    Rating = 6,
                    CountryId = 3
                },
                new Hotel { 
                    Id= 4,
                    Name="Milicin Hotle",
                    Rating=6.8,
                    CountryId=2
                }
            );
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Srbija",
                    ShortName = "Srb",
                },
                new Country
                {
                    Id = 2,
                    Name = "Mauricijus",
                    ShortName="Mau"
                    
                },
                new Country
                {
                    Id = 3,
                    Name = "Germany",
                    ShortName = "Ger",
                  
                }
            );
        }
    }
}
