using QuickFuzzr.Reactor.Lists;

namespace QuickFuzzr.Reactor;

public static partial class Fuze
{
    public static readonly FuzzrOf<string> Fruit = Fuzzr.OneOf(DataLists.Fruits);
}