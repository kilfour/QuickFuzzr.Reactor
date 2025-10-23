using System.Reflection;

namespace QuickFuzzr.Reactor;

public class PersonalInfo : IProfile<Person>
{
    public FuzzrOf<Person> GetConfigr(Type type) =>
        from person in Fuze.Person
        from _1 in Configr.Property(a => PropertyNamed(type, a, "firstname"), person.FirstName)
        from _2 in Configr.Property(a => PropertyNamed(type, a, "lastname"), person.LastName)
        from _3 in Configr.Property(a => PropertyNamed(type, a, "fullname"), person.FullName)
        from _4 in Configr.Property(a => PropertyNamed(type, a, "email"), person.Email)
        from _5 in Configr.Property(a => PropertyNamed(type, a, "username"), person.UserName)
        from _6 in Configr.Property(a => PropertyNamed(type, a, "lastname"), person.LastName)
        select person;

    private static bool PropertyNamed(Type type, PropertyInfo propertyInfo, string propertyName)
        => propertyInfo.DeclaringType == type
            && string.Equals(propertyInfo.Name, propertyName, StringComparison.OrdinalIgnoreCase);
}
