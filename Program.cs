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
var connStr = builder.Configuration["VolunteerMatchDbConnectionString"];
if (string.IsNullOrEmpty(connStr))
    throw new InvalidOperationException(
        "Connection string 'VolunteerMatchDbConnectionString' not found.");

// Register EF Core with Npgsql
builder.Services.AddDbContext<VolunteerMatchDbContext>(opts =>
    opts.UseNpgsql(connStr));

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Prevent circular JSON
builder.Services.Configure<JsonOptions>(opts =>
    opts.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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

app.MapGet("/volunteers/uid/{uid}", async (string uid, VolunteerMatchDbContext db) =>
{
    var v = await db.Volunteers.FirstOrDefaultAsync(x => x.Uid == uid);
    return v is not null ? Results.Ok(v) : Results.NotFound();
})
   .WithName("GetVolunteerByUid");

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

// Use OrganizationSeedDto from Models
app.MapPost("/organizations", async (OrganizationSeedDto dto, VolunteerMatchDbContext db) =>
{
    var org = new Organization
    {
        Name              = dto.Name,
        Description       = dto.Description,
        ImageURL          = dto.ImageURL,
        Location          = dto.Location,
        IsFollowing       = dto.IsFollowing,
        VolunteerId       = 1,  // or dto.VolunteerId if added
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

// ... remaining endpoints unchanged ...

app.Run();
