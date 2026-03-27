using IdentityService.Application.Dto;
using NewsApi.Domain.Common.Validation;

namespace IdentityService.Application.Abstraction;

public interface IUserService
{
    public Task<Result<Guid, Error>> RegisterAsync(RegisterCommand command);
}