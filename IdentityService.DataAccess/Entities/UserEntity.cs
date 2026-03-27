namespace IdentityService.DataAccess.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }
    
    public ICollection<RoleEntity> Roles { get; set; } = [];
    public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = [];
}