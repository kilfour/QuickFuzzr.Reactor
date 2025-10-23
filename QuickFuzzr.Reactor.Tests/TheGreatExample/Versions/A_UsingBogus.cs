using Bogus;
using QuickFuzzr.Reactor.Tests._Tools.Models.GreatExample;
using QuickPulse.Explains;
using QuickPulse.Instruments;
using QuickPulse.Show;

namespace QuickFuzzr.Reactor.Tests.TheGreatExample.Versions;

[DocFile]
public class A_UsingBogus
{
    const string logFile = "./QuickFuzzr.Reactor.Tests/TheGreatExample/Versions/using-bogus-result.txt";

    [Fact]
    [DocHeader("The Faker")]
    [DocExample(typeof(A_UsingBogus), nameof(TheFaker))]
    [DocHeader("Execution")]
    [DocExample(typeof(A_UsingBogus), nameof(Execute))]
    [DocHeader("Result")]
    [DocCodeFile("using-bogus-result.txt", "bash")]
    public void FakeIt()
    {
        File.Delete(Path.Combine(SolutionLocator.FindSolutionRoot()!, logFile));
        Execute();
    }

    [CodeSnippet]
    private static void Execute()
    {
        TheFaker().Generate(3).PulseToLog(logFile);
    }

    [CodeSnippet]
    [CodeRemove("return testUsers;")]
    private static Faker<User> TheFaker()
    {
        Randomizer.Seed = new Random(3897234);
        var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };
        var orderIds = 0;
        var testOrders = new Faker<Order>()
           .StrictMode(true)
           .RuleFor(o => o.OrderId, f => orderIds++)
           .RuleFor(o => o.Item, f => f.PickRandom(fruit))
           .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
           .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f));
        var userIds = 0;
        var testUsers = new Faker<User>()
           .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))
           .RuleFor(u => u.FirstName, f => f.Name.FirstName())
           .RuleFor(u => u.LastName, f => f.Name.LastName())
           .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
           .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
           .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
           .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")
           .RuleFor(u => u.SomeGuid, f => f.Random.Guid())
           .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
           .RuleFor(u => u.CartId, f => f.Random.Guid())
           .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
           .RuleFor(u => u.Orders, f => testOrders.Generate(3))
           .FinishWith((f, u) => $"User Created! Name={u.Id}".PulseToLog(logFile));
        return testUsers;
    }
}