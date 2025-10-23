using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Coaches;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Courses;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain;

public class UnavailableFor(Id<Coach> CoachId, Id<Course> CourseId) : DomainEntity<UnavailableFor>
{
    public Id<Coach> CoachId { get; } = CoachId;
    public Id<Course> CourseId { get; } = CourseId;
}
