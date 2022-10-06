using System.IO;

using ActivityTracker.Domain.Database;

using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.UnitTests.Helpers;

public static class DbContextHelper
{
    public static UserDbContext CreateTestDb()
    {
        var tempFile = Path.GetTempFileName();

        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseSqlite($"Data Source={tempFile};")
            .Options;

       var  dbContext = new UserDbContext(options);
        dbContext.Database.Migrate();
        return dbContext;
    }
}