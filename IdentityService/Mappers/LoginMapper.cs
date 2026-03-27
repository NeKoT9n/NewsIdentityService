using IdentityService.Application.Dto;
using IdentityService.Dto.Requests;
using Riok.Mapperly.Abstractions;

namespace IdentityService.Mappers;

[Mapper]
public partial class LoginMapper
{
    public partial LoginCommand ToCommand(LoginRequest request);
}