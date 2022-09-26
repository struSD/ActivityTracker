
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shouldly;

namespace ActivityTracker.UnitTests.Commands;

public class CreateActivityTrackerCommandHandlerTests
{
    private readonly ActivityTrackerDbContext _dbContext;
    public readonly IRequestHandler<CreateActivityTrackerCommand, CreateActivityTrackerCommandResult> _handler;
    public CreateActivityTrackerCommandHandlerTests()
    {
        var tempFile = Path.GetTempFileName();

        var options = new DbContextOptionsBuilder<ActivityTrackerDbContext>()
            .UseSqlite($"Data Source={tempFile};")
            .Options;

        _dbContext = new ActivityTrackerDbContext(options);
        _dbContext.Database.Migrate();
        _handler = new CreateActivityTrackerCommandHandler(_dbContext); 
    }

    [Fact]
    public async Task HandleShouldCreateEmptyActivityTracker()
    {
        // Arrange
        var activityTrackerName = Guid.NewGuid().ToString();
        var command = new CreateActivityTrackerCommand
        {
            Name = activityTrackerName
        };
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.ActivityTrackerId.ShouldBeGreaterThan(0);
    }
}