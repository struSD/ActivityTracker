using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Domain.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Domain.Queries
{
    public class GetProductByIdQuery : IRequest<GetProductByIdResult>
    {
        public string Name { get; }

        public GetProductByIdQuery(string name)
        {
            Name = name;
        }
    }

    public class GetProductByIdResult
    {
        public Contracts.Http.ActivityUser[] ActivityUser { get; set; }
    }

    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        private readonly UserDbContext _dbContext;

        public GetProductByIdHandler(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            Contracts.Database.ActivityUser[] act = await _dbContext.ActivityUsers.Where(activity => activity.ActivityType == request.Name).ToArrayAsync(cancellationToken);
            return new GetProductByIdResult
            {
                ActivityUser = act.Select(x => new Contracts.Http.ActivityUser
                {
                    ActivityDateTime = x.ActivityDateTime,
                    ActivityDuration = x.ActivityDuration,
                    ActivityType = x.ActivityType
                }).ToArray()
            };
        }
    }
}