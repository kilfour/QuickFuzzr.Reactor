using QuickFuzzr.Reactor.Lists;

namespace QuickFuzzr.Reactor;

public static partial class Fuze
{
    public static readonly FuzzrOf<string> Avatar =
        from nr in Fuzzr.Int(1000000, 9999999)
        from guid in Fuzzr.Guid()
        select $"https://grovotor.com/userimage/{nr}/{guid.ToString("N")}.jpeg?size=256";
}