namespace QuickFuzzr.Reactor.Tests._Tools.Models.GreatExample;

public class User(int userId, string ssn)
{
    public int Id { get; set; } = userId;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SomethingUnique { get; set; } = string.Empty;
    public Guid SomeGuid { get; set; }

    public string Avatar { get; set; } = string.Empty;
    public Guid CartId { get; set; }
    public string SSN { get; set; } = ssn;
    public Gender Gender { get; set; }

    public List<Order> Orders { get; set; } = [];
}