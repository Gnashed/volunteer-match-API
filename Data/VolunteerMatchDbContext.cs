// Data/VolunteerMatchDbContext.cs

using Microsoft.EntityFrameworkCore;
using VolunteerMatch.Data.SampleData;
using VolunteerMatch.Models;

namespace VolunteerMatch.Data;

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

        // Seed data
        modelBuilder.Entity<Cause>().HasData(
            CauseData.GetCauses());

        modelBuilder.Entity<Volunteer>().HasData(
            VolunteerData.GetVolunteers());

        modelBuilder.Entity<Organization>().HasData(
            OrganizationData.GetOrganizations());

        modelBuilder.Entity<OrganizationFollower>().HasData(
            OrganizationFollowerData.GetOrganizationFollowers());
    }
}