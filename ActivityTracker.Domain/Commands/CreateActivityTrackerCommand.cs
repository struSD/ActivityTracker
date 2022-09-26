using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Database;
using ActivityTracker.Domain.Database;

using MediatR;

namespace ActivityTracker.Domain.Commands;

public class CreateActivityTrackerCommand : IRequest<CreateActivityTrackerCommandResult>
{
    public string Name { get; set; }
}

public class CreateActivityTrackerCommandResult
{
    public int ActivityTrackerId { get; set; }
}
public class CreateActivityTrackerCommandHandler : IRequestHandler<CreateActivityTrackerCommand, CreateActivityTrackerCommandResult>
{
    private readonly ActivityTrackerDbContext _dbContext;

    public CreateActivityTrackerCommandHandler(ActivityTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateActivityTrackerCommandResult> Handle(CreateActivityTrackerCommand request, CancellationToken cancellationToken)
    {
        var activityTracker = new User
        {
            Name = request.Name
        };
        await _dbContext.AddAsync(activityTracker, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new CreateActivityTrackerCommandResult
        {
            ActivityTrackerId = activityTracker.Id
        };
    }
}