using CMSApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMSApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public override DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, CountryName = "Sweden" },
                new Country { Id = 2, CountryName = "Denmark" },
                new Country { Id = 3, CountryName = "Norway" }
                );
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, CityName = "Göteborg", CountryId = 1 },
                new City { Id = 2, CityName = "Stockholm", CountryId = 1 },
                new City { Id = 3, CityName = "Oslo", CountryId = 3 },
                new City { Id = 4, CityName = "Köpenhamn", CountryId = 2 }
                );

            string roleId = Guid.NewGuid().ToString();
            string userId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id= roleId,
                    Name= "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = roleId
                });

            var adminUser = new ApplicationUser
            {
                Id = userId,
                FirstName = "Admin",
                Email = "admin@site.com",
                NormalizedEmail = "ADMIN@SITE.COM",
                EmailConfirmed = true,
                UserName = "admin@site.com",
                NormalizedUserName = "ADMIN@SITE.COM",
                CityId = 1
            };

            PasswordHasher<ApplicationUser> pH = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = pH.HashPassword(adminUser, "admin");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = roleId,
                    UserId = userId
                });

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });
        }
    }
}
