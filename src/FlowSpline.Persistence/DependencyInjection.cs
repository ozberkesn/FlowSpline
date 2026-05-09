using FlowSpline.Application.AgentManagement.Repositories;
using FlowSpline.Application.ExecutionEngine.Repositories;
using FlowSpline.Application.Memory.Repositories;
using FlowSpline.Application.ToolRuntime.Repositories;
using FlowSpline.Persistence.AgentManagement;
using FlowSpline.Persistence.ExecutionEngine;
using FlowSpline.Persistence.Memory;
using FlowSpline.Persistence.ToolRuntime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FlowSpline.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FlowSplineDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));

        services.AddScoped<IAgentRepository, AgentRepository>();
        services.AddScoped<IAgentTeamRepository, AgentTeamRepository>();
        services.AddScoped<IExecutionRunRepository, ExecutionRunRepository>();
        services.AddScoped<IToolDefinitionRepository, ToolDefinitionRepository>();
        services.AddScoped<IMemoryEntryRepository, MemoryEntryRepository>();

        return services;
    }
}
