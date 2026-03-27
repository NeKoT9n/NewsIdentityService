namespace IdentityService.DataAccess.Entities;

public class RoleEntity
{
    public long Id { get; set; } 
    
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }

    public ICollection<UserEntity> Users { get; set; } = [];
}