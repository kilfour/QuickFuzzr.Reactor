using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Accounts.InvalidationReasons;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Accounts;

public record ApplicationUserEmail : DefaultString<ApplicationUserEmailCanNotBeEmpty, ApplicationUserEmailCanNotBeTooLong>
{
    public ApplicationUserEmail(string value) : base(value) { }
    protected ApplicationUserEmail() { }
    public static ApplicationUserEmail Empty => new();
}
