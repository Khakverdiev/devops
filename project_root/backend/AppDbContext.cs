using homework_3.Models;
using Microsoft.EntityFrameworkCore;

namespace homework_3;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<HomeTask> Tasks { get; set; }
}
