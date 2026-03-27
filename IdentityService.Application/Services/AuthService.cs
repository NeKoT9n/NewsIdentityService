using IdentityService.Application.Abstraction;
using IdentityService.Application.Dto;
using IdentityService.DataAccess;
using IdentityService.DataAccess.Entities;
using IdentityService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using NewsApi.Domain.Common.Validation;

namespace IdentityService.Application.Services;

public class AuthService(
    IdentityDbContext context,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider) : IAuthService
{
    public async Task<Result<AuthDto, Error>> RegisterAsync(RegisterCommand command, CancellationToken ct)
    {
        bool exists = await context.Users
            .AnyAsync(u => u.Email == command.Email, ct);

        if (exists)
            return Errors.General.AlreadyExist(nameof(command.Email));

        var hashPassword = passwordHasher.Generate(command.Password);
        
        var roleEntities = await context.Roles
            .Where(r => command.RoleIds.AsEnumerable().Contains(r.Id))
            .ToListAsync(ct);
        
        var roles = roleEntities
            .Select(e => Role.Create(e.Id, e.Name, e.Description))
            .ToList();
        
        var user = User.Create(command.Email, hashPassword, roles);
        
        var accessToken = jwtProvider.GenerateAccessToken(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        
        var userEntity = new UserEntity
        {
            Id = user.Id,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            IsActive = true,
            Roles = roleEntities
        };
        
        var refreshTokenEntity = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = refreshTokenExpiration
        };
        
        await context.Users.AddAsync(userEntity, ct);
        await context.RefreshTokens.AddAsync(refreshTokenEntity, ct);
        await context.SaveChangesAsync(ct);
        
        return new AuthDto(accessToken, refreshToken, refreshTokenExpiration);
    }
    
    public async Task<Result<AuthDto, Error>> LoginAsync(LoginCommand command, CancellationToken ct)
    {
        var userEntity = await context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == command.Email, ct);

        if (userEntity == null)
            return Errors.Auth.InvalidCredentials; 
        
        var isPasswordValid = passwordHasher.Verify(command.Password, userEntity.PasswordHash);

        if (!isPasswordValid)
            return Errors.Auth.InvalidCredentials;
        
        if (!userEntity.IsActive)
            return Errors.Auth.AccountBlocked;


        var userDomain = User.Create(
            userEntity.Email, 
            userEntity.PasswordHash, 
            userEntity.Roles.Select(r => Role.Create(r.Id, r.Name)).ToList());
        
        var accessToken = jwtProvider.GenerateAccessToken(userDomain);
        var refreshTokenStr = jwtProvider.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            Token = refreshTokenStr,
            UserId = userEntity.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        
        await context.RefreshTokens.AddAsync(refreshTokenEntity, ct);
        await context.SaveChangesAsync(ct);

        return new AuthDto(accessToken, refreshTokenStr, refreshTokenEntity.ExpiresAt);
    }

    public async Task<Result<AuthDto, Error>> RefreshTokenAsync(string token, CancellationToken ct)
    {
        var refreshTokenEntity = await context.RefreshTokens
            .Include(t => t.User)
                .ThenInclude(u => u.Roles)
            .FirstOrDefaultAsync(t => t.Token == token, ct);
        
        if (refreshTokenEntity is not { IsActive: true })
            return Errors.Auth.InvalidRefreshToken;
        
        var userEntity = refreshTokenEntity.User;
        context.RefreshTokens.Remove(refreshTokenEntity);
        
        var user = User.Create(userEntity.Email, userEntity.PasswordHash, 
            userEntity.Roles.Select(r => Role.Create(r.Id, r.Name)).ToList());
    
        var newAccessToken = jwtProvider.GenerateAccessToken(user);
        var newRefreshTokenStr = jwtProvider.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        var newRefreshTokenEntity = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            Token = newRefreshTokenStr,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        
        await context.RefreshTokens.AddAsync(newRefreshTokenEntity, ct);
        await context.SaveChangesAsync(ct);
        
        return new AuthDto(newAccessToken, newRefreshTokenStr, refreshTokenExpiration);
    }
    
    public async Task LogoutAsync(string token)
    {
        var refreshTokenEntity = await context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token);

        if (refreshTokenEntity == null)
            return;

        context.RefreshTokens.Remove(refreshTokenEntity);
        await context.SaveChangesAsync();

    }
}