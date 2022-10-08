using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ActivityTracker.Contract.Http;
using ActivityTracker.Domain.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;

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
    public ActivityUser[] ActivityUser { get; set; }
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
        var act = await _dbContext.ActivityUsers.Where(activity => activity.ActivityType ==  request.Name).ToArrayAsync();
        return new GetProductByIdResult
        {
            ActivityUser = act.Select(x=>new ActivityUser
            {
                ActivityDateTime = x.ActivityDateTime,
                ActivityDuration = x.ActivityDuration,
                ActivityType = x.ActivityType
            }).ToArray()
        };
    }
}