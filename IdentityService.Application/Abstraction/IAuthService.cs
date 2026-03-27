using IdentityService.Application.Dto;
using NewsApi.Domain.Common.Validation;

namespace IdentityService.Application.Abstraction;

public interface IAuthService
{
    public Task<Result<AuthDto, Error>> RegisterAsync(RegisterCommand command, CancellationToken ct);
    public Task<Result<AuthDto, Error>> LoginAsync(LoginCommand command, CancellationToken ct);
    public Task LogoutAsync(string token);
    public Task<Result<AuthDto, Error>> RefreshTokenAsync(string token, CancellationToken ct);
}
