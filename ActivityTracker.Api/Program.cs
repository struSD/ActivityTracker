using System;

using ActivityTracker.Api.Configuration;
using ActivityTracker.Domain;
using ActivityTracker.Domain.Commands;
using ActivityTracker.Domain.Database;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().AddNpgSql((sp) =>
{
    var configuration = sp.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
    return configuration.CurrentValue.ConnectionString;
},
    timeout: TimeSpan.FromSeconds(5));

builder.Services.Configure<AppConfiguration>(builder.Configuration);

builder.Services.AddDomainServices((sp, optionts) =>
{
    var configuration = sp.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
    optionts.UseNpgsql(configuration.CurrentValue.ConnectionString);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
