using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Coaches.InvalidationReasons;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Coaches;

public record CoachEmail : DefaultString<CoachEmailCanNotBeEmpty, CoachEmailCanNotBeTooLong>
{
    public CoachEmail(string value) : base(value) { }
    protected CoachEmail() { }
    public static CoachEmail Empty => new();
}
