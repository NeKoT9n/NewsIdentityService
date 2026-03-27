namespace IdentityService.Domain.Models;

public class Role
{
    public long Id { get; private set; } 
    
    public string? Name { get; private set; }
    
    public string? Description { get; private set; }
    
    private Role(long id, string? name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static Role Create(long id, string? name = null, string? description = null)
    {
        return new Role(id, name, description);
    }
}