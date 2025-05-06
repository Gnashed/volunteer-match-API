using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using VolunteerMatch.Data; // For ignoring cycling.
using VolunteerMatch.Data.Repositories.Interfaces;
// using VolunteerMatch.Services;
// using VolunteerMatch.Services.Interface;
using VolunteerMatch.Data.Repositories;
// The following imports are needed for handling token validation and key encoding.
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// The following keys are read .env (for Docker) and from config (user secrets in this case for local development).
var jwtKey = Environment.GetEnvironmentVariable("JWT:KEY")
             ?? builder.Configuration["JWT:KEY"]
             ?? throw new Exception("Missing JWT key");
var jwtIssuer = builder.Configuration["JWT:ISSUER"] ?? "VolunteerMatch";
var jwtAudience = builder.Configuration["JWT:AUDIENCE"] ?? "VolunteerMatchClient";

// Just like the other services in Program.cs, we're adding JWT authentication to the service container.
// Here we're specifically setting the default authentication and challenge scheme to JWT Bearer.
builder.Services.AddAuthentication(options =>
    {
        // Tells .NET how to validate incoming request (check for a JWT in the `Authorization: Bearer <token>` header).
        // Without it, [Authorize] wouldn't know which scheme to use to authenticate the request.
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        // Tells .NET what to do when a user hits a protected endpoint w/o being authenticated. In this case, ...
        // it challenges the client with a 401 `Unauthorized`.
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // Configures how incoming JWTs are validated.
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ValidateIssuer and ValidateAudience == ensures the token was meant for this app.
            ValidateIssuer = true,
            ValidateAudience = true,
            // Ensures it was signed with our key.
            ValidateIssuerSigningKey = true,
            // Ensures the token hasn't expired.
            ValidateLifetime = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            // Builds a secure key from our `jwtKey` string for signature validation.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

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
builder.Services.AddNpgsql<VolunteerMatchDbContext>(builder.Configuration["VOLUNTEERMATCH_DB_CONNECTION_STRING"]);

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
// Adds authorization middleware which enables `[authorize]` on our controller endpoints.
builder.Services.AddAuthorization();

var app = builder.Build();

// Automatic migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VolunteerMatchDbContext>();
    db.Database.Migrate(); // apply migrations
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || builder.Configuration["ENABLESWAGGER"] == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// Validates JWTs issued to clients.
app.UseAuthentication();    // Validates the JWT
app.UseAuthorization();     // Enforces the `[authorize]` rules.

// ASP.NET Core will scan for controller endpoints and expose them. W/o this line, the controllers are not registered
// and Swagger won't see them.
app.MapControllers();

app.Run();
