using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Coaches.InvalidationReasons;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Coaches;

public record CoachName : DefaultString<CoachNameCanNotBeEmpty, CoachNameCanNotBeTooLong>
{
    public CoachName(string value) : base(value) { }
    protected CoachName() { }
    public static CoachName Empty => new();
}
