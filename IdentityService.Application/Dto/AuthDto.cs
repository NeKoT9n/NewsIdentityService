namespace IdentityService.Application.Dto;

public record AuthDto(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);