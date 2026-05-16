using CinemaBookingTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
 
namespace CinemaBookingTask.DataAcess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<MovieSubImg> MovieSubImgs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=cinemaDatabase;Integrated Security=True;TrustServerCertificate=True");
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityPasskeyData>().HasNoKey();
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "Comedy" },
                new Category { Id = 3, Name = "Drama" },
                new Category { Id = 4, Name = "Horror" },
                new Category { Id = 5, Name = "Sci-Fi" }
            );

            modelBuilder.Entity<Cinema>().HasData(
                new Cinema { Id = 1, Name = "Grand Cinema", Description = "Largest cinema in the city", Img = null },
                new Cinema { Id = 2, Name = "Star Cinema", Description = "Premium movie experience", Img = null },
                new Cinema { Id = 3, Name = "Galaxy Cinema", Description = "Family friendly cinema", Img = null }
            );

            modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, Name = "Tom Hanks", Img = null },
                new Actor { Id = 2, Name = "Leonardo DiCaprio", Img = null },
                new Actor { Id = 3, Name = "Scarlett Johansson", Img = null },
                new Actor { Id = 4, Name = "Robert Downey Jr.", Img = null },
                new Actor { Id = 5, Name = "Morgan Freeman", Img = null }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 1,
                    Name = "Inception",
                    Description = "A mind-bending thriller",
                    Price = 50.00,
                    Status = "Available",
                    DateTime = new DateTime(2025, 5, 10),
                    MainImg = null,
                    CategoryId = 5,
                    CinemaId = 1
                },
                new Movie
                {
                    Id = 2,
                    Name = "The Dark Knight",
                    Description = "Batman faces the Joker",
                    Price = 45.00,
                    Status = "Available",
                    DateTime = new DateTime(2025, 5, 15),
                    MainImg = null,
                    CategoryId = 1,
                    CinemaId = 2
                },
                new Movie
                {
                    Id = 3,
                    Name = "Forrest Gump",
                    Description = "Life is like a box of chocolates",
                    Price = 40.00,
                    Status = "Coming Soon",
                    DateTime = new DateTime(2025, 6, 1),
                    MainImg = null,

                    CategoryId = 3,
                    CinemaId = 3
                }
            );

            modelBuilder.Entity<MovieActor>().HasData(
                new MovieActor { MovieId = 1, ActorId = 2 }, 
                new MovieActor { MovieId = 2, ActorId = 4 },  
                new MovieActor { MovieId = 3, ActorId = 1 },  
                new MovieActor { MovieId = 3, ActorId = 5 }   
            );
        }
    }
}
