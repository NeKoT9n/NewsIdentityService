using IdentityService.Application.Abstraction;
using BC = BCrypt.Net.BCrypt;

namespace IdentityService.Application.Services.Password;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password) =>
        BC.HashPassword(password);

    public bool Verify(string password, string hashedPassword) =>
        BC.Verify(password, hashedPassword);
}