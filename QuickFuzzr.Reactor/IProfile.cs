namespace QuickFuzzr.Reactor;

public interface IProfile<T>
{
    FuzzrOf<T> GetConfigr(Type type);
}
