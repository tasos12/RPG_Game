using _NET_Course.Models;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base (options)
    {

    }

    public DbSet<Character> Characters { get; set; }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Weapon> Weapon { get; set; }

    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skill>().HasData(
            new Skill { ID = 1, Name = "Fireball", Damage = 30},
            new Skill { ID = 2, Name = "Icicle", Damage = 30 },
            new Skill { ID = 3, Name = "Breeze", Damage = 30 }
        );
    }
}