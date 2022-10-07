using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Database;
using ActivityTracker.Domain.Database;

using MediatR;

namespace ActivityTracker.Domain.Commands;

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
        var user = new User
        {
            Name = request.Name
        };
        await _dbContext.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new CreateUserCommandResult
        {
            UserId = user.Id
        };
    }
}