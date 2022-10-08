using System.IO;

using ActivityTracker.Domain.Database;

using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.UnitTests.Helpers
{
    public static class DbContextHelper
    {
        public static UserDbContext CreateTestDb()
        {
            string tempFile = Path.GetTempFileName();

            DbContextOptions<UserDbContext> options = new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlite($"Data Source={tempFile};")
                .Options;

            UserDbContext dbContext = new(options);
            dbContext.Database.Migrate();
            return dbContext;
        }
    }
}