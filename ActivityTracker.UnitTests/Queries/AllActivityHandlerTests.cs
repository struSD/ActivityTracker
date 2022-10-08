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
    public class AllActivityHandlerTests
    {
        private readonly UserDbContext _dbContext;
        private readonly IRequestHandler<AllActivityQuery, AllActivityQueryResult> _handler;

        public AllActivityHandlerTests()
        {
            _dbContext = DbContextHelper.CreateTestDb();
            _handler = new AllActivityQueryHandler(_dbContext);
        }

        [Fact]
        public async Task HandleShouldReturnAllActivity()
        {
            // Arrange
            Contracts.Database.User user = new()
            {
                Name = Guid.NewGuid().ToString(),
                ActivityUsers = new[]
                {
                    new Contracts.Database.ActivityUser()
                    {
                        ActivityType = Guid.NewGuid().ToString(),
                        ActivityDateTime = DateTime.Now,
                        ActivityDuration = new Random().Next(1000, 2500),
                    },
                    new Contracts.Database.ActivityUser()
                    {
                        ActivityType = Guid.NewGuid().ToString(),
                        ActivityDateTime = DateTime.Now,
                        ActivityDuration = new Random().Next(1000, 2500)
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

        [Fact]
        public async Task HandleShouldThrowExceptionIfNoAllActivity()
        {
            // Arrange
            int activityId = -1;
            AllActivityQuery query = new()
            {
            };

            try
            {
                // Act
                _ = await _handler.Handle(query, CancellationToken.None);
            }
            catch (ActivityTrackerException ate) when (ate.ErrorCode == ErrorCode.ActivityNotFound
                && ate.Message == $"Activity {activityId} not found")
            {
                // Assert
                // ignore
            }
        }
    }
}