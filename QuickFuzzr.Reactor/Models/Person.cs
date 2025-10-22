namespace QuickFuzzr.Reactor;

public class Person
{
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsMale { get; set; }
    public bool IsFemale => !IsMale;
}
