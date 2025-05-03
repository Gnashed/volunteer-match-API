using volunteerMatch.Models;
using volunteerMatch.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1) Read your user‑secret
var connStr = builder.Configuration["VolunteerMatchDbConnectionString"];
if (string.IsNullOrEmpty(connStr))
    throw new InvalidOperationException(
        "Connection string 'VolunteerMatchDbConnectionString' not found in configuration.");

// 2) Register EF Core with Npgsql
builder.Services.AddDbContext<VolunteerMatchDbContext>(options =>
    options.UseNpgsql(connStr));

// Enable OpenAPI (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Prevent circular JSON
builder.Services.Configure<JsonOptions>(opts =>
    opts.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// CORS for localhost:3000
builder.Services.AddCors(opts =>
    opts.AddPolicy("AllowFrontend", p =>
        p.WithOrigins("http://localhost:3000")
         .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials()));

var app = builder.Build();

// Middlewares
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

    if (!db.Causes.Any())
    {
        db.Causes.AddRange(CauseData.Causes.Select(c => new Cause {
            Id          = c.Id,
            Name        = c.Name,
            Description = c.Description,
            ImageUrl    = c.ImageUrl
        }));
        db.SaveChanges();
    }

    if (!db.Volunteers.Any())
    {
        db.Volunteers.AddRange(VolunteerData.Volunteers.Select(v => new Volunteer {
            Id        = v.Id,
            Uid       = v.Uid,
            FirstName = v.FirstName,
            LastName  = v.LastName,
            Email     = v.Email,
            ImageUrl  = v.ImageUrl
        }));
        db.SaveChanges();
    }

    if (!db.Organizations.Any())
    {
        var orgs = OrganizationData.Organizations
            .Select(dto => new Organization {
                Id                = dto.Id,
                Name              = dto.Name,
                Description       = dto.Description,
                ImageURL          = dto.ImageURL,
                Location          = dto.Location,
                IsFollowing       = dto.IsFollowing,
                VolunteerId       = 1,
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
   .WithName("GetVolunteers")
   .Produces<List<Volunteer>>(StatusCodes.Status200OK);

app.MapGet("/volunteers/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var v = await db.Volunteers.FindAsync(id);
    return v is not null ? Results.Ok(v) : Results.NotFound();
})
   .WithName("GetVolunteerById")
   .Produces<Volunteer>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound);

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
   .WithName("GetOrganizations")
   .Produces<List<Organization>>(StatusCodes.Status200OK);

app.MapGet("/organizations/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var org = await db.Organizations
        .Include(o => o.OrganizationCauses)
            .ThenInclude(oc => oc.Cause)
        .FirstOrDefaultAsync(o => o.Id == id);

    return org is not null ? Results.Ok(org) : Results.NotFound();
})
   .WithName("GetOrganizationById")
   .Produces<Organization>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound);

// =============================
// CAUSES
// =============================
app.MapGet("/causes", async (VolunteerMatchDbContext db) =>
    await db.Causes
        .Include(c => c.OrganizationCauses)
            .ThenInclude(oc => oc.Organization)
        .ToListAsync())
   .WithName("GetCauses")
   .Produces<List<Cause>>(StatusCodes.Status200OK);

app.MapGet("/causes/{id}", async (int id, VolunteerMatchDbContext db) =>
{
    var c = await db.Causes
        .Include(ca => ca.OrganizationCauses)
            .ThenInclude(oc => oc.Organization)
        .FirstOrDefaultAsync(ca => ca.Id == id);

    return c is not null ? Results.Ok(c) : Results.NotFound();
})
   .WithName("GetCauseById")
   .Produces<Cause>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound);

app.Run();
