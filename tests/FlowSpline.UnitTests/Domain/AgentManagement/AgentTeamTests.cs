using FlowSpline.Domain.AgentManagement.Aggregates;

namespace FlowSpline.UnitTests.Domain.AgentManagement;

public class AgentTeamTests
{
    [Fact]
    public void CreateTeam_WithValidData_ShouldCreateTeam()
    {
        var supervisorId = Guid.NewGuid();

        var team = new AgentTeam("Support Team", supervisorId);

        Assert.NotEqual(Guid.Empty, team.Id);
        Assert.Equal("Support Team", team.Name);
        Assert.Equal(supervisorId, team.SupervisorId);
        Assert.Empty(team.MemberIds);
    }

    [Fact]
    public void CreateTeam_WithEmptySupervisor_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new AgentTeam("Support Team", Guid.Empty));
    }

    [Fact]
    public void CreateTeam_WithShortName_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new AgentTeam("AB", Guid.NewGuid()));
    }

    [Fact]
    public void AddMember_WithNewAgent_ShouldAddMember()
    {
        var team = CreateTeam();
        var agentId = Guid.NewGuid();

        team.AddMember(agentId);

        Assert.Contains(agentId, team.MemberIds);
    }

    [Fact]
    public void AddMember_WithSupervisor_ShouldThrow()
    {
        var supervisorId = Guid.NewGuid();
        var team = new AgentTeam("Support Team", supervisorId);

        Assert.Throws<InvalidOperationException>(() => team.AddMember(supervisorId));
    }

    [Fact]
    public void AddMember_Duplicate_ShouldThrow()
    {
        var team = CreateTeam();
        var agentId = Guid.NewGuid();
        team.AddMember(agentId);

        Assert.Throws<InvalidOperationException>(() => team.AddMember(agentId));
    }

    [Fact]
    public void RemoveMember_WithExistingMember_ShouldRemove()
    {
        var team = CreateTeam();
        var agentId = Guid.NewGuid();
        team.AddMember(agentId);

        team.RemoveMember(agentId);

        Assert.DoesNotContain(agentId, team.MemberIds);
    }

    [Fact]
    public void RemoveMember_WithSupervisor_ShouldThrow()
    {
        var supervisorId = Guid.NewGuid();
        var team = new AgentTeam("Support Team", supervisorId);

        Assert.Throws<InvalidOperationException>(() => team.RemoveMember(supervisorId));
    }

    [Fact]
    public void RemoveMember_WithEmptyGuid_ShouldThrow()
    {
        var team = CreateTeam();

        Assert.Throws<ArgumentException>(() => team.RemoveMember(Guid.Empty));
    }

    [Fact]
    public void ChangeSupervisor_WithExistingMember_ShouldChangeSupervisor()
    {
        var team = CreateTeam();
        var newSupervisorId = Guid.NewGuid();
        team.AddMember(newSupervisorId);

        team.ChangeSupervisor(newSupervisorId);

        Assert.Equal(newSupervisorId, team.SupervisorId);
    }

    [Fact]
    public void ChangeSupervisor_WithNonMember_ShouldThrow()
    {
        var team = CreateTeam();

        Assert.Throws<InvalidOperationException>(() => team.ChangeSupervisor(Guid.NewGuid()));
    }

    [Fact]
    public void CreateTeam_ShouldRaiseDomainEvent()
    {
        var team = CreateTeam();

        Assert.Single(team.DomainEvents);
    }

    private static AgentTeam CreateTeam() => new("Support Team", Guid.NewGuid());
}
