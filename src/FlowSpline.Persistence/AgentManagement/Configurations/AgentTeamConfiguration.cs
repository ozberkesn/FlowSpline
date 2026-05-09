using System.Text.Json;
using FlowSpline.Domain.AgentManagement.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlowSpline.Persistence.AgentManagement.Configurations;

internal sealed class AgentTeamConfiguration : IEntityTypeConfiguration<AgentTeam>
{
    public void Configure(EntityTypeBuilder<AgentTeam> builder)
    {
        builder.ToTable("agent_teams");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.SupervisorId)
            .IsRequired();

        var guidSetConverter = new ValueConverter<HashSet<Guid>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<HashSet<Guid>>(v, (JsonSerializerOptions?)null) ?? new HashSet<Guid>()
        );

        builder.Property<HashSet<Guid>>("_memberIds")
            .HasField("_memberIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion(guidSetConverter)
            .HasColumnName("member_ids")
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
