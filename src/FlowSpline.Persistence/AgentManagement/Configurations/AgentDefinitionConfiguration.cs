using FlowSpline.Domain.AgentManagement.Aggregates;
using FlowSpline.Domain.AgentManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowSpline.Persistence.AgentManagement.Configurations;

internal sealed class AgentDefinitionConfiguration : IEntityTypeConfiguration<AgentDefinition>
{
    public void Configure(EntityTypeBuilder<AgentDefinition> builder)
    {
        builder.ToTable("agents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.SystemPrompt)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.OwnsOne(x => x.Model, m =>
        {
            m.Property(x => x.Provider)
                .HasColumnName("provider")
                .HasMaxLength(100)
                .IsRequired();

            m.Property(x => x.Model)
                .HasColumnName("model")
                .HasMaxLength(200)
                .IsRequired();

            m.Property(x => x.Temperature)
                .HasColumnName("temperature")
                .IsRequired();

            m.Property(x => x.MaxTokens)
                .HasColumnName("max_tokens")
                .IsRequired();
        });

        builder.OwnsMany(x => x.Tools, t =>
        {
            t.ToTable("agent_tools");
            t.WithOwner().HasForeignKey("AgentId");
            t.HasKey("AgentId", nameof(Tool.Name));
            t.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Navigation(x => x.Tools)
            .HasField("_tools")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
