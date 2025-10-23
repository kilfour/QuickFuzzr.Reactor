using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Coaches.InvalidationReasons;

public class CoachAlreadyHasSkill(string skill) : DomainException(skill) { }