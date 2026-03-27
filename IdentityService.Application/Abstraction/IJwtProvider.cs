using IdentityService.Domain.Models;

namespace IdentityService.Application.Abstraction;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}