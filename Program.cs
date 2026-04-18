using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// === DATABASE SETTING ===
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Automatically Create Tables in PostgreSQL DB on startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        Console.WriteLine("PostgreSQL Database connected and tables ensured.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"PostgreSQL DB Connection/Creation Failed: {ex.Message}");
    }
}

// API Endpoints for Blogs
app.MapGet("/api/blogs", async (AppDbContext db) =>
    await db.Blogs.ToListAsync());

app.MapPost("/api/blogs", async (Blog blog, AppDbContext db) =>
{
    db.Blogs.Add(blog);
    await db.SaveChangesAsync();
    return Results.Created($"/api/blogs/{blog.Id}", blog);
});

// API Endpoints for Dashboard Stats
app.MapGet("/api/stats", async (AppDbContext db) =>
    await db.Stats.ToListAsync());

app.MapPost("/api/stats/visitor", async (AppDbContext db) =>
{
    var visitorStat = await db.Stats.FirstOrDefaultAsync(s => s.Key == "DailyVisitors");
    if (visitorStat == null)
    {
        visitorStat = new Stat { Key = "DailyVisitors", Value = 1 };
        db.Stats.Add(visitorStat);
    }
    else
    {
        visitorStat.Value++;
    }
    await db.SaveChangesAsync();
    return Results.Ok(visitorStat);
});

app.Run();
