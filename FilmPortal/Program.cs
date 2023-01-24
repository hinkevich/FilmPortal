using FilmPortal.Models.Data;
using FilmPortal.Models.Repo;
using FilmsPortal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<FilmsPortalDbContext>(opt => opt.UseSqlServer("name=ConnectionStrings:FilmPortalDbDefault"));
builder.Services.AddScoped<IActorsRepository, ECActorsRepositiry>();
builder.Services.AddScoped<IFilmsRepository, ECFilmRepository>();
builder.Services.AddScoped<IReviewRepositiry, ECReviewRepositiry>();

builder.Services.AddScoped<IFilmsRepository, ECFilmRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
#pragma warning disable S1075 // URIs should not be hardcoded
    c.SwaggerDoc("v1",
    new OpenApiInfo
    {
        Title = "FilmPortal",
        Version = "v1",
        Description = "Test task for InnowiseLab",
        Contact = new OpenApiContact
        {
            Name = "Serge Hinkevich",
            Email = "hinkevich@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/mit-license.php")
        }
    });
#pragma warning restore S1075 // URIs should not be hardcoded
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}


app.UseAuthorization();

app.MapControllers();

SeedData.EnsurePopolate(app);

app.Run();

/// <summary>
/// For integration tests
/// </summary>
public partial class Program { }
