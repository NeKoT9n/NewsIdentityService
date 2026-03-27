namespace IdentityService.Domain.Models;

public class RefreshToken 
{
    public Guid Id { get; set; }
    
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsActive => !IsRevoked && !IsExpired;

    public virtual User User { get; set; } = null!;
}