using System.Security.Claims;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Accounts;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Coaches;
using QuickPulse.Show;

namespace QuickFuzzr.Reactor.Tests;

public class GenericIdentityTests
{
    private readonly Actor Admin = Actor.From([new(ClaimTypes.Role, ApplicationUser.AdminRole)]);

    [Fact]
    public void CanFuzz()
    {
        var fuzzr =
            from _ in Configr<Id<Coach>>.Construct(() => Id<Coach>.From(42))
            from coach in Fuzzr.One(() => Coach.Create(Admin, "name", "email"))
            select coach;
        Assert.Equal(42, fuzzr.Generate().Id.Value);
    }

    [Fact]
    public void Countering()
    {
        var fuzzr =
            from coachId in Fuzzr.Counter("coach-id")
            from _1 in Configr<Id<Coach>>.Construct(() => Id<Coach>.From(coachId))
            from _2 in Configr<Coach>.Construct(() => Coach.Create(Admin, "name", "email"))
            from coach in Fuzzr.One<Coach>()
            select coach;
        var coaches = fuzzr.Many(3).Generate().ToList();
        Assert.Equal(1, coaches[0].Id.Value);
        Assert.Equal(2, coaches[1].Id.Value);
        Assert.Equal(3, coaches[2].Id.Value);
    }
}