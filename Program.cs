using VolunteerMatch.Data.Repositories.Interfaces;
using VolunteerMatch.Services;
using VolunteerMatch.Services.Interface;
using VolunteerMatch.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services for repository pattern here. Dependency injection is taking place here.
builder.Services.AddScoped<ICauseRepository, CauseRepository>();
builder.Services.AddScoped<ICauseService, CauseService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
