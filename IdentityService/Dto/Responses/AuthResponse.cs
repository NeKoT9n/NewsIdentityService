namespace IdentityService.Dto.Responses;

public record AuthResponse(string AccessToken, DateTime ExpiresDate);