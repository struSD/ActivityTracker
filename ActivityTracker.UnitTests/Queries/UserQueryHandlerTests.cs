
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Database;
using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Queries;
using ActivityTracker.UnitTests.Helpers;

using MediatR;

using Shouldly;

namespace ActivityTracker.UnitTests.Queries;

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
        var user = new User
        {
            Name = Guid.NewGuid().ToString(),
            ActivityUsers = new[]
            {
                new ActivityUser
                {
                    ActivityType = ActivityType.Golf,
                    ActivityDateTime = new DateTime(2000, 01, 01),
                    ActivityDuration = 900
                },
                new ActivityUser
                {
                    ActivityType = ActivityType.Hiking,
                    ActivityDateTime = new DateTime(2010, 10, 10),
                    ActivityDuration = 500
                }
            }
        };

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var query = new UserQuery
        {
            UserId = user.Id
        };
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.ShouldNotBeNull();
        result.User.ShouldNotBeNull();
        result.User.Id.ShouldBe(user.Id);
        result.User.Name.ShouldBe(user.Name);
        result.User.ActivityUsers.ShouldNotBeEmpty();
    }

}

