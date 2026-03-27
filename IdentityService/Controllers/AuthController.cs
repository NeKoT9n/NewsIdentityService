using IdentityService.Application.Abstraction;
using IdentityService.Dto.Requests;
using IdentityService.Dto.Responses;
using IdentityService.Mappers;
using Microsoft.AspNetCore.Mvc;
using RegisterRequest = IdentityService.Dto.Requests.RegisterRequest;

namespace IdentityService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] RegistrationMapper mapper)
    {
        var result = await authService.RegisterAsync(mapper.ToCommand(request), HttpContext.RequestAborted);

        if (result.IsFailure)
            return BadRequest(result.Error);
        
        var authData = result.Value;
        
        SetRefreshTokenCookie(authData.RefreshToken, authData.RefreshTokenExpiration);
        
        return Ok(new AuthResponse(authData.AccessToken, authData.RefreshTokenExpiration));
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginMapper mapper)
    {
        
        var result = await authService.LoginAsync(mapper.ToCommand(request), HttpContext.RequestAborted);

        if (result.IsFailure)
            return Unauthorized(result.Error);
        
        var authData = result.Value;
        
        SetRefreshTokenCookie(authData.RefreshToken, authData.RefreshTokenExpiration);
    
        return Ok(new AuthResponse(authData.AccessToken, authData.RefreshTokenExpiration));
    }
    
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue("refresh", out var refreshToken))
            return Unauthorized("Refresh token is missing");
        
        var result = await authService.RefreshTokenAsync(refreshToken, HttpContext.RequestAborted);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        var authData = result.Value;
        
        SetRefreshTokenCookie(authData.RefreshToken, authData.RefreshTokenExpiration);

        return Ok(new AuthResponse(authData.AccessToken, authData.RefreshTokenExpiration));
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue("refresh", out var refreshToken))
            await authService.LogoutAsync(refreshToken);
        
        Response.Cookies.Delete("refresh", new CookieOptions { Path = "/api/auth" });
        
        return Ok();
    }
    
    private void SetRefreshTokenCookie(string token, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,  
            Secure = true,   
            SameSite = SameSiteMode.Strict, 
            Expires = expires, 
            Path = "/api/auth"
        };

        Response.Cookies.Append("refresh", token, cookieOptions);
    }
}