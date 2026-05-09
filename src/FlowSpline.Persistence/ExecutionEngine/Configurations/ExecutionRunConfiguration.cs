using FlowSpline.Domain.ExecutionEngine.Aggregates;
using FlowSpline.Domain.ExecutionEngine.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowSpline.Persistence.ExecutionEngine.Configurations;

internal sealed class ExecutionRunConfiguration : IEntityTypeConfiguration<ExecutionRun>
{
    public void Configure(EntityTypeBuilder<ExecutionRun> builder)
    {
        builder.ToTable("execution_runs");

        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Context, c =>
        {
            c.Property(x => x.AgentId)
                .HasColumnName("agent_id")
                .IsRequired();

            c.Property(x => x.Input)
                .HasColumnName("input")
                .HasColumnType("text")
                .IsRequired();

            c.Property(x => x.SessionId)
                .HasColumnName("session_id")
                .IsRequired();

            c.HasIndex(nameof(FlowSpline.Domain.ExecutionEngine.ValueObjects.RunContext.AgentId),
                       nameof(FlowSpline.Domain.ExecutionEngine.ValueObjects.RunContext.SessionId));
        });

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.StartedAt)
            .HasColumnType("timestamptz");

        builder.Property(x => x.CompletedAt)
            .HasColumnType("timestamptz");

        builder.Property(x => x.FailureReason)
            .HasColumnType("text");

        builder.Property(x => x.RetryCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasIndex(x => x.Status);
    }
}
