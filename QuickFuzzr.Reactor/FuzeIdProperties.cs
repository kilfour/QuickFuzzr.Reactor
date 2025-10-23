namespace QuickFuzzr.Reactor;

public static partial class Fuze
{
    public static FuzzrOf<Intent> IdProperties =>
        Configr.Property(a => a.IsEntityId<int>(), a => Fuzzr.Counter($"{a.Name.ToLower()}-id"));
}