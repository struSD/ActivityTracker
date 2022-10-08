
using System;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Database;
using ActivityTracker.UnitTests.Helpers;

using MediatR;

using Shouldly;

namespace ActivityTracker.UnitTests.Commands
{
    public class CreateUserCommandHandlerTests : IDisposable
    {
        private readonly UserDbContext _dbContext;
        public readonly IRequestHandler<CreateUserCommand, CreateUserCommandResult> Handler;

        public CreateUserCommandHandlerTests()
        {
            _dbContext = DbContextHelper.CreateTestDb();
            Handler = new CreateUserCommandHandler(_dbContext);
        }

        [Fact]
        public async Task HandleShouldCreateEmptyUser()
        {
            // Arrange
            string userName = Guid.NewGuid().ToString();
            CreateUserCommand command = new()
            {
                Name = userName
            };
            // Act
            CreateUserCommandResult result = await Handler.Handle(command, CancellationToken.None);
            // Assert
            _ = result.ShouldNotBeNull();
            result.UserId.ShouldBeGreaterThan(0);
        }

        public void Dispose()
        {
            _ = _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}