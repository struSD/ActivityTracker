using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contracts.Database;
using ActivityTracker.Domain.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Domain.Queries
{
    public class AllActivityQuery : IRequest<AllActivityQueryResult>
    {
    }

    public class AllActivityQueryResult
    {
        public List<ActivityUser> AllActivityTypes { get; set; }
    }

    internal class AllActivityQueryHandler : IRequestHandler<AllActivityQuery, AllActivityQueryResult>
    {
        private readonly UserDbContext _dbContext;

        public AllActivityQueryHandler(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AllActivityQueryResult> Handle(AllActivityQuery request, CancellationToken cancellationToken)
        {
            List<ActivityUser> activity = await _dbContext.ActivityUsers.ToListAsync(cancellationToken);
            return new AllActivityQueryResult
            {
                AllActivityTypes = activity
            };
        }
    }
}