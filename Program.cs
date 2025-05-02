using volunteerMatch.Models;
using volunteerMatch.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Enable OpenAPI (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow passing DateTimes without timezone data
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Connect API to PostgreSQL Database
builder.Services.AddNpgsql<VolunteerMatchDbContext>(builder.Configuration["volunteer-match-APIDbConnectionString"]);

// Set JSON serialization options (Prevents circular JSON errors)
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// CORS Policy (Allow frontend at localhost:3000)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

var app = builder.Build();

// Enable CORS Middleware
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VolunteerMatchDbContext>();

    // Seed Causes
    if (!db.Causes.Any())
    {
        db.Causes.AddRange(volunteerMatch.Data.CauseData.Causes.Select(c => new Cause
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            ImageUrl = c.ImageUrl
        }));
        db.SaveChanges();
    }

    // Seed Volunteers
    if (!db.Volunteers.Any())
    {
        db.Volunteers.AddRange(volunteerMatch.Data.VolunteerData.Volunteers.Select(v => new Volunteer
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

    // Seed Organizations (with many-to-many via OrganizationCauses)
    if (!db.Organizations.Any())
    {
        var orgEntities = OrganizationData.Organizations
            .Select(dto => new Organization
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                ImageURL = dto.ImageURL,
                Location = dto.Location,
                IsFollowing = dto.IsFollowing,
                VolunteerId = 1,
                OrganizationCauses = dto.CauseIds
                    .Select(cid => new OrganizationCause { CauseId = cid })
                    .ToList()
            })
            .ToList();

        db.Organizations.AddRange(orgEntities);
        db.SaveChanges();
    }
}

//
// VOLUNTEERS
//

app.MapGet("/volunteers", async (VolunteerMatchDbContext db) =>
    await db.Volunteers.ToListAsync());

app.MapGet("/volunteers/{id}", async (int id, VolunteerMatchDbContext db) =>
    await db.Volunteers.FindAsync(id));

app.MapGet("/volunteers/uid/{uid}", async (string uid, VolunteerMatchDbContext db) =>
{
    var match = await db.Volunteers.FirstOrDefaultAsync(v => v.Uid == uid);
    return match is not null ? Results.Ok(match) : Results.NotFound();
});

app.MapGet("/volunteers/{volunteerId}/followed-organizations", async (int volunteerId, VolunteerMatchDbContext db) =>
{
    var orgIds = await db.OrganizationFollowers
        .Where(f => f.VolunteerId == volunteerId)
        .Select(f => f.OrganizationId)
        .ToListAsync();

    var organizations = await db.Organizations
        .Where(o => orgIds.Contains(o.Id))
        .ToListAsync();

    return Results.Ok(organizations);
});

app.MapPost("/volunteers", async (Volunteer volunteer, VolunteerMatchDbContext db) =>
{
    db.Volunteers.Add(volunteer);
    await db.SaveChangesAsync();
    return Results.Created($"/volunteers/{volunteer.Id}", volunteer);
});

app.MapPut("/volunteers/{id}", async (int id, Volunteer input, VolunteerMatchDbContext db) =>
{
    var v = await db.Volunteers.FindAsync(id);
    if (v == null) return Results.NotFound();

    db.Entry(v).CurrentValues.SetValues(input);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/volunteers/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var v = await db.Volunteers.FindAsync(id);
    if (v == null) return Results.NotFound();

    db.Volunteers.Remove(v);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//
// ORGANIZATIONS
//

app.MapGet("/organizations", async (HttpRequest request, VolunteerMatchDbContext db) =>
{
    var causeIdParam = request.Query["causeId"].FirstOrDefault();

    if (!string.IsNullOrEmpty(causeIdParam) && int.TryParse(causeIdParam, out int causeId))
    {
        var filtered = await db.Organizations
            .Where(o => o.OrganizationCauses.Any(oc => oc.CauseId == causeId))
            .Include(o => o.OrganizationCauses)
                .ThenInclude(oc => oc.Cause)
            .ToListAsync();

        return Results.Ok(filtered);
    }

    var allOrgs = await db.Organizations
        .Include(o => o.OrganizationCauses)
            .ThenInclude(oc => oc.Cause)
        .ToListAsync();

    return Results.Ok(allOrgs);
});

app.MapGet("/organizations/{organizationId}/volunteers", async (int organizationId, VolunteerMatchDbContext db) =>
{
    var followers = await db.OrganizationFollowers
        .Where(f => f.OrganizationId == organizationId)
        .Select(f => new
        {
            f.Volunteer.Id,
            f.Volunteer.FirstName,
            f.Volunteer.LastName,
            f.Volunteer.Email,
            f.Volunteer.ImageUrl
        })
        .ToListAsync();
    return Results.Ok(followers);
});

app.MapGet("/organizations/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var org = await db.Organizations
        .Include(o => o.OrganizationCauses)
            .ThenInclude(oc => oc.Cause)
        .FirstOrDefaultAsync(o => o.Id == id);

    return org is not null
        ? Results.Ok(org)
        : Results.NotFound();
});

app.MapPost("/organizations", async (Organization organization, VolunteerMatchDbContext db) =>
{
    db.Organizations.Add(organization);
    await db.SaveChangesAsync();
    return Results.Created($"/organizations/{organization.Id}", organization);
});

app.MapPut("/organizations/{id}", async (int id, Organization input, VolunteerMatchDbContext db) =>
{
    var org = await db.Organizations.FindAsync(id);
    if (org == null) return Results.NotFound();

    db.Entry(org).CurrentValues.SetValues(input);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/organizations/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var org = await db.Organizations.FindAsync(id);
    if (org == null) return Results.NotFound();

    db.Organizations.Remove(org);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//
// CAUSES
//

app.MapGet("/causes", async (VolunteerMatchDbContext db) =>
    await db.Causes.Include(c => c.Organizations).ToListAsync());

app.MapGet("/causes/{id}", async (int id, VolunteerMatchDbContext db) =>
    await db.Causes.Include(c => c.Organizations).FirstOrDefaultAsync(c => c.Id == id));

app.MapPost("/causes", async (Cause cause, VolunteerMatchDbContext db) =>
{
    db.Causes.Add(cause);
    await db.SaveChangesAsync();
    return Results.Created($"/causes/{cause.Id}", cause);
});

app.MapPut("/causes/{id}", async (int id, Cause input, VolunteerMatchDbContext db) =>
{
    var cause = await db.Causes.FindAsync(id);
    if (cause == null) return Results.NotFound();

    db.Entry(cause).CurrentValues.SetValues(input);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/causes/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var cause = await db.Causes.FindAsync(id);
    if (cause == null) return Results.NotFound();

    db.Causes.Remove(cause);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
