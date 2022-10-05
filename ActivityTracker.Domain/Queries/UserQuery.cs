using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Database;
using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Exceptions;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Domain.Queries;

public class UserQuery : IRequest<UserQueryResult>
{
    public int UserId { get; set; }
}

public class UserQueryResult
{
    public User User { get; set; }
}

public class UserQueryHandler : IRequestHandler<UserQuery, UserQueryResult>
{
    private readonly UserDbContext _dbContext;

    public UserQueryHandler(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserQueryResult> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Include(u => u.ActivityUsers).SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            throw new UserException(ErrorCode.UserNotFound, $"User{request.UserId} not found");
        }
        return new UserQueryResult
        {
            User = user
        };
    }
}