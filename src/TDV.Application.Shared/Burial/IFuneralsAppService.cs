using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Burial.Dtos;
using TDV.Dto;
using System.Collections.Generic;

namespace TDV.Burial
{
    public interface IFuneralsAppService : IApplicationService
    {
        Task<PagedResultDto<GetFuneralForViewDto>> GetAll(GetAllFuneralsInput input);

        Task<GetFuneralForViewDto> GetFuneralForView(int id);

        Task<GetFuneralForEditOutput> GetFuneralForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditFuneralDto input);

        Task<int> CreateAndGetId(CreateOrEditFuneralDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFuneralsToExcel(GetAllFuneralsForExcelInput input);

        Task<List<VehiclesLookupTableDto>> GetVehiclesForPackage(int packageId);

        Task<List<EmployeeLookupTableDto>> GetEmployeesForPackage(int packageId);

        Task<List<FuneralFuneralTypeLookupTableDto>> GetAllFuneralTypeForTableDropdown();

        Task<PagedResultDto<FuneralContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<List<FuneralOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForTableDropdown();

        Task<List<FuneralUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<List<FuneralFuneralPackageLookupTableDto>> GetAllFuneralPackageForTableDropdown();

        Task<List<FuneralContractLookupTableDto>> GetAllContractForTableDropdown();

        Task<List<VehiclesLookupTableDto>> GetAllVehicleForTableDropdown(int? companyId = null);

        Task<List<EmployeeLookupTableDto>> GetAllEmployeeForTableDropdown(int? companyId = null, string roleName = null);

        Task<List<CompaniesLookupTableDto>> GetCompaniesForPackage(List<int> funeralIDs);

        Task<PagedResultDto<GetFuneraPackagelForViewDto>> GetAllGroupByPackage();

        Task ClearPackageFromFuneral(int funeralId);
    }
}