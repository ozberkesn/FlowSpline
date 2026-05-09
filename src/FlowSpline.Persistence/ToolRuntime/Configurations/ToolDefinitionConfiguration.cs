using FlowSpline.Domain.ToolRuntime.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowSpline.Persistence.ToolRuntime.Configurations;

internal sealed class ToolDefinitionConfiguration : IEntityTypeConfiguration<ToolDefinition>
{
    public void Configure(EntityTypeBuilder<ToolDefinition> builder)
    {
        builder.ToTable("tool_definitions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.IsEnabled)
            .IsRequired();

        builder.OwnsOne(x => x.Schema, s =>
        {
            s.Property(x => x.InputSchema)
                .HasColumnName("input_schema")
                .HasColumnType("text");

            s.Property(x => x.OutputSchema)
                .HasColumnName("output_schema")
                .HasColumnType("text");
        });
    }
}
