
using System;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contracts.Database;
using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Queries;
using ActivityTracker.UnitTests.Helpers;

using MediatR;

using Shouldly;

namespace ActivityTracker.UnitTests.Queries
{
    public class UserQueryHandlerTests
    {
        private readonly UserDbContext _dbContext;
        private readonly IRequestHandler<UserQuery, UserQueryResult> _handler;

        public UserQueryHandlerTests()
        {
            _dbContext = DbContextHelper.CreateTestDb();
            _handler = new UserQueryHandler(_dbContext);
        }

        [Fact]
        public async Task HandlerShouldReturnUser()
        {

            //Arrenge
            User user = new()
            {
                Name = Guid.NewGuid().ToString(),
                ActivityUsers = new[]
                {
                new ActivityUser
                {
                    ActivityType = "golf",
                    ActivityDateTime = new DateTime(2000, 01, 01),
                    ActivityDuration = new Random().Next(1000, 2500)
                },
                new ActivityUser
                {
                    ActivityType = "run",
                    ActivityDateTime = new DateTime(2010, 10, 10),
                    ActivityDuration = new Random().Next(1000, 2500)
                }
            }
            };

            _ = await _dbContext.AddAsync(user);
            _ = await _dbContext.SaveChangesAsync();
            UserQuery query = new()
            {
                UserId = user.Id
            };
            //Act
            UserQueryResult result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            _ = result.ShouldNotBeNull();
            _ = result.User.ShouldNotBeNull();
            result.User.Id.ShouldBe(user.Id);
            result.User.Name.ShouldBe(user.Name);
            result.User.ActivityUsers.ShouldNotBeEmpty();
        }
    }
}