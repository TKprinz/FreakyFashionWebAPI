using Microsoft.EntityFrameworkCore;
using FreakyFashionWebAPI.Domain;



namespace FreakyFashionWebAPI.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    // Se till att Products har samma åtkomstnivå (i detta fall public)
    public DbSet<Product> Products { get; set; }
}
