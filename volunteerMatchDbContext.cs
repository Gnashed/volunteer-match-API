// Data/VolunteerMatchDbContext.cs
using Microsoft.EntityFrameworkCore;
using volunteerMatch.Models;


public class VolunteerMatchDbContext : DbContext
    {
        public VolunteerMatchDbContext(DbContextOptions<VolunteerMatchDbContext> options) : base(options) { }

        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Cause> Causes { get; set; }
        public DbSet<OrganizationFollower> OrganizationFollowers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for join table
            modelBuilder.Entity<OrganizationFollower>()
                .HasKey(of => new { of.VolunteerId, of.OrganizationId });

            // Relationships
            modelBuilder.Entity<OrganizationFollower>()
                .HasOne(of => of.Volunteer)
                .WithMany(v => v.OrganizationFollowers)
                .HasForeignKey(of => of.VolunteerId);

            modelBuilder.Entity<OrganizationFollower>()
                .HasOne(of => of.Organization)
                .WithMany(o => o.OrganizationFollowers)
                .HasForeignKey(of => of.OrganizationId);

            modelBuilder.Entity<Organization>()
                .HasOne(o => o.Volunteer)
                .WithMany() // You can replace this with a collection if needed
                .HasForeignKey(o => o.VolunteerId);

            modelBuilder.Entity<Organization>()
                .HasOne(o => o.Cause)
                .WithMany(c => c.Organizations)
                .HasForeignKey(o => o.CauseId);

            // Seed data
            modelBuilder.Entity<Cause>().HasData(
                new Cause { Id = 1, Name = "Environment", ImageUrl = "env.jpg", Description = "Protecting the planet" },
                new Cause { Id = 2, Name = "Education", ImageUrl = "edu.jpg", Description = "Supporting learning" },
                new Cause { Id = 3, Name = "Animals", ImageUrl = "animals.jpg", Description = "Helping pets and wildlife" }
            );

            modelBuilder.Entity<Volunteer>().HasData(
                new Volunteer { Id = 1, Uid = "abc123", FirstName = "Alex", LastName = "Johnson", Email = "alex@example.com", ImageUrl = "alex.jpg" },
                new Volunteer { Id = 2, Uid = "def456", FirstName = "Taylor", LastName = "Smith", Email = "taylor@example.com", ImageUrl = "taylor.jpg" }
            );

            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = 1,
                    VolunteerId = 1,
                    Name = "Save the Cats",
                    ImageURL = "cats.jpg",
                    Description = "Rescue and rehome cats",
                    Location = "Nashville",
                    CauseId = 3,
                    IsFollowing = false
                }
            );

            modelBuilder.Entity<OrganizationFollower>().HasData(
                new OrganizationFollower
                {
                    VolunteerId = 2,
                    OrganizationId = 1
                }
            );
        }
    }