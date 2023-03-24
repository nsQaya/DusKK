using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Corporation.Dtos;
using TDV.Dto;

namespace TDV.Corporation
{
    public interface IVehiclesAppService : IApplicationService
    {
        Task<PagedResultDto<GetVehicleForViewDto>> GetAll(GetAllVehiclesInput input);

        Task<GetVehicleForViewDto> GetVehicleForView(int id);

        Task<GetVehicleForEditOutput> GetVehicleForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditVehicleDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetVehiclesToExcel(GetAllVehiclesForExcelInput input);

        Task<PagedResultDto<VehicleCompanyLookupTableDto>> GetAllCompanyForLookupTable(GetAllForLookupTableInput input);

    }
}