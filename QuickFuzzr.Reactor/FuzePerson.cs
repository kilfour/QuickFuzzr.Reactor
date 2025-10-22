using QuickFuzzr.Reactor.Lists;

namespace QuickFuzzr.Reactor;


public static partial class Fuze
{
    public static readonly FuzzrOf<Person> Person =
        from isMale in Fuzzr.Bool()
        let firstNames = isMale ? DataLists.MaleFirstNames : DataLists.FemaleFirstNames
        from firstName in Fuzzr.OneOf(DataLists.FirstNames)
        from lastName in Fuzzr.OneOf(DataLists.LastNames)
        select new Person
        {
            FirstName = firstName,
            LastName = lastName,
            FullName = $"{firstName} {lastName}",
            Email = $"{firstName}.{lastName}@mail.com".ToLower(),
            IsMale = isMale
        };
}