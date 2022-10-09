using System;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contracts.Http;
using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Exceptions;
using ActivityTracker.Domain.Queries;
using ActivityTracker.UnitTests.Helpers;

using MediatR;

using Shouldly;

namespace ActivityTracker.UnitTests.Queries
{
    public class GetActFromNameTest
    {
        private readonly UserDbContext _dbContext;
        private readonly IRequestHandler<AllActivityQuery, AllActivityQueryResult> _handler;

        public GetActFromNameTest()
        {
            _dbContext = DbContextHelper.CreateTestDb();
            _handler = new AllActivityQueryHandler(_dbContext);
        }

        [Fact]
        public async Task HandleShouldReturnActivityFromName()
        {
            // Arrange
            Contracts.Database.User user = new()
            {
                Name = Guid.NewGuid().ToString(),
                ActivityUsers = new[]
                {
                    new Contracts.Database.ActivityUser()
                    {
                        ActivityType = Guid.NewGuid().ToString()
                    },
                    new Contracts.Database.ActivityUser()
                    {
                        ActivityType = Guid.NewGuid().ToString()
                    }
                }
            };

            _ = await _dbContext.AddAsync(user);
            _ = await _dbContext.SaveChangesAsync();

            AllActivityQuery query = new()
            {
            };

            // Act
            AllActivityQueryResult result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _ = result.ShouldNotBeNull();
            _ = result.AllActivityTypes.ShouldNotBeNull();
        }
    }
}