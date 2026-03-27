namespace IdentityService.Application.Dto;

public record RegisterCommand(string Username,string Email, string Password);