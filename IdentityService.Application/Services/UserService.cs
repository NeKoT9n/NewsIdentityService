using IdentityService.Application.Abstraction;
using IdentityService.Application.Dto;
using IdentityService.DataAccess;
using NewsApi.Domain.Common.Validation;

namespace IdentityService.Application.Services;

public class UserService(IdentityDbContext context) : IUserService
{
    public async Task<Result<Guid, Error>> RegisterAsync(RegisterCommand command)
    {
        
        return null;
    }
}