namespace QuickFuzzr.Reactor.Tests;

public class PersonInfo
{
    [Fact]
    public void Seeded()
    {
        var person = Fuze.Person.Generate(42);
        Assert.Equal("Earl", person.FirstName);
        Assert.Equal("Owens", person.LastName);
        Assert.Equal("Earl Owens", person.FullName);
        Assert.Equal("earl.owens@hotmail.org", person.Email);
        Assert.True(person.IsMale);
        Assert.False(person.IsFemale);
    }
}