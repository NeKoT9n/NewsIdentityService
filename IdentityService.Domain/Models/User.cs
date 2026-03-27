namespace IdentityService.Domain.Models;

public class User
{
    public Guid Id { get; private set; } 
    public string Email { get; private set; } 
    public string PasswordHash { get; private set; } 
    public bool IsActive { get; private set; } 

    public virtual ICollection<Role> Roles { get; private set; } = [];

    private User(Guid id, string email, string passwordHash, bool isActive, ICollection<Role> roles)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = isActive;
        Roles = roles;
    }

    public static User Create(string email, string passwordHash, ICollection<Role> roles)
    {
        return Create(email, passwordHash, true, roles);
    }
    
    public static User Create(string email, string passwordHash, bool isActive, ICollection<Role> roles)
    {
        // TODO: Validation
        return new User(Guid.NewGuid(), email, passwordHash, isActive, roles);
    }
}