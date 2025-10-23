
using Bogus;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Accounts;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Coaches;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Courses;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Courses.TimeSlots;

namespace QuickFuzzr.Examples;

// Adjust namespace to your project
public static class BogusDomainSeed
{
    // public void Go()
    // {
    //     var admin = AdminActor();

    //     var coaches = GenerateCoaches(count: 10, seed: 2025);
    //     var courses = GenerateCourses(count: 6, seed: 2026);
    // }
    // Use a system/admin actor to satisfy the domain guards
    public static Actor AdminActor() => Actor.SystemActor();

    // Small curated pools — tweak freely
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

    // ------------------------------------------------------------
    // Coaches
    // ------------------------------------------------------------
    public static IReadOnlyList<Coach> GenerateCoaches(int count, int seed = 42)
    {
        var admin = AdminActor();
        Randomizer.Seed = new Random(seed);

        var faker = new Faker("en");

        var coaches = new List<Coach>(capacity: count);
        for (int i = 0; i < count; i++)
        {
            var name = $"{faker.Name.FirstName()} {faker.Name.LastName()}";
            var email = faker.Internet.Email(firstName: name.Split(' ').First(), lastName: name.Split(' ').Last());

            var coach = Coach.Create(admin, name, email);

            // 1..5 unique skills
            var skills = faker.PickRandom(SkillPool, faker.Random.Int(1, 5)).Distinct().ToList();
            coach.UpdateSkills(admin, skills);

            coaches.Add(coach);
        }

        return coaches;
    }

    // ------------------------------------------------------------
    // Courses
    // ------------------------------------------------------------
    public static IReadOnlyList<Course> GenerateCourses(int count, int seed = 1337)
    {
        var admin = AdminActor();
        Randomizer.Seed = new Random(seed);

        var faker = new Faker("en");
        var courses = new List<Course>(capacity: count);

        for (int i = 0; i < count; i++)
        {
            var title = faker.PickRandom(CourseTitles);

            // Period: pick a start within the next 30 days, duration 2–6 weeks
            var start = DateOnly.FromDateTime(faker.Date.SoonOffset(30).Date);
            var end = start.AddDays(faker.Random.Int(14, 42));

            var course = Course.Create(admin, title, start, end);

            // Required skills: 1..4
            var reqSkills = faker.PickRandom(SkillPool, faker.Random.Int(1, 4)).Distinct().ToList();
            course.UpdateRequiredSkills(admin, reqSkills);

            // Non-overlapping time slots within 09–17h, min length 1h
            var slots = BuildNonOverlappingSlots(faker, start, end);
            course.UpdateTimeSlots<(CourseDay Day, int Start, int End)>(
                admin,
                slots,
                s => (s.Day, s.Start, s.End)
            );

            // Optional: confirm course (requires at least one slot)
            course.Confirm(admin);

            courses.Add(course);
        }

        return courses;
    }

    // Helper to build a handful of non-overlapping slots within the course period
    private static List<(CourseDay Day, int Start, int End)> BuildNonOverlappingSlots(Faker f, DateOnly start, DateOnly end)
    {
        var daysInPeriod = Enumerable.Range(0, end.DayNumber - start.DayNumber + 1)
            .Select(offset => start.AddDays(offset))
            .Where(d => d.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
            .ToList();

        if (daysInPeriod.Count == 0)
            daysInPeriod.Add(start);

        var pick = f.Random.Int(1, Math.Min(4, daysInPeriod.Count));
        var chosenDates = f.PickRandom(daysInPeriod, pick).Distinct();

        var results = new List<(CourseDay Day, int Start, int End)>();

        foreach (var date in chosenDates)
        {
            var maxStart = 16;
            var startHour = f.Random.Int(9, maxStart);
            var length = f.Random.Int(1, Math.Min(3, 17 - startHour));
            var endHour = startHour + length;

            var day = (CourseDay)date.DayOfWeek;
            results.Add((day, startHour, endHour));
        }

        // Value tuples have structural equality — this dedups nicely
        return [.. results.Distinct()];
    }
}
