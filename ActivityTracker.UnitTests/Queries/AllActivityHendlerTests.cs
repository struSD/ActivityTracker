using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Database;
using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Exceptions;
using ActivityTracker.Domain.Queries;
using ActivityTracker.UnitTests.Helpers;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shouldly;

using static ActivityTracker.Domain.Queries.AllActivityQueryResult;

public class UserQueryHandlerTests
{
    private readonly UserDbContext _dbContext;
    private readonly IRequestHandler<AllActivityQuery, AllActivityQueryResult> _handler;

    public UserQueryHandlerTests()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new AllActivityQueryHandler(_dbContext);
    }

    [Fact]
    public async Task HandleShouldReturnAllActivity()
    {
        // Arrange
        var activity1 = new ActivityUser
        {
            ActivityType = Guid.NewGuid().ToString(),
            ActivityDateTime = DateTime.Now,
            ActivityDuration = new Random().Next(1000, 2500)
        };

        await _dbContext.AddAsync(activity1);
        await _dbContext.SaveChangesAsync();

        var activity2 = new ActivityUser
        {
            ActivityType = Guid.NewGuid().ToString(),
            ActivityDateTime = DateTime.Now,
            ActivityDuration = new Random().Next(1000, 2500)
        };
        await _dbContext.AddAsync(activity2);
        await _dbContext.SaveChangesAsync();

        var activitys = new List<ActivityUser>()
        {
            activity1,
            activity2
        };

        var query = new AllActivityQuery
        {
            ActivityId = activity1.ActivityId
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.AllActivityTypes.ShouldNotBeNull();

    }

    [Fact]
    public async Task HandleShouldThrowExceptionIfNoAllActivity()
    {
        // Arrange
        var activityId = -1;
        var query = new AllActivityQuery
        {
            ActivityId = activityId
        };

        try
        {
            // Act
            await _handler.Handle(query, CancellationToken.None);
        }
        catch (ActivityTrackerException ate) when (ate.ErrorCode == System.ComponentModel.DataAnnotations.ErrorCode.ActivityNotFound
            && ate.Message == $"Activity {activityId} not found")
        {
            // Assert
            // ignore
        }
    }
}