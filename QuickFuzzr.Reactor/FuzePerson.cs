using System.Reflection;
using QuickFuzzr.Reactor.Lists;

namespace QuickFuzzr.Reactor;


public static partial class Fuze
{
    public static readonly FuzzrOf<Person> Person =
        from isMale in Fuzzr.Bool()
        let firstNames = isMale ? DataLists.MaleFirstNames : DataLists.FemaleFirstNames
        from firstName in Fuzzr.OneOf(DataLists.FirstNames)
        from lastName in Fuzzr.OneOf(DataLists.LastNames)
        from emailProvider in Fuzzr.OneOf(DataLists.EmailProviders)
        from domain in Fuzzr.OneOf(DataLists.TopLevelDomains)
        from userNameSuffix in Fuzzr.Int(7, 99)
        select new Person
        {
            UserName = $"{firstName}_{lastName}{userNameSuffix}",
            FirstName = firstName,
            LastName = lastName,
            FullName = $"{firstName} {lastName}",
            Email = $"{firstName}.{lastName}@{emailProvider}.{domain}".ToLower(),
            IsMale = isMale
        };
}

public class PersonalInfo : IProfile
{
    public FuzzrOf<Intent> GetConfigr(Type type) =>
        from person in Fuze.Person
        from _1 in Configr.Property(a => PropertyNamed(type, a, "firstname"), person.FirstName)
        from _2 in Configr.Property(a => PropertyNamed(type, a, "lastname"), person.LastName)
        from _3 in Configr.Property(a => PropertyNamed(type, a, "fullname"), person.FullName)
        from _4 in Configr.Property(a => PropertyNamed(type, a, "email"), person.Email)
        from _5 in Configr.Property(a => PropertyNamed(type, a, "username"), person.UserName)
        from _6 in Configr.Property(a => PropertyNamed(type, a, "lastname"), person.LastName)
        select Intent.Fixed;

    private static bool PropertyNamed(Type type, PropertyInfo propertyInfo, string propertyName)
        => propertyInfo.DeclaringType == type
            && string.Equals(propertyInfo.Name, propertyName, StringComparison.OrdinalIgnoreCase);
}

public interface IProfile
{
    FuzzrOf<Intent> GetConfigr(Type type);
}

public static class Fuze<TTarget>
{
    public static FuzzrOf<Intent> With(IProfile profile) =>
        from _ in profile.GetConfigr(typeof(TTarget))
        select Intent.Fixed;
}