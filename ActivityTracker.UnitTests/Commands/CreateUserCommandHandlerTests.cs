
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Database;
using ActivityTracker.UnitTests.Helpers;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Shouldly;

namespace ActivityTracker.UnitTests.Commands;

public class CreateActivityTrackerCommandHandlerTests : IDisposable
{
    private readonly UserDbContext _dbContext;
    public readonly IRequestHandler<CreateUserCommand, CreateUserCommandResult> _handler;
    public CreateActivityTrackerCommandHandlerTests()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new CreateUserCommandHandler(_dbContext);
    }

    [Fact]
    public async Task HandleShouldCreateEmptyActivityTracker()
    {
        // Arrange
        var userName = Guid.NewGuid().ToString();
        var command = new CreateUserCommand
        {
            Name = userName
        };
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.UserId.ShouldBeGreaterThan(0);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}