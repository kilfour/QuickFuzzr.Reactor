using System.Security.Claims;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Accounts;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Coaches;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Courses;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Courses.TimeSlots;
using QuickPulse.Instruments;
using QuickPulse.Show;
using WibblyWobbly;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Skills;
using System.Collections.ObjectModel;

namespace QuickFuzzr.Reactor.Tests;

public class DomainTests
{
    private static readonly Actor Admin = Actor.From([new(ClaimTypes.Role, ApplicationUser.AdminRole)]);

    private static readonly string[] SkillPool =
    {
        "TDD", "Refactoring", "C#", "ASP.NET", "EF Core", "SQL",
        "DomainDrivenDesign", "UnitTesting", "Git", "CI/CD",
        "JavaScript", "React", "Elm", "Architecture"
    };

    private static readonly string[] CourseTitles =
    {
        "Intro to C#", "Advanced C# Patterns", "WebAPI & REST",
        "EF Core In Depth", "Domain Modelling", "Testing Workshop",
        "Frontend for Backenders", "CI/CD Fundamentals"
    };

    [Fact]
    public void BuildDomain()
    {
        var logFile = "horses-for-courses.log";
        File.Delete(Path.Combine(SolutionLocator.FindSolutionRoot()!, logFile));
        var fuzzr =
            from _ in Configr.IgnoreAll()
            from coaches in CoachFuzzr.Many(10)
            from courses in CourseFuzzr(coaches).Many(3)
            select (courses, coaches);
        var result = fuzzr.Generate();
        Please.AllowMe()
            .ToReplace<Skill>(a => $"\"{a.Value}\"")
            .ToInline<ReadOnlyCollection<Skill>>()
            .ToReplace<Id<Coach>>(a => $"\"{a.Value}\"")
            .ToSelfReference<Coach>(a => $"<cycle => Coach.Id: {a.Id.Value}>")
            .ToReplace<CoachName>(a => $"\"{a.Value}\"")
            .ToReplace<CoachEmail>(a => $"\"{a.Value}\"")
            .ToReplace<Id<Course>>(a => $"\"{a.Value}\"")
            .ToSelfReference<Course>(a => $"<cycle => Course.Id: {a.Id.Value}>")
            .ToReplace<CourseName>(a => $"\"{a.Value}\"")
            .ToInline<TimeSlot>()
            .IntroduceThis(result)
            .PulseToLog(logFile);
    }

    private static FuzzrOf<int> GenericId<T>()
        where T : DomainEntity<T> =>
        from id in Fuzzr.Counter($"{typeof(T).Name.ToLower()}-id")
        from _ in Configr<T>.Property(a => a.Id, Id<T>.From(id))
        select id;

    private static readonly FuzzrOf<Coach> CoachFuzzr =
        from coachId in GenericId<Coach>()
        from person in Fuze.Person
        from coach in Fuzzr.One(() => Coach.Create(Admin, person.FullName, person.Email))
        from skills in Fuzzr.OneOf(SkillPool).Unique(coachId).Many(3, 5)
        let _ = coach.UpdateSkills(Admin, skills)
        select coach;

    private static readonly FuzzrOf<(DateOnly Start, DateOnly End)> PeriodFuzzr =
        from start in Fuzzr.DateOnly(1.January(2025), 31.December(2025))
        from length in Fuzzr.Int(0, 120)
        from end in Fuzzr.DateOnly(start, start.AddDays(length))
        select (start, end);

    private static FuzzrOf<Course> CourseFuzzr(IEnumerable<Coach> coaches) =>
        from courseId in GenericId<Course>()
        from courseTitle in Fuzzr.OneOf(CourseTitles)
        from period in PeriodFuzzr
        from course in Fuzzr.One(() => Course.Create(Admin, courseTitle, period.Start, period.End))
        from requiredSkills in Fuzzr.OneOf(SkillPool).Many(1)
        let _1 = course.UpdateRequiredSkills(Admin, requiredSkills)
        from timeslots in TimeslotGeneratorFor(courseId).Many(1, 3)
        let _2 = course.UpdateTimeSlots(Admin, [.. timeslots], a => a)
        let _3 = course.Confirm(Admin)
        from coachToAssign in Fuzzr.OneOfOrDefault(
            coaches.Where(a => a.IsSuitableFor(course) && a.IsAvailableFor(course)))
        select coachToAssign == null ? course : course.AssignCoach(Admin, coachToAssign);

    private static FuzzrOf<(CourseDay Day, int Start, int End)> TimeslotGeneratorFor(int key) =>
        from start in Fuzzr.Int(9, 17)
        from end in Fuzzr.Int(start + 1, 18)
        from day in Fuzzr.Enum<CourseDay>().Unique($"day-{key}")
        select (day, start, end);
}