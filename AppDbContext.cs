using Microsoft.EntityFrameworkCore;

// Define Models
public class Blog
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Img { get; set; } = string.Empty; // Base64 or URL
    public int Views { get; set; }
    public DateTime DatePublished { get; set; } = DateTime.Now;
}

public class Stat
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty; // e.g. "DailyVisitors"
    public int Value { get; set; }
}

// Define Context
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<Stat> Stats => Set<Stat>();
}
