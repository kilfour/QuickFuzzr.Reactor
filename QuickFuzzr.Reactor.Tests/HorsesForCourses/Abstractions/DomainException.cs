using System.Text.RegularExpressions;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;

public class DomainException : Exception
{
    public DomainException() : base() { }
    public DomainException(string message) : base(message) { }

    public string MessageFromType =>
        LowercaseAllLettersExceptTheFirst(PutASpaceBeforeEachCapital(GetType().Name));

    private static string LowercaseAllLettersExceptTheFirst(string withSpaces)
        => $"{new string([.. withSpaces.Take(1)])}{new string([.. withSpaces.Skip(1).Select(char.ToLower)])}.";

    private string PutASpaceBeforeEachCapital(string input)
        => Regex.Replace(input, "(?<!^)([A-Z])", " $1");
}
