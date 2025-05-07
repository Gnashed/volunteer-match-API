using Microsoft.EntityFrameworkCore;
using volunteerMatch.Models;

public class VolunteerMatchDbContext : DbContext
{
    public VolunteerMatchDbContext(DbContextOptions<VolunteerMatchDbContext> options) : base(options) { }

    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Cause> Causes { get; set; }
    public DbSet<OrganizationFollower> OrganizationFollowers { get; set; }
    public DbSet<VolunteerFollower> VolunteerFollowers { get; set; }
    public DbSet<OrganizationCause> OrganizationCauses { get; set; } // Add the DbSet for the join entity

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ------------------------
        // OrganizationFollower setup
        // ------------------------
        modelBuilder.Entity<OrganizationFollower>()
            .HasKey(of => new { of.VolunteerId, of.OrganizationId });

        modelBuilder.Entity<OrganizationFollower>()
            .HasOne(of => of.Volunteer)
            .WithMany(v => v.OrganizationFollowers)
            .HasForeignKey(of => of.VolunteerId);

        modelBuilder.Entity<OrganizationFollower>()
            .HasOne(of => of.Organization)
            .WithMany(o => o.OrganizationFollowers)
            .HasForeignKey(of => of.OrganizationId);

        // ------------------------
        // VolunteerFollower setup
        // ------------------------

        modelBuilder.Entity<Volunteer>()
        .HasAlternateKey(v => v.Uid); // Ensure Uid is marked as an alternate key

    modelBuilder.Entity<VolunteerFollower>()
        .HasKey(vf => new { vf.FollowerId, vf.FollowedId });

    modelBuilder.Entity<VolunteerFollower>()
        .HasOne(vf => vf.Follower)
        .WithMany(v => v.Followers)
        .HasForeignKey(vf => vf.FollowerId)
        .HasPrincipalKey(v => v.Id)
        .OnDelete(DeleteBehavior.Restrict); // Optional

    modelBuilder.Entity<VolunteerFollower>()
        .HasOne(vf => vf.Followed)
        .WithMany(v => v.Followed)
        .HasForeignKey(vf => vf.FollowedId)
        .HasPrincipalKey(v => v.Id) // Int primary key
        .OnDelete(DeleteBehavior.Restrict); // Optional

        // ------------------------
        // Organization ↔ Cause
        // ------------------------
        // Many-to-many relationship via OrganizationCause
        modelBuilder.Entity<OrganizationCause>()
            .HasKey(oc => new { oc.OrganizationId, oc.CauseId });

        modelBuilder.Entity<OrganizationCause>()
            .HasOne(oc => oc.Organization)
            .WithMany(o => o.OrganizationCauses)
            .HasForeignKey(oc => oc.OrganizationId);

        modelBuilder.Entity<OrganizationCause>()
            .HasOne(oc => oc.Cause)
            .WithMany(c => c.OrganizationCauses)
            .HasForeignKey(oc => oc.CauseId);
    }
}