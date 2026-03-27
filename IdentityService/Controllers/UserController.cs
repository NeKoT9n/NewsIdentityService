using IdentityService.Application.Abstraction;
using IdentityService.Dto.Requests;
using IdentityService.Extiensions;
using IdentityService.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request, [FromServices] RegistrationMapper mapper)
    {
        var result = await userService.RegisterAsync(mapper.ToCommand(request));

        return result.ToActionResult();
    }
}