namespace QuickFuzzr.Reactor.Tests;

public class Spike
{
    public record User(string Name, string Email);

    [Fact]
    public void FirstShot()
    {
        var userFuzzr =
            from person in Fuze.Person
            select new User(person.FullName, person.Email);
        var user = userFuzzr.Generate(42);

        Assert.Equal("Earl Owens", user.Name);
        Assert.Equal("earl.owens@hotmail.org", user.Email);
    }
}
