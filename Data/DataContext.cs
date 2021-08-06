using _NET_Course.Models;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base (options)
    {

    }

    public DbSet<Character> Characters { get; set; }
}