using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Accounts.InvalidationReasons;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Accounts;

public record ApplicationUserName : DefaultString<ApplicationUserNameCanNotBeEmpty, ApplicationUserNameCanNotBeTooLong>
{
    public ApplicationUserName(string value) : base(value) { }
    protected ApplicationUserName() { }
    public static ApplicationUserName Empty => new();
}
