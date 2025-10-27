using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use a design-time connection string
        var connectionString = "Server=localhost;Database=MagicSessions;Uid=magicuser;Pwd=magicpass123!;";
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}