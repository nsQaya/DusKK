using TDV.Corporation;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Corporation.Exporting;
using TDV.Corporation.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;

namespace TDV.Corporation
{
    [AbpAuthorize(AppPermissions.Pages_Vehicles)]
    public class VehiclesAppService : TDVAppServiceBase, IVehiclesAppService
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IVehiclesExcelExporter _vehiclesExcelExporter;
        private readonly IRepository<Company, int> _lookup_companyRepository;

        public VehiclesAppService(IRepository<Vehicle> vehicleRepository, IVehiclesExcelExporter vehiclesExcelExporter, IRepository<Company, int> lookup_companyRepository)
        {
            _vehicleRepository = vehicleRepository;
            _vehiclesExcelExporter = vehiclesExcelExporter;
            _lookup_companyRepository = lookup_companyRepository;

        }

        public async Task<PagedResultDto<GetVehicleForViewDto>> GetAll(GetAllVehiclesInput input)
        {

            var filteredVehicles = _vehicleRepository.GetAll()
                        .Include(e => e.CompanyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Plate.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Brand.Contains(input.Filter) || e.TrackNo.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PlateFilter), e => e.Plate.Contains(input.PlateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinEndExaminationDateFilter != null, e => e.EndExaminationDate >= input.MinEndExaminationDateFilter)
                        .WhereIf(input.MaxEndExaminationDateFilter != null, e => e.EndExaminationDate <= input.MaxEndExaminationDateFilter)
                        .WhereIf(input.MinEndInsuranceDateFilter != null, e => e.EndInsuranceDate >= input.MinEndInsuranceDateFilter)
                        .WhereIf(input.MaxEndInsuranceDateFilter != null, e => e.EndInsuranceDate <= input.MaxEndInsuranceDateFilter)
                        .WhereIf(input.MinEndGuarantyDateFilter != null, e => e.EndGuarantyDate >= input.MinEndGuarantyDateFilter)
                        .WhereIf(input.MaxEndGuarantyDateFilter != null, e => e.EndGuarantyDate <= input.MaxEndGuarantyDateFilter)
                        .WhereIf(input.MinCapactiyFilter != null, e => e.Capactiy >= input.MinCapactiyFilter)
                        .WhereIf(input.MaxCapactiyFilter != null, e => e.Capactiy <= input.MaxCapactiyFilter)
                        .WhereIf(input.MinYearFilter != null, e => e.Year >= input.MinYearFilter)
                        .WhereIf(input.MaxYearFilter != null, e => e.Year <= input.MaxYearFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandFilter), e => e.Brand.Contains(input.BrandFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TrackNoFilter), e => e.TrackNo.Contains(input.TrackNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyDisplayPropertyFilter), e => string.Format("{0} {1}", e.CompanyFk == null || e.CompanyFk.TaxAdministration == null ? "" : e.CompanyFk.TaxAdministration.ToString()
, e.CompanyFk == null || e.CompanyFk.RunningCode == null ? "" : e.CompanyFk.RunningCode.ToString()
) == input.CompanyDisplayPropertyFilter);

            var pagedAndFilteredVehicles = filteredVehicles
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

           
            var totalCount = await filteredVehicles.CountAsync();

            var dbList = await pagedAndFilteredVehicles.ToListAsync();
           

            return new PagedResultDto<GetVehicleForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetVehicleForViewDto>>(dbList)
            );

        }

        public async Task<GetVehicleForViewDto> GetVehicleForView(int id)
        {
            var vehicle = await _vehicleRepository.GetAsync(id);

            var output = new GetVehicleForViewDto { Vehicle = ObjectMapper.Map<VehicleDto>(vehicle) };

            if (output.Vehicle.CompanyId != null)
            {
                var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)output.Vehicle.CompanyId);
                output.CompanyDisplayProperty = string.Format("{0} {1}", _lookupCompany.TaxAdministration, _lookupCompany.RunningCode);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Vehicles_Edit)]
        public async Task<GetVehicleForEditOutput> GetVehicleForEdit(EntityDto input)
        {
            var vehicle = await _vehicleRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVehicleForEditOutput { Vehicle = ObjectMapper.Map<CreateOrEditVehicleDto>(vehicle) };

            if (output.Vehicle.CompanyId != null)
            {
                var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)output.Vehicle.CompanyId);
                output.CompanyDisplayProperty = string.Format("{0} {1}", _lookupCompany.TaxAdministration, _lookupCompany.RunningCode);
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVehicleDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Vehicles_Create)]
        protected virtual async Task Create(CreateOrEditVehicleDto input)
        {
            var vehicle = ObjectMapper.Map<Vehicle>(input);

            await _vehicleRepository.InsertAsync(vehicle);

        }

        [AbpAuthorize(AppPermissions.Pages_Vehicles_Edit)]
        protected virtual async Task Update(CreateOrEditVehicleDto input)
        {
            var vehicle = await _vehicleRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, vehicle);

        }

        [AbpAuthorize(AppPermissions.Pages_Vehicles_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _vehicleRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetVehiclesToExcel(GetAllVehiclesForExcelInput input)
        {

            var filteredVehicles = _vehicleRepository.GetAll()
                        .Include(e => e.CompanyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Plate.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Brand.Contains(input.Filter) || e.TrackNo.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PlateFilter), e => e.Plate.Contains(input.PlateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinEndExaminationDateFilter != null, e => e.EndExaminationDate >= input.MinEndExaminationDateFilter)
                        .WhereIf(input.MaxEndExaminationDateFilter != null, e => e.EndExaminationDate <= input.MaxEndExaminationDateFilter)
                        .WhereIf(input.MinEndInsuranceDateFilter != null, e => e.EndInsuranceDate >= input.MinEndInsuranceDateFilter)
                        .WhereIf(input.MaxEndInsuranceDateFilter != null, e => e.EndInsuranceDate <= input.MaxEndInsuranceDateFilter)
                        .WhereIf(input.MinEndGuarantyDateFilter != null, e => e.EndGuarantyDate >= input.MinEndGuarantyDateFilter)
                        .WhereIf(input.MaxEndGuarantyDateFilter != null, e => e.EndGuarantyDate <= input.MaxEndGuarantyDateFilter)
                        .WhereIf(input.MinCapactiyFilter != null, e => e.Capactiy >= input.MinCapactiyFilter)
                        .WhereIf(input.MaxCapactiyFilter != null, e => e.Capactiy <= input.MaxCapactiyFilter)
                        .WhereIf(input.MinYearFilter != null, e => e.Year >= input.MinYearFilter)
                        .WhereIf(input.MaxYearFilter != null, e => e.Year <= input.MaxYearFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandFilter), e => e.Brand.Contains(input.BrandFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TrackNoFilter), e => e.TrackNo.Contains(input.TrackNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyDisplayPropertyFilter), e => string.Format("{0} {1}", e.CompanyFk == null || e.CompanyFk.TaxAdministration == null ? "" : e.CompanyFk.TaxAdministration.ToString()
, e.CompanyFk == null || e.CompanyFk.RunningCode == null ? "" : e.CompanyFk.RunningCode.ToString()
) == input.CompanyDisplayPropertyFilter);

            var query = (from o in filteredVehicles
                         join o1 in _lookup_companyRepository.GetAll() on o.CompanyId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetVehicleForViewDto()
                         {
                             Vehicle = new VehicleDto
                             {
                                 Plate = o.Plate,
                                 Description = o.Description,
                                 EndExaminationDate = o.EndExaminationDate,
                                 EndInsuranceDate = o.EndInsuranceDate,
                                 EndGuarantyDate = o.EndGuarantyDate,
                                 Capactiy = o.Capactiy,
                                 Year = o.Year,
                                 Brand = o.Brand,
                                 TrackNo = o.TrackNo,
                                 Id = o.Id
                             },
                             CompanyDisplayProperty = string.Format("{0} {1}", s1 == null || s1.TaxAdministration == null ? "" : s1.TaxAdministration.ToString()
, s1 == null || s1.RunningCode == null ? "" : s1.RunningCode.ToString()
)
                         });

            var vehicleListDtos = await query.ToListAsync();

            return _vehiclesExcelExporter.ExportToFile(vehicleListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Vehicles)]
        public async Task<PagedResultDto<VehicleCompanyLookupTableDto>> GetAllCompanyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_companyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => string.Format("{0} {1}", e.TaxAdministration, e.RunningCode).Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var companyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<VehicleCompanyLookupTableDto>();
            foreach (var company in companyList)
            {
                lookupTableDtoList.Add(new VehicleCompanyLookupTableDto
                {
                    Id = company.Id,
                    DisplayName = string.Format("{0} {1}", company.TaxAdministration, company.RunningCode)
                });
            }

            return new PagedResultDto<VehicleCompanyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}