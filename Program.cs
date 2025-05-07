using System.Text.Json;

using volunteerMatch.Models;
using volunteerMatch.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Read connection string from user-secrets
var connStr = builder.Configuration["volunteer-match-APIDbConnectionString"];
if (string.IsNullOrEmpty(connStr))
    throw new InvalidOperationException(
        "Connection string 'volunteer-match-APIDbConnectionString' not found.");

// Register EF Core with Npgsql
builder.Services.AddDbContext<VolunteerMatchDbContext>(opts =>
    opts.UseNpgsql(connStr));

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JSON settings: camel‑case plus cycle‑ignore
builder.Services.Configure<JsonOptions>(opts =>
{
    // produce camelCased JSON keys
    opts.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    opts.SerializerOptions.DictionaryKeyPolicy   = JsonNamingPolicy.CamelCase;

    // still ignore cycles
    opts.SerializerOptions.ReferenceHandler      = ReferenceHandler.IgnoreCycles;
});

// CORS
builder.Services.AddCors(opts =>
    opts.AddPolicy("AllowFrontend", p =>
        p.WithOrigins("http://localhost:3000")
         .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials()));

var app = builder.Build();

app.UseCors("AllowFrontend");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// ------------------
// SEED DATABASE
// ------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VolunteerMatchDbContext>();

    // Causes
    if (!db.Causes.Any())
    {
        db.Causes.AddRange(CauseData.Causes.Select(c => new Cause
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            ImageUrl = c.ImageUrl
        }));
        db.SaveChanges();
    }

    // Volunteers
    if (!db.Volunteers.Any())
    {
        db.Volunteers.AddRange(VolunteerData.Volunteers.Select(v => new Volunteer
        {
            Id = v.Id,
            Uid = v.Uid,
            FirstName = v.FirstName,
            LastName = v.LastName,
            Email = v.Email,
            ImageUrl = v.ImageUrl
        }));
        db.SaveChanges();
    }

    // Organizations (many-to-many via OrganizationCauses)
    if (!db.Organizations.Any())
    {
        var orgs = OrganizationData.Organizations
            .Select(dto => new Organization
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                ImageURL = dto.ImageURL,
                Location = dto.Location,
                IsFollowing = dto.IsFollowing,
                VolunteerId = dto.VolunteerId,
                OrganizationCauses = dto.CauseIds
                    .Select(cid => new OrganizationCause { CauseId = cid })
                    .ToList()
            })
            .ToList();

        db.Organizations.AddRange(orgs);
        db.SaveChanges();
    }
}

// =============================
// VOLUNTEERS
// =============================
app.MapGet("/volunteers", async (VolunteerMatchDbContext db) =>
    await db.Volunteers.ToListAsync())
   .WithName("GetVolunteers");

app.MapGet("/volunteers/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var v = await db.Volunteers.FindAsync(id);
    return v is not null ? Results.Ok(v) : Results.NotFound();
})
   .WithName("GetVolunteerById");

// Full volunteer data (GET)
app.MapGet("/volunteers/uid/{uid}", async (string uid, VolunteerMatchDbContext db) =>
{
    var v = await db.Volunteers.FirstOrDefaultAsync(x => x.Uid == uid);
    return v is not null ? Results.Ok(v) : Results.NotFound();
})
.WithName("GetVolunteerByUid");

// Only volunteerId (POST)
app.MapPost("/volunteers/uid/{uid}/id", async (string uid, VolunteerMatchDbContext db) =>
{
    var volunteerId = await db.Volunteers
                               .Where(x => x.Uid == uid)
                               .Select(v => v.Id)
                               .FirstOrDefaultAsync();
    if (volunteerId != 0)
    {
        return Results.Ok(new { volunteerId });
    }
    else
    {
        return Results.NotFound("Volunteer not found");
    }
})
.WithName("GetVolunteerIdByUid");


app.MapGet("/volunteers/{volunteerId}/followed-organizations", async (int volunteerId, VolunteerMatchDbContext db) =>
{
    var orgIds = await db.OrganizationFollowers
        .Where(f => f.VolunteerId == volunteerId)
        .Select(f => f.OrganizationId)
        .ToListAsync();
    var orgs = await db.Organizations
        .Where(o => orgIds.Contains(o.Id))
        .ToListAsync();
    return Results.Ok(orgs);
})
   .WithName("GetFollowedOrgs");

app.MapPost("/volunteers", async (Volunteer volunteer, VolunteerMatchDbContext db) =>
{
    db.Volunteers.Add(volunteer);
    await db.SaveChangesAsync();
    return Results.Created($"/volunteers/{volunteer.Id}", volunteer);
})
   .WithName("CreateVolunteer");

app.MapPut("/volunteers/{id}", async (int id, Volunteer input, VolunteerMatchDbContext db) =>
{
    var v = await db.Volunteers.FindAsync(id);
    if (v is null) return Results.NotFound();

    db.Entry(v).CurrentValues.SetValues(input);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
   .WithName("UpdateVolunteer");

app.MapDelete("/volunteers/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var v = await db.Volunteers.FindAsync(id);
    if (v is null) return Results.NotFound();

    db.Volunteers.Remove(v);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
   .WithName("DeleteVolunteer");

// =============================
// ORGANIZATIONS
// =============================
app.MapGet("/organizations", async (int? causeId, VolunteerMatchDbContext db) =>
{
    var q = db.Organizations
        .Include(o => o.OrganizationCauses)
            .ThenInclude(oc => oc.Cause)
        .AsQueryable();
    if (causeId.HasValue)
        q = q.Where(o => o.OrganizationCauses.Any(oc => oc.CauseId == causeId.Value));
    return await q.ToListAsync();
})
   .WithName("GetOrganizations");

app.MapGet("/organizations/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var org = await db.Organizations
        .Include(o => o.OrganizationCauses)
            .ThenInclude(oc => oc.Cause)
        .FirstOrDefaultAsync(o => o.Id == id);
    return org is not null ? Results.Ok(org) : Results.NotFound();
})
   .WithName("GetOrganizationById");

app.MapGet("/organizations/{organizationId}/volunteers", async (int organizationId, VolunteerMatchDbContext db) =>
{
    var followers = await db.OrganizationFollowers
        .Where(f => f.OrganizationId == organizationId)
        .Select(f => new {
            f.Volunteer.Id,
            f.Volunteer.FirstName,
            f.Volunteer.LastName,
            f.Volunteer.Email,
            f.Volunteer.ImageUrl
        })
        .ToListAsync();
    return Results.Ok(followers);
})
   .WithName("GetOrganizationVolunteers");

app.MapPost("/organizations", async (OrganizationSeedDto dto, VolunteerMatchDbContext db) =>
{
    var org = new Organization
    {
        Name              = dto.Name,
        Description       = dto.Description,
        ImageURL          = dto.ImageURL,
        Location          = dto.Location,
        IsFollowing       = dto.IsFollowing,
        VolunteerId       = dto.VolunteerId,
        OrganizationCauses = dto.CauseIds.Select(cid => new OrganizationCause { CauseId = cid }).ToList()
    };
    db.Organizations.Add(org);
    await db.SaveChangesAsync();
    return Results.Created($"/organizations/{org.Id}", org);
})
   .WithName("CreateOrganization");

app.MapPut("/organizations/{id}", async (int id, OrganizationSeedDto dto, VolunteerMatchDbContext db) =>
{
    var org = await db.Organizations
        .Include(o => o.OrganizationCauses)
        .FirstOrDefaultAsync(o => o.Id == id);
    if (org is null) return Results.NotFound();

    org.Name = dto.Name;
    org.Description = dto.Description;
    org.ImageURL = dto.ImageURL;
    org.Location = dto.Location;
    org.IsFollowing = dto.IsFollowing;

    org.OrganizationCauses.Clear();
    foreach (var cid in dto.CauseIds)
        org.OrganizationCauses.Add(new OrganizationCause { CauseId = cid });

    await db.SaveChangesAsync();
    return Results.NoContent();
})
   .WithName("UpdateOrganization");

app.MapDelete("/organizations/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var org = await db.Organizations.FindAsync(id);
    if (org is null) return Results.NotFound();

    db.Organizations.Remove(org);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
   .WithName("DeleteOrganization");

// =============================
// CAUSES
// =============================
app.MapGet("/causes", async (VolunteerMatchDbContext db) =>
    await db.Causes
        .Include(c => c.OrganizationCauses)
            .ThenInclude(oc => oc.Organization)
        .ToListAsync())
   .WithName("GetCauses");

app.MapGet("/causes/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var c = await db.Causes
        .Include(ca => ca.OrganizationCauses)
            .ThenInclude(oc => oc.Organization)
        .FirstOrDefaultAsync(ca => ca.Id == id);
    return c is not null ? Results.Ok(c) : Results.NotFound();
})
   .WithName("GetCauseById");

app.MapPost("/causes", async (Cause cause, VolunteerMatchDbContext db) =>
{
    db.Causes.Add(cause);
    await db.SaveChangesAsync();
    return Results.Created($"/causes/{cause.Id}", cause);
})
   .WithName("CreateCause");

app.MapPut("/causes/{id}", async (int id, Cause input, VolunteerMatchDbContext db) =>
{
    var cause = await db.Causes.FindAsync(id);
    if (cause is null) return Results.NotFound();
    db.Entry(cause).CurrentValues.SetValues(input);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
   .WithName("UpdateCause");

app.MapDelete("/causes/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var cause = await db.Causes.FindAsync(id);
    if (cause is null) return Results.NotFound();
    db.Causes.Remove(cause);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
   .WithName("DeleteCause");

// =============================
// ORGANIZATION FOLLOWERS
// =============================
app.MapGet("/organizationfollowers", async (VolunteerMatchDbContext db) =>
    await db.OrganizationFollowers.ToListAsync())
   .WithName("GetOrganizationFollowers");

app.MapGet("/organizationfollowers/check", async (int volunteerId, int organizationId, VolunteerMatchDbContext db) =>
{
    var isFollowing = await db.OrganizationFollowers
        .AnyAsync(f => f.VolunteerId == volunteerId && f.OrganizationId == organizationId);
    return Results.Ok(new { isFollowing });
})
   .WithName("CheckOrganizationFollower");

app.MapPost("/organizationfollowers", async (OrganizationFollower f, VolunteerMatchDbContext db) =>
{
    db.OrganizationFollowers.Add(f);
    await db.SaveChangesAsync();
    return Results.Created($"/organizationfollowers/{f.VolunteerId}/{f.OrganizationId}", f);
})
   .WithName("CreateOrganizationFollower");

app.MapDelete("/organizationfollowers/{volunteerId}/{organizationId}", async (int volunteerId, int organizationId, VolunteerMatchDbContext db) =>
{
    var rec = await db.OrganizationFollowers.FindAsync(volunteerId, organizationId);
    if (rec is null) return Results.NotFound();
    db.OrganizationFollowers.Remove(rec);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
   .WithName("DeleteOrganizationFollower");

// =============================
// VOLUNTEER FOLLOWERS
// =============================
app.MapGet("/volunteerfollowers", async (VolunteerMatchDbContext db) =>
    await db.VolunteerFollowers.ToListAsync())
   .WithName("GetVolunteerFollowers");

app.MapGet("/volunteerfollowers/check", async (int followerId, int followingId, VolunteerMatchDbContext db) =>
{
    var isFollowing = await db.VolunteerFollowers
        .AnyAsync(f => f.FollowerId == followerId && f.FollowedId == followingId);
    return Results.Ok(new { isFollowing });
})
   .WithName("CheckVolunteerFollower");

app.MapPost("/volunteerfollowers", async (VolunteerFollower f, VolunteerMatchDbContext db) =>
{
    db.VolunteerFollowers.Add(f);
    await db.SaveChangesAsync();
    return Results.Created($"/volunteerfollowers/{f.FollowerId}/{f.FollowedId}", f);
})
   .WithName("CreateVolunteerFollower");

app.MapDelete("/volunteerfollowers/{followerId}/{followingId}", async (int followerId, int followingId, VolunteerMatchDbContext db) =>
{
    var rec = await db.VolunteerFollowers.FindAsync(followerId, followingId);
    if (rec is null) return Results.NotFound();
    db.VolunteerFollowers.Remove(rec);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
   .WithName("DeleteVolunteerFollower");

app.Run();