using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Domain.Database;
using ActivityTracker.Domain.Exceptions;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Domain.Queries;

public class AllActivityQuery : IRequest<AllActivityQueryResult>
{
    public int ActivityId { get; set; }
}

public class AllActivityQueryResult
{

    public List<ActivityTracker.Contract.Database.ActivityUser> AllActivityTypes { get; set; }

    internal class AllActivityQueryHandler : IRequestHandler<AllActivityQuery, AllActivityQueryResult>
    {
        private readonly UserDbContext _dbContext;

        public AllActivityQueryHandler(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AllActivityQueryResult> Handle(AllActivityQuery request, CancellationToken cancellationToken)
        {
            var activity = await _dbContext.ActivityUsers.ToListAsync(cancellationToken);
            return new AllActivityQueryResult
            {
                AllActivityTypes = activity
            };
        }

    }
}