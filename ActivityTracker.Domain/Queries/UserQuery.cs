using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contracts.Http;
using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Exceptions;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Domain.Queries
{
    public class UserQuery : IRequest<UserQueryResult>
    {
        public int UserId { get; set; }
    }

    public class UserQueryResult
    {
        public Contracts.Database.User User { get; set; }
    }

    internal class UserQueryHandler : IRequestHandler<UserQuery, UserQueryResult>
    {
        private readonly UserDbContext _dbContext;

        public UserQueryHandler(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserQueryResult> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            Contracts.Database.User user = await _dbContext.Users.Include(u => u.ActivityUsers).SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            return user == null
                ? throw new ActivityTrackerException(ErrorCode.UserNotFound, $"User{request.UserId} not found")
                : new UserQueryResult
                {
                    User = user
                };
        }
    }
}