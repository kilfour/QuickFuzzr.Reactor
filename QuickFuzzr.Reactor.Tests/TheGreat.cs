using QuickPulse.Instruments;
using QuickPulse.Show;

namespace QuickFuzzr.Reactor.Tests;

public class TheGreat
{
    [Fact]
    public void Example()
    {
        File.Delete(Path.Combine(SolutionLocator.FindSolutionRoot()!, "result.log"));

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
            from person in Fuze.Person
            from __1 in Configr<User>.Ignore(a => a.Id)
            from __2 in Configr<User>.Ignore(a => a.SSN)
            from __3 in Configr<User>.Property(a => a.FirstName, person.FirstName)
            from __4 in Configr<User>.Property(a => a.LastName, person.LastName)
            from __5 in Configr<User>.Property(a => a.Avatar, Fuze.Avatar)
            from __6 in Configr<User>.Property(a => a.UserName, person.UserName)
            from __7 in Configr<User>.Property(a => a.Email, person.Email)
            from __8 in Configr<User>.Property(a => a.SomethingUnique, Fuzzr.String().Unique("something"))
            from __9 in Configr<User>.Property(a => a.Gender, person.IsMale ? Gender.Male : Gender.Female)
            from _10 in Configr<User>.Property(a => a.FullName, person.FullName)
            from _11 in Configr<User>.Property(a => a.Orders, orderFuzzr.Many(3))
            from id in Fuzzr.Counter("user-id")
            from ssn in ssnFuzzr
            from user in Fuzzr.One(() => new User(id, ssn)).Apply(a => $"User Created! Id={a.Id}".PulseToLog("result.log"))
            select user;

        userFuzzr.Generate(8675309).PulseToLog("result.log");
    }

    [Fact]
    public void Profiled()
    {
        File.Delete(Path.Combine(SolutionLocator.FindSolutionRoot()!, "result.log"));

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
            from _1 in Configr<User>.Ignore(a => a.Id)
            from _2 in Configr<User>.Ignore(a => a.SSN)
            from _3 in Fuze<User>.With(new PersonalInfo())
            from _4 in Configr<User>.Property(a => a.Avatar, Fuze.Avatar)
            from _5 in Configr<User>.Property(a => a.SomethingUnique, Fuzzr.String().Unique("something"))
            from _6 in Configr<User>.Property(a => a.Gender, Fuzzr.Enum<Gender>())
            from _7 in Configr<User>.Property(a => a.Orders, orderFuzzr.Many(3))
            from id in Fuzzr.Counter("user-id")
            from ssn in ssnFuzzr
            from user in Fuzzr.One(() => new User(id, ssn)).Apply(a => $"User Created! Id={a.Id}".PulseToLog("result.log"))
            select user;

        userFuzzr.Generate(8675309).PulseToLog("result.log");
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string Item { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int? LotNumber { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class User(int userId, string ssn)
    {
        public int Id { get; set; } = userId;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SomethingUnique { get; set; } = string.Empty;
        public Guid SomeGuid { get; set; }

        public string Avatar { get; set; } = string.Empty;
        public Guid CartId { get; set; }
        public string SSN { get; set; } = ssn;
        public Gender Gender { get; set; }

        public List<Order> Orders { get; set; } = [];
    }
}
