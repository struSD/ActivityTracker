using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contracts.Database;
using ActivityTracker.Domain.Database;

using MediatR;

namespace ActivityTracker.Domain.Commands
{
    public class CreateUserCommand : IRequest<CreateUserCommandResult>
    {
        public string Name { get; set; }
    }

    public class CreateUserCommandResult
    {
        public int UserId { get; set; }
    }

    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResult>
    {
        private readonly UserDbContext _dbContext;

        public CreateUserCommandHandler(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateUserCommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User user = new()
            {
                Name = request.Name
            };
            _ = await _dbContext.AddAsync(user, cancellationToken);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);
            return new CreateUserCommandResult
            {
                UserId = user.Id
            };
        }
    }
}