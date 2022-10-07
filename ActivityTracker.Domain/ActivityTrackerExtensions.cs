using System;

using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Database;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityTracker.Domain;
public static class ActivityTrackerExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services,
    Action<IServiceProvider, DbContextOptionsBuilder> dbOptionsAction)
    {
        services.AddMediatR(typeof(CreateUserCommand));
        services.AddDbContext<UserDbContext>(dbOptionsAction);
        return services;
    }
}