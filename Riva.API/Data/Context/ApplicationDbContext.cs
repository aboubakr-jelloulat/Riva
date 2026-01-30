using Microsoft.EntityFrameworkCore;
using Riva.API.Models;
namespace Riva.API.Data.Context;


public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Villa> Villas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Villa>().HasData(
        new Villa
        {
            Id = 1,
            Name = "Sunset Paradise",
            Details = "Cozy villa with a private terrace and beautiful sunset views.",
            Rate = 350.0,
            Sqft = 2000,
            Occupancy = 4,
            ImageUrl = null,
            CreatedDate = new DateTime(2024, 2, 1),
            UpdatedDate = new DateTime(2024, 2, 1)
        },
        new Villa
        {
            Id = 2,
            Name = "Mountain Escape",
            Details = "Charming villa surrounded by pine trees and a peaceful mountain setting.",
            Rate = 600.0,
            Sqft = 2800,
            Occupancy = 6,
            ImageUrl = null,
            CreatedDate = new DateTime(2024, 2, 10),
            UpdatedDate = new DateTime(2024, 2, 10)
        });
    }
}

