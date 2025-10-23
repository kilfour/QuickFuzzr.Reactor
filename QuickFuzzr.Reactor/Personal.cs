using QuickFuzzr.Reactor.Models;

namespace QuickFuzzr.Reactor;

public static class Personal
{
    public static PersonalInfo Info => new();

    public class PersonalInfo : IProfile<Person>
    {
        public FuzzrOf<Person> GetConfigr(Type type) =>
            from person in Fuze.Person
            from _1 in Configr.Property(a => type.PropertyNamed(a, "firstname"), person.FirstName)
            from _2 in Configr.Property(a => type.PropertyNamed(a, "lastname"), person.LastName)
            from _3 in Configr.Property(a => type.PropertyNamed(a, "fullname"), person.FullName)
            from _4 in Configr.Property(a => type.PropertyNamed(a, "email"), person.Email)
            from _5 in Configr.Property(a => type.PropertyNamed(a, "username"), person.UserName)
            from _6 in Configr.Property(a => type.PropertyNamed(a, "lastname"), person.LastName)
            select person;


    }
}
