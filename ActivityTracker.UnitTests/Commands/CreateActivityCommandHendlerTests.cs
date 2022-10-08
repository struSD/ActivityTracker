
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contracts.Http;
using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Exceptions;
using ActivityTracker.UnitTests.Helpers;

using MediatR;

using Shouldly;

namespace ActivityTracker.UnitTests.Commands
{
    public class CreateActivityCommandHendlerTests
    {
        private readonly UserDbContext _dbContext;
        public readonly IRequestHandler<CreateActivityCommand, CreateActivityCommandResult> Handler;

        public CreateActivityCommandHendlerTests()
        {
            _dbContext = DbContextHelper.CreateTestDb();
            Handler = new CreateActivityCommandHendler(_dbContext);
        }

        [Fact]
        public async Task HandleShouldAddActivity()
        {
            // Arrange
            Contracts.Database.User user = new()
            {
                Name = Guid.NewGuid().ToString(),
                ActivityUsers = Enumerable.Empty<Contracts.Database.ActivityUser>().ToList()
            };

            _ = await _dbContext.Users.AddAsync(user);
            _ = await _dbContext.SaveChangesAsync();

            CreateActivityCommand command = new()
            {
                ActivityType = "golf",
                ActivityDuration = new Random().Next(1000, 2500),
                UserId = user.Id
            };
            // Act
            CreateActivityCommandResult result = await Handler.Handle(command, CancellationToken.None);
            // Assert
            _ = result.ActivityUser.ShouldNotBeNull();
            result.ActivityUser.ActivityId.ShouldBeGreaterThan(0);
            result.ActivityUser.ActivityDuration.ShouldBe(command.ActivityDuration);
            result.ActivityUser.ActivityType.ShouldBe(command.ActivityType);
        }

        [Fact]
        public async Task HandleShouldThrowExceptionIfNoUser()
        {
            // Arrange
            int userId = -1;


            CreateActivityCommand command = new()
            {
                ActivityType = "golf",
                ActivityDuration = new Random().Next(1000, 2500),
                UserId = userId
            };
            try
            {
                // Act
                _ = await Handler.Handle(command, CancellationToken.None);
            }
            catch (ActivityTrackerException ate) when (ate.ErrorCode == ErrorCode.UserNotFound
                && ate.Message == $"User {userId} not found")
            {

                // Assert
                //Ignore
            }
        }
    }
}