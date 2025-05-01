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
    public DbSet<VolunteerFollower> VolunteerFollowers { get;}

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

        // Composite key for VolunteerFollower
        modelBuilder.Entity<VolunteerFollower>()
            .HasKey(vf => new { vf.FollowerId, vf.FollowedId });

        modelBuilder.Entity<VolunteerFollower>()
            .HasOne(vf => vf.Follower)
            .WithMany(v => v.Followers)  
            .HasForeignKey(vf => vf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<VolunteerFollower>()
            .HasOne(vf => vf.Followed)
            .WithMany(v => v.Followed)
            .HasForeignKey(vf => vf.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Organization>()
            .HasOne(o => o.Volunteer)
            .WithMany()
            .HasForeignKey(o => o.VolunteerId);

        modelBuilder.Entity<Organization>()
            .HasOne(o => o.Cause)
            .WithMany(c => c.Organizations)
            .HasForeignKey(o => o.CauseId);
    }
} 
