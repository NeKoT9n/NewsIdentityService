namespace IdentityService.Dto.Requests;

public record RegisterRequest(string Username,string Email, string Password, long[]  RoleIds);