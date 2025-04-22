using volunteerMatch.Models;
using volunteerMatch.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore.Query;
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

//  CORS Policy (Allow frontend at `localhost:3000`)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

var app = builder.Build();

// ✅ Enable CORS Middleware BEFORE routing
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

    // Only seed if DB is empty
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

    if (!db.Organizations.Any())
    {
        db.Organizations.AddRange(volunteerMatch.Data.OrganizationData.Organizations.Select(o => new Organization
        {
            Id = o.Id,
            Name = o.Name,
            Description = o.Description,
            ImageURL = o.ImageURL,
            Location = o.Location,
            CauseId = o.CauseId,
            IsFollowing = o.IsFollowing,
            VolunteerId = 1 // assign a valid volunteerId
        }));
        db.SaveChanges();
    }
}

// =============================
// VOLUNTEERS
// =============================
app.MapGet("/volunteers", async (HttpRequest request, VolunteerMatchDbContext db) =>
{
    var uid = request.Query["uid"].FirstOrDefault();

    if (!string.IsNullOrEmpty(uid))
    {
        var match = await db.Volunteers.FirstOrDefaultAsync(v => v.Uid == uid);
        return match is not null ? Results.Ok(match) : Results.NotFound();
    }

    return Results.Ok(await db.Volunteers.ToListAsync());
});

app.MapGet("/volunteers/{id}", async (int id, VolunteerMatchDbContext db) =>
    await db.Volunteers.FindAsync(id));

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

// =============================
// ORGANIZATIONS
// =============================
app.MapGet("/organizations", async (HttpRequest request, VolunteerMatchDbContext db) =>
{
    var causeIdParam = request.Query["causeId"].FirstOrDefault();

    if (!string.IsNullOrEmpty(causeIdParam) && int.TryParse(causeIdParam, out int causeId))
    {
        return Results.Ok(await db.Organizations
            .Include(o => o.Cause)
            .Where(o => o.CauseId == causeId)
            .ToListAsync());
    }

    return Results.Ok(await db.Organizations.Include(o => o.Cause).ToListAsync());
});

app.MapGet("/organizations/{id}", async (int id, VolunteerMatchDbContext db) =>
    await db.Organizations.Include(o => o.Cause).FirstOrDefaultAsync(o => o.Id == id));

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

// =============================
// CAUSES
// =============================
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

// =============================
// ORGANIZATION FOLLOWERS
// =============================
app.MapGet("/organizationfollowers", async (VolunteerMatchDbContext db) =>
    await db.OrganizationFollowers.ToListAsync());

app.MapPost("/organizationfollowers", async (OrganizationFollower follower, VolunteerMatchDbContext db) =>
{
    db.OrganizationFollowers.Add(follower);
    await db.SaveChangesAsync();
    return Results.Created($"/organizationfollowers/{follower.VolunteerId}/{follower.OrganizationId}", follower);
});

app.MapDelete("/organizationfollowers/{volunteerId}/{organizationId}", async (int volunteerId, int organizationId, VolunteerMatchDbContext db) =>
{
    var record = await db.OrganizationFollowers.FindAsync(volunteerId, organizationId);
    if (record == null) return Results.NotFound();

    db.OrganizationFollowers.Remove(record);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
