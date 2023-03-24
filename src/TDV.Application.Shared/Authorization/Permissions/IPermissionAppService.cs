using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Authorization.Permissions.Dto;

namespace TDV.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
