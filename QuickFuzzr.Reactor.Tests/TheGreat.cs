using QuickFuzzr.Reactor.Tests._Tools.Models.GreatExample;
using QuickPulse.Instruments;
using QuickPulse.Show;

namespace QuickFuzzr.Reactor.Tests;

public class TheGreat
{
    const string logFile = "example.log";

    [Fact]
    public void Example()
    {
        File.Delete(Path.Combine(SolutionLocator.FindSolutionRoot()!, logFile));

        var orderFuzzr =
            from _1 in Configr<Order>.Property(a => a.OrderId, Fuzzr.Counter("order-id"))
            from _2 in Configr<Order>.Property(a => a.Item, Fuze.Fruit)
            from _3 in Configr<Order>.Property(a => a.Quantity, Fuzzr.Int(1, 10))
            from _4 in Configr<Order>.Property(a => a.LotNumber, Fuzzr.Int(0, 99).Nullable(0.8))
            from order in Fuzzr.One<Order>()
            select order;

        var ssnFuzzr =
            from a in Fuzzr.Int(100, 999)
            from b in Fuzzr.Int(10, 99)
            from c in Fuzzr.Int(1000, 9999)
            select $"{a}-{b}-{c}";

        var userFuzzr =
            from info in Fuze<User>.With(new PersonalInfo())
            from _1 in Configr<User>.Ignore(a => a.Id)
            from _2 in Configr<User>.Ignore(a => a.SSN)
            from _3 in Configr<User>.Property(a => a.Avatar, Fuze.Avatar)
            from _4 in Configr<User>.Property(a => a.SomethingUnique, Fuzzr.String().Unique("something"))
            from _5 in Configr<User>.Property(a => a.Gender, info.IsMale ? Gender.Male : Gender.Female)
            from _6 in Configr<User>.Property(a => a.Orders, orderFuzzr.Many(3))
            from id in Fuzzr.Counter("user-id")
            from ssn in ssnFuzzr
            from user in Fuzzr.One(() => new User(id, ssn)).Apply(a => $"User Created! Id={a.Id}".PulseToLog(logFile))
            select user;

        userFuzzr.Generate(8675309).PulseToLog(logFile);
    }
}
