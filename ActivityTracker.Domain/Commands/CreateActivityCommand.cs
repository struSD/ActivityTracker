using System;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contracts.Http;
using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Exceptions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using ActivityDatabaseUser = ActivityTracker.Contracts.Database.ActivityUser;

namespace ActivityTracker.Domain.Commands
{
    public class CreateActivityCommand : IRequest<CreateActivityCommandResult>
    {
        public string ActivityType { get; init; }
        public int ActivityDuration { get; init; }
        public int UserId { get; init; }
    }

    public class CreateActivityCommandResult
    {
        public ActivityUser ActivityUser { get; set; }
    }

    public class CreateActivityCommandHendler : IRequestHandler<CreateActivityCommand, CreateActivityCommandResult>
    {
        private readonly UserDbContext _dbContext;

        public CreateActivityCommandHendler(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateActivityCommandResult> Handle(CreateActivityCommand request,
            CancellationToken cancellationToken)
        {
            Contracts.Database.User user = await _dbContext.Users.Include(u => u.ActivityUsers).SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                throw new ActivityTrackerException(ErrorCode.UserNotFound, $"User {request.UserId} not found");
            }

            ActivityDatabaseUser activity = new()
            {
                ActivityType = request.ActivityType,
                ActivityDateTime = DateTime.UtcNow,
                ActivityDuration = request.ActivityDuration,
                UserId = request.UserId
            };
            _ = await _dbContext.ActivityUsers.AddAsync(activity, cancellationToken);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateActivityCommandResult
            {
                ActivityUser = new ActivityUser
                {
                    ActivityDateTime = activity.ActivityDateTime,
                    ActivityDuration = activity.ActivityDuration,
                    ActivityId = activity.ActivityId,
                    ActivityType = activity.ActivityType,
                }
            };
        }
    }
}