using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using VolunteerMatch.Data; // For ignoring cycling.
using VolunteerMatch.Data.Repositories.Interfaces;
// using VolunteerMatch.Services;
// using VolunteerMatch.Services.Interface;
using VolunteerMatch.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

#region Check comment below this line.
// Add this above the `var app = builder.Build();` line. The middle statement is the most important since it makes an
// instance of the VolunteerMatchDbContext class available to our endpoints.

// allows passing date times without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<VolunteerMatchDbContext>(builder.Configuration["VolunteerMatchDbConnectionString"]);

// Set the JSON serializer options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
#endregion

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services for repository pattern here.
builder.Services.AddScoped<ICauseRepository, CauseRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();
builder.Services.AddScoped<IOrganizationFollowerRepository, OrganizationFollowerRepository>();
// builder.Services.AddScoped<ICauseService, CauseService>();

// Register controllers
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ASP.NET Core wills scan for controller endpoints and expose them. W/o this line, the controllers are not registered
// and Swagger won't see them.
app.MapControllers();

app.Run();
