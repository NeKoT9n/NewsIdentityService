using IdentityService.Application.Dto;
using Riok.Mapperly.Abstractions;
using RegisterRequest = IdentityService.Dto.Requests.RegisterRequest;

namespace IdentityService.Mappers;

[Mapper]
public partial class RegistrationMapper
{
    public partial RegisterCommand ToCommand(RegisterRequest request);
}