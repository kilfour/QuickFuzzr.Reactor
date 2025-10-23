using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Courses.InvalidationReasons;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Courses;

public record CourseName : DefaultString<CourseNameCanNotBeEmpty, CourseNameCanNotBeTooLong>
{
    public CourseName(string value) : base(value) { }
    protected CourseName() { }
    public static CourseName Empty => new();
}
