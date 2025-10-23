using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Courses.InvalidationReasons;

public class CourseAlreadyHasSkill(string skill) : DomainException(skill) { }
