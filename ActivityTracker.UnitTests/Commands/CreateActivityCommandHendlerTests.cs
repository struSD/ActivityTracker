
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Database;
using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Exceptions;
using ActivityTracker.UnitTests.Helpers;

using MediatR;

using Shouldly;

namespace ActivityTracker.UnitTests.Commands;
public class CreateActivityCommandHendlerTests
{
    private readonly UserDbContext _dbContext;
    public readonly IRequestHandler<CreateActivityCommand, CreateActivityCommandResult> _handler;
    public CreateActivityCommandHendlerTests()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new CreateActivityCommandHendler(_dbContext);
    }

    [Fact]
    public async Task HandleShouldAddActivity()
    {
        

        // Arrange
        var user = new User
        {
            Name = Guid.NewGuid().ToString(),
            ActivityUsers = Enumerable.Empty<ActivityUser>().ToList()
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        var command = new CreateActivityCommand
        {
            ActivityType = "golf",
            ActivityDuration = new Random().Next(1000, 2500),
            UserId = user.Id
        };
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.ActivityUser.ShouldNotBeNull();
        result.ActivityUser.ActivityId.ShouldBeGreaterThan(0);
        result.ActivityUser.ActivityDuration.ShouldBe(command.ActivityDuration);
        result.ActivityUser.ActivityType.ShouldBe(command.ActivityType);
        result.ActivityUser.UserId.ShouldBe(command.UserId);
    }

    [Fact]
    public async Task HandleShouldThrowExceptionIfNoUser()
    {
        // Arrange
        var userId = -1;


        var command = new CreateActivityCommand
        {
            ActivityType = "golf",
            ActivityDuration = new Random().Next(1000, 2500),
            UserId = userId
        };
        try
        {
            // Act
            await _handler.Handle(command, CancellationToken.None);
        }
        catch (ActivityTrackerException ate) when (ate.ErrorCode == ErrorCode.UserNotFound
            && ate.Message == $"User {userId} not found")
        {

            // Assert
            //Ignore
        }



    }


}