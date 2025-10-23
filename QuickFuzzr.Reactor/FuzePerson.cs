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
