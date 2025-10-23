namespace QuickFuzzr.Reactor;

public static class Fuze<TTarget>
{
    public static FuzzrOf<T> With<T>(IProfile<T> profile)
        => profile.GetConfigr(typeof(TTarget));
}
