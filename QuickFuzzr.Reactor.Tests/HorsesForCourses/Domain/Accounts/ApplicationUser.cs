using System.Security.Claims;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Abstractions;
using QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Accounts.InvalidationReasons;

namespace QuickFuzzr.Reactor.Tests.HorsesForCourses.Domain.Accounts;

public class ApplicationUser : DomainEntity<ApplicationUser>
{
    public const string AdminRole = "admin";
    public const string CoachRole = "coach";
    public const string SystemRole = "system";

    public ApplicationUserName Name { get; init; } = ApplicationUserName.Empty;
    public ApplicationUserEmail Email { get; init; } = ApplicationUserEmail.Empty;
    public string PasswordHash { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;

    protected ApplicationUser() { }

    private ApplicationUser(string name, string email, string passwordHash, string role)
    {
        Name = new ApplicationUserName(name);
        Email = new ApplicationUserEmail(email);
        PasswordHash = passwordHash;
        Role = role;
    }

    public static ApplicationUser Create(string name, string email, string pass, string role)
    {
        if (string.IsNullOrWhiteSpace(pass))
            throw new PasswordCanNotBeEmpty();

        return new ApplicationUser(name, email, new Pbkdf2PasswordHasher().Hash(pass), role);
    }

    public virtual void CheckPassword(string password)
    {
        if (!new Pbkdf2PasswordHasher().Verify(password, PasswordHash))
            throw new EmailOrPasswordAreInvalid();
    }

    public virtual Actor EnterScene()
        => new Actor()
            .Declare(ClaimTypes.Name, Name.Value)
            .Declare(ClaimTypes.Email, Email.Value)
            .Declare(ClaimTypes.Role, Role);
}