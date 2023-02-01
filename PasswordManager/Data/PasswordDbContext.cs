
using Microsoft.EntityFrameworkCore;
using PasswordManager.Models;

namespace PasswordManager.Data;

public class PasswordDbContext : DbContext
{
    public PasswordDbContext(DbContextOptions<PasswordDbContext> options) : base(options)
    {

    }

    // increment the id of the model
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();

        // fluent api stuff would go in this method
    }

    public DbSet<AccountModel> PasswordTableEF { get; set; }
    public DbSet<UserModel> UserTableEF { get; set; }


}
