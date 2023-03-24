using TDV.Burial;
using TDV.Communication;
using Abp.Organizations;
using TDV.Authorization.Users;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Burial.Exporting;
using TDV.Burial.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;
using TDV.Flight;
using TDV.Integration.Modules.Interfaces;
using TDV.Location;
using TDV.Organizations;
using TDV.Payment;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TDV.Corporation;
using TDV.Authorization.Roles;
using Abp.EntityFrameworkCore.EFPlus;
using NUglify.Helpers;
using System.Collections;

namespace TDV.Burial
{
    [AbpAuthorize(AppPermissions.Pages_Funerals)]
    public class FuneralsAppService : TDVAppServiceBase, IFuneralsAppService
    {
        private readonly IRepository<Funeral> _funeralRepository;
        private readonly IFuneralsExcelExporter _funeralsExcelExporter;
        private readonly IRepository<FuneralType, int> _lookup_funeralTypeRepository;
        private readonly IRepository<Contact, int> _lookup_contactRepository;
        private readonly IRepository<AirlineCompany, int> _lookup_airlineCompanyRepository;
        private readonly IRepository<Airport, int> _lookup_airportRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<FuneralPackage, int> _lookup_funeralPackageRepository;
        private readonly IRepository<Region, int> _lookup_regionRepository;
        private readonly IRepository<Quarter, int> _lookup_quarterRepository;
        private readonly CustomAbpSession _customAbpSession;
        private readonly IContactsAppService _contactsAppService;
        private readonly IBlobStorageModule _blobStorageModule;
        private readonly UserManager _userManager;
        private readonly IUserOrganizationUnitRepository _userOrganizationUnitRepository;
        private readonly IRepository<Contract, int> _lookup_contractRepository;
        private readonly IRepository<Company, int> _lookup_companyRepository;
        private readonly IRepository<Role, int> _lookup_roleRepository;
        private readonly IRepository<Vehicle, int> _lookup_vehicleRepository;

        public FuneralsAppService(
            IRepository<Funeral> funeralRepository,
            IFuneralsExcelExporter funeralsExcelExporter,
            IRepository<FuneralType, int> lookup_funeralTypeRepository,
            IRepository<Contact, int> lookup_contactRepository,
            IRepository<AirlineCompany, int> lookup_airlineCompanyRepository,
            IRepository<Airport, int> lookup_airportRepository,
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<Role, int> lookup_roleRepository,
            IRepository<FuneralPackage, int> lookup_funeralPackageRepository,
            IRepository<Region, int> lookup_regionRepository,
            IRepository<Quarter, int> lookup_quarterRepository,
            IBlobStorageModule blobStorageModule,
            IContactsAppService contactsAppService,
            CustomAbpSession customAbpSession,
            UserManager userManager,
            IUserOrganizationUnitRepository userOrganizationUnitRepository,
            IRepository<Contract, int> lookup_contractRepository,
            IRepository<Vehicle, int> lookup_vehicleRepository,
            IRepository<Company, int> lookup_companyRepository)
        {
            _funeralRepository = funeralRepository;
            _funeralsExcelExporter = funeralsExcelExporter;
            _lookup_funeralTypeRepository = lookup_funeralTypeRepository;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_funeralPackageRepository = lookup_funeralPackageRepository;
            _contactsAppService = contactsAppService;
            _lookup_airlineCompanyRepository = lookup_airlineCompanyRepository;
            _lookup_airportRepository = lookup_airportRepository;
            _customAbpSession = customAbpSession;

            _lookup_regionRepository = lookup_regionRepository;
            _lookup_quarterRepository = lookup_quarterRepository;
            _blobStorageModule = blobStorageModule;
            _userManager = userManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _lookup_contractRepository = lookup_contractRepository;
            _lookup_companyRepository = lookup_companyRepository;
            _lookup_roleRepository = lookup_roleRepository;
            _lookup_vehicleRepository = lookup_vehicleRepository;
        }

        public async Task<PagedResultDto<GetFuneralForViewDto>> GetAll(GetAllFuneralsInput input)
        {
            var userOrgUnits = _customAbpSession.OrganizationUnits;
            var b = GetCurrentUser();
            var c = _customAbpSession.Roles;

            var statusFilter = input.StatusFilter.HasValue
                        ? (FuneralStatus)input.StatusFilter
                        : default;

            var filteredFunerals = _funeralRepository.GetAll()
                        .Include(e => e.TypeFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.OwnerOrgUnitFk)
                        .Include(e => e.GiverOrgUnitFk)
                        .Include(e => e.ContractorOrgUnitFk)
                        .Include(e => e.EmployeePersonFk)
                        .Include(e => e.FuneralPackageFk)
                        .Include(e => e.Flights)
                        .Include(e => e.Documents)
                        .Include(e => e.Addresses)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TransferNo.Contains(input.Filter) || e.MemberNo.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Surname.Contains(input.Filter) || e.PassportNo.Contains(input.Filter) || e.LadingNo.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransferNoFilter), e => e.TransferNo.Contains(input.TransferNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MemberNoFilter), e => e.MemberNo.Contains(input.MemberNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurnameFilter), e => e.Surname.Contains(input.SurnameFilter))
                        .WhereIf(input.MinTcNoFilter != null, e => e.TcNo >= input.MinTcNoFilter)
                        .WhereIf(input.MaxTcNoFilter != null, e => e.TcNo <= input.MaxTcNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PassportNoFilter), e => e.PassportNo.Contains(input.PassportNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LadingNoFilter), e => e.LadingNo.Contains(input.LadingNoFilter))
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(input.MinOperationDateFilter != null, e => e.OperationDate >= input.MinOperationDateFilter)
                        .WhereIf(input.MaxOperationDateFilter != null, e => e.OperationDate <= input.MaxOperationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralTypeDescriptionFilter), e => e.TypeFk != null && e.TypeFk.Description == input.FuneralTypeDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.ContactFk == null || e.ContactFk.Name == null ? "" : e.ContactFk.Name.ToString()
, e.ContactFk == null || e.ContactFk.Surname == null ? "" : e.ContactFk.Surname.ToString()
, e.ContactFk == null || e.ContactFk.IdentifyNo == null ? "" : e.ContactFk.IdentifyNo.ToString()
) == input.ContactDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OwnerOrganizationUnitDisplayNameFilter), e => e.OwnerOrgUnitFk != null && e.OwnerOrgUnitFk.DisplayName == input.OwnerOrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GiverOrganizationUnitDisplayNameFilter), e => e.GiverOrgUnitFk != null && e.GiverOrgUnitFk.DisplayName == input.GiverOrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContractorOrganizationUnitDisplayNameFilter), e => e.ContractorOrgUnitFk != null && e.ContractorOrgUnitFk.DisplayName == input.ContractorOrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.EmployeePersonFk != null && e.EmployeePersonFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralPackageCodeFilter), e => e.FuneralPackageFk != null && e.FuneralPackageFk.Code == input.FuneralPackageCodeFilter);

            if (!c.Contains(TDV.Authorization.Roles.StaticRoleNames.Tenants.Manager))
            {
                //Eğer yönetici rolünde ise kısıt verilmeden listelenecek.
                if (c.Contains(TDV.Authorization.Roles.StaticRoleNames.Tenants.Employee))
                {
                    //Kitabevi ise sadece kendi sahibi olduğu cenazeleri veya kendine atanan cenazeleri görür
                    filteredFunerals = filteredFunerals.Where(x => userOrgUnits.Contains(x.GiverOrgUnitId) || userOrgUnits.Contains(x.ContractorOrgUnitId.Value));
                }
                if (c.Contains(TDV.Authorization.Roles.StaticRoleNames.Tenants.Driver))
                {
                    //Şöförler sadece kendine atanan cenazeleri görör
                    filteredFunerals = filteredFunerals.Where(x => x.EmployeePersonId == _customAbpSession.UserId.Value);
                }
                if (c.Contains(TDV.Authorization.Roles.StaticRoleNames.Tenants.Enterer))
                {
                    //Cenaze girişi yapanlar sadece kendi cenaze girişi yaptıkları cenazeleri görürler
                    filteredFunerals = filteredFunerals.Where(x => userOrgUnits.Contains(x.OwnerOrgUnitId));
                }
            }

            var pagedAndFilteredFunerals = filteredFunerals
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);



            var totalCount = await filteredFunerals.CountAsync();

            var dbList = await pagedAndFilteredFunerals.ToListAsync();


            return new PagedResultDto<GetFuneralForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetFuneralForViewDto>>(dbList)
            );

        }

        public async Task<GetFuneralForViewDto> GetFuneralForView(int id)
        {
            var funeral = await _funeralRepository.
                GetAllIncluding(x => x.ContactFk,
                x => x.ContactFk.Details,
                x => x.TypeFk,
                x => x.ContactFk,
                x => x.OwnerOrgUnitFk,
                x => x.GiverOrgUnitFk,
                x => x.ContractorOrgUnitFk,
                x => x.EmployeePersonFk,
                x => x.FuneralPackageFk,
                x => x.Documents)
                .Include(x => x.Addresses)
                    .ThenInclude(x => x.QuarterFk.DistrictFk)
                    .ThenInclude(x => x.RegionFk)
                .Include(x => x.Addresses)
                    .ThenInclude(x => x.QuarterFk.DistrictFk.CityFk.CountryFk)
                .Include(x=> x.Flights)
                    .ThenInclude(x=>x.AirlineCompanyFk)
                .Include(x => x.Flights)
                    .ThenInclude(x => x.LiftOffAirportFk)
                .Include(x => x.Flights)
                    .ThenInclude(x => x.LangingAirportFk)
                .FirstOrDefaultAsync(x => x.Id == id);

            
            var output = new GetFuneralForViewDto { Funeral = ObjectMapper.Map<FuneralDto>(funeral) };

            if (output.Funeral.TypeId != null)
            {
                var _lookupFuneralType = await _lookup_funeralTypeRepository.FirstOrDefaultAsync((int)output.Funeral.TypeId);
                output.FuneralTypeDescription = _lookupFuneralType?.Description?.ToString();
            }

            if (output.Funeral.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((int)output.Funeral.ContactId);
                output.ContactDisplayProperty = string.Format("{0} {1} {2}", _lookupContact.Name, _lookupContact.Surname, _lookupContact.IdentifyNo);
            }

            if (output.Funeral.OwnerOrgUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Funeral.OwnerOrgUnitId);
                output.OwnerOrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.Funeral.GiverOrgUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Funeral.GiverOrgUnitId);
                output.GiverOrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.Funeral.ContractorOrgUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Funeral.ContractorOrgUnitId);
                output.ContractorOrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.Funeral.EmployeePersonId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Funeral.EmployeePersonId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Funeral.FuneralPackageId != null)
            {
                var _lookupFuneralPackage = await _lookup_funeralPackageRepository.FirstOrDefaultAsync((int)output.Funeral.FuneralPackageId);
                output.FuneralPackageCode = _lookupFuneralPackage?.Code?.ToString();
            }

            if (output.Funeral?.Address?.QuarterId != null)
            {
                var address = funeral.Addresses.FirstOrDefault();
                output.CountryDisplayName = address.QuarterFk.DistrictFk.CityFk.CountryFk.Name;
                output.CityDisplayName = address.QuarterFk.DistrictFk.CityFk.Name;
                output.DistrictDisplayName = address.QuarterFk.DistrictFk.Name;
                output.QuarterDisplayName = address.QuarterFk.Name;
                output.RegionDisplayName = address.QuarterFk.DistrictFk.RegionFk.Name;
            }

            if (output.Funeral?.Flight?.LiftOffAirportId != null)
            {
                var flight = funeral.Flights.FirstOrDefault();
                output.LandingAirportDisplayName = flight.LangingAirportFk.Name;
                output.LiftOffAirportDisplayName = flight.LiftOffAirportFk.Name;
                output.AirlineCompanyDisplayName = flight.AirlineCompanyFk.Name;
            }

            return output;
        }

        public async Task<List<CompaniesLookupTableDto>> GetCompaniesForPackage(List<int> funeralIDs)
        {
            var funerals = await _funeralRepository
                .GetAllIncluding()
                .Include(x => x.Flights)
                    .Include(x => x.Addresses)
                        .ThenInclude(x => x.QuarterFk)
                            .ThenInclude(x => x.DistrictFk)
                            .Where(x => x.FuneralPackageId == null && funeralIDs.Contains(x.Id)).ToListAsync();

            //TODO: Açıklama düzenlenecek
            if (funerals == null)
                throw new UserFriendlyException("Cenaze yok");
            if (funerals.Count == 0)
                throw new UserFriendlyException("Cenaze yok");

            var region = funerals.GroupBy(g => g.Addresses.FirstOrDefault().QuarterFk.DistrictFk.RegionId).ToList();
            if (region.Count > 1)
            {
                throw new UserFriendlyException("Paket yapmak istediğin cenazelerin bölgeleri farklı olamaz.");
            }

            var regionID = region.FirstOrDefault().Key;
            var avaibleContracts = new List<Contract>();

            foreach (var funeral in funerals)
            {
                var flight = funeral.Flights.LastOrDefault();

                var contracts = await _lookup_contractRepository.GetAll()
                .Where(x => x.RegionId == regionID
                    && x.StartDate <= flight.LandingDate
                    && x.EndDate >= flight.LandingDate)
                .ToListAsync();

                avaibleContracts.AddRange(contracts);
            }

            var grouppedContracts = avaibleContracts.GroupBy(x => x.CompanyId).Select(x => new
            {
                Count = x.Count(),
                CompanyId = x.First().CompanyId
            });

            var avaibleForPacking = grouppedContracts.Where(x => x.Count == funeralIDs.Count).Select(x => x.CompanyId);

            if (avaibleForPacking.Count() <= 0)
            {
                throw new UserFriendlyException("Cenazeleri paketleyebileceğiniz ortak bir aktif sözleşmesi olan şirket yok !");
            }

            return await _lookup_companyRepository.GetAllIncluding(x => x.OrganizationUnitFk)
               .Select(company => new CompaniesLookupTableDto
               {
                   Id = company.Id,
                   DisplayName = company.OrganizationUnitFk.DisplayName,
               }).ToListAsync();
        }
        public async Task<List<VehiclesLookupTableDto>> GetVehiclesForPackage(int packageId)
        {
            var package = _lookup_funeralPackageRepository.GetAllIncluding(x => x.Funerals).FirstOrDefault(x => x.Id == packageId);
            if (package==null)
            {
                throw new UserFriendlyException("Paket yok !");
            }

            var orgUnitId = package.Funerals.First().ContractorOrgUnitId;
            var company = await _lookup_companyRepository.GetAllIncluding(x=>x.Vehicles).FirstOrDefaultAsync(x => x.OrganizationUnitId == orgUnitId && x.IsActive==true);

            if (company == null)
            {
                throw new UserFriendlyException("Şirket yok veya aktif değil!");
            }


            return company.Vehicles.Select(x =>
            new VehiclesLookupTableDto
            {
                DisplayName = x.Plate,
                Id = x.Id
            }).ToList();
        }

        public async Task<List<EmployeeLookupTableDto>> GetEmployeesForPackage(int packageId)
        {
            var package = _lookup_funeralPackageRepository.GetAllIncluding(x => x.Funerals).FirstOrDefault(x => x.Id == packageId);
            if (package == null)
            {
                throw new UserFriendlyException("Paket yok !");
            }

            var orgUnitId = package.Funerals.First().ContractorOrgUnitId;
            var company = await _lookup_companyRepository.FirstOrDefaultAsync(x => x.OrganizationUnitId == orgUnitId && x.IsActive == true);

            if (company == null)
            {
                throw new UserFriendlyException("Şirket yok veya aktif değil!");
            }


            return await GetAllEmployeeForTableDropdown(company.Id, "Driver");
        }
        

        [AbpAuthorize(AppPermissions.Pages_Funerals_Edit)]
        public async Task<GetFuneralForEditOutput> GetFuneralForEdit(EntityDto input)
        {
            var funeral = await _funeralRepository.
               GetAllIncluding(x => x.ContactFk,
               x => x.Flights,
               x => x.ContactFk.Details,
               x => x.Addresses,
               x => x.TypeFk,
               x => x.ContactFk,
               x => x.OwnerOrgUnitFk,
               x => x.GiverOrgUnitFk,
               x => x.ContractorOrgUnitFk,
               x => x.EmployeePersonFk,
               x => x.FuneralPackageFk,
               x => x.Documents)
               .FirstOrDefaultAsync(x => x.Id == input.Id);

            var output = new GetFuneralForEditOutput { Funeral = ObjectMapper.Map<CreateOrEditFuneralDto>(funeral) };

            if (output.Funeral.TypeId != null)
            {
                var _lookupFuneralType = await _lookup_funeralTypeRepository.FirstOrDefaultAsync((int)output.Funeral.TypeId);
                output.FuneralTypeDescription = _lookupFuneralType?.Description?.ToString();
            }

            if (output.Funeral.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((int)output.Funeral.ContactId);
                output.ContactDisplayProperty = string.Format("{0} {1} {2}", _lookupContact.Name, _lookupContact.Surname, _lookupContact.IdentifyNo);
            }

            if (output.Funeral.OwnerOrgUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Funeral.OwnerOrgUnitId);
                output.OwnerOrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.Funeral.GiverOrgUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Funeral.GiverOrgUnitId);
                output.GiverOrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.Funeral.ContractorOrgUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Funeral.ContractorOrgUnitId);
                output.ContractorOrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.Funeral.EmployeePersonId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Funeral.EmployeePersonId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Funeral.FuneralPackageId != null)
            {
                var _lookupFuneralPackage = await _lookup_funeralPackageRepository.FirstOrDefaultAsync((int)output.Funeral.FuneralPackageId);
                output.FuneralPackageCode = _lookupFuneralPackage?.Code?.ToString();
            }
            /*
            if (output.Funeral.Flight.AirlineCompanyId != null)
            {
                var _lookupAirlineCompany = await _lookup_airlineCompanyRepository.FirstOrDefaultAsync((int)output.Funeral.Flight.AirlineCompanyId);
                output.AirlineDisplayProperty = _lookupAirlineCompany.Code;
            }

            if (output.Funeral.Flight.LiftOffAirportId != null)
            {
                var _lookupLiftOffAirport = await _lookup_airportRepository.FirstOrDefaultAsync((int)output.Funeral.Flight.LiftOffAirportId);
                output.LiftOffAirportDisplayProperty = _lookupLiftOffAirport.Code + " " + _lookupLiftOffAirport.Name;
            }

            if (output.Funeral.Flight.LangingAirportId != null)
            {
                var _lookupLangingOffAirport = await _lookup_airportRepository.FirstOrDefaultAsync((int)output.Funeral.Flight.LangingAirportId);
                output.LandingAirportDisplayProperty = _lookupLangingOffAirport.Code + " " + _lookupLangingOffAirport.Name;
            }

            if (output.Funeral.Address.QuarterId != null)
            {
                var _lookupQuarter = await _lookup_quarterRepository
                    .GetAllIncluding()
                    .Include(x => x.DistrictFk)
                        .ThenInclude(x => x.CityFk)
                            .ThenInclude(x => x.CountryFk)
                    .Include(x => x.DistrictFk)
                        .ThenInclude(x => x.RegionFk)
                    .FirstOrDefaultAsync(x => x.Id == output.Funeral.Address.QuarterId);

                output.RegionDisplayProperty = _lookupQuarter.DistrictFk.RegionFk.Name;
                output.DistrictId = _lookupQuarter.DistrictId;
                output.QuarterId = output.Funeral.Address.QuarterId;
                output.CityId = _lookupQuarter.DistrictFk.CityId;
                output.CountryId = _lookupQuarter.DistrictFk.CityFk.CountryId;
            }
            */
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFuneralDto input)
        {
            if ((input.Contact.Id.HasValue == false || input.Contact.Id.Value==0) && !string.IsNullOrEmpty(input.Contact.Name))
            {

                input.ContactId = await _contactsAppService.CreateAndGet(input.Contact);
            }
            else
            {
                input.ContactId = input.Contact.Id.Value;
            }

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals_Create)]
        protected virtual async Task Create(CreateOrEditFuneralDto input)
        {
            var funeral = ObjectMapper.Map<Funeral>(input);
            await _funeralRepository.InsertAsync(funeral);
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals_Create)]
        public async Task<int> CreateAndGetId(CreateOrEditFuneralDto input)
        {

            if ((input.Contact.Id.HasValue == false || input.Contact.Id.Value == 0) && !string.IsNullOrEmpty(input.Contact.Name))
            {

                input.ContactId = await _contactsAppService.CreateAndGet(input.Contact);
            }
            else
            {
                input.ContactId = input.Contact.Id.Value;
            }

            var funeral = ObjectMapper.Map<Funeral>(input);
            return await _funeralRepository.InsertAndGetIdAsync(funeral);
        }
        [AbpAuthorize(AppPermissions.Pages_Funerals_Create)]
        public async Task CheckForOperation(int funeralId)
        {
            var funeral = await _funeralRepository.GetAllIncluding(x=>x.Addresses, x=>x.Flights, x => x.Documents).FirstOrDefaultAsync(x => x.Id == funeralId);

            if(funeral.Addresses.Count <= 0)
            {
                throw new UserFriendlyException("Adres bilgisi gerekli !");
            }

            if (funeral.Flights.Count <= 0)
            {
                throw new UserFriendlyException("Uçuş bilgisi gerekli !");
            }

            if (funeral.Documents.Count <= 0)
            {
                throw new UserFriendlyException("Belge gerekli !");
            }

            funeral.Status = FuneralStatus.New;

            await _funeralRepository.UpdateAsync(funeral);
        }


        [AbpAuthorize(AppPermissions.Pages_Funerals_Vehicle_Assignment)]
        public async Task FuneralVehicleAssignment(int packedId, int vehicleId)
        {
            var funeral = await _funeralRepository.GetAll().Where(x => x.FuneralPackageId == packedId).ToListAsync();
            //TODO: Açıklama düzenlenecek
            if (funeral == null)
                throw new UserFriendlyException("Cenaze yok");
            if (funeral.Count == 0)
                throw new UserFriendlyException("Cenaze yok");

            var vehicleOrgUnit = await _lookup_vehicleRepository.GetAllIncluding(x => x.CompanyFk.OrganizationUnitFk)
                .Where(x => x.Id == vehicleId)
                .Select(s => s.CompanyFk.OrganizationUnitId).FirstOrDefaultAsync();

            foreach (var f in funeral)
            {
                //TODO: Açıklama düzenlenecek
                if (f.ContractorOrgUnitId.Value != vehicleOrgUnit)
                    throw new UserFriendlyException("Yetkisiz araç");
            }

            foreach (var f in funeral)
            {
                f.VehicleId = vehicleId;
                f.Status = FuneralStatus.Appointed;
                await _funeralRepository.UpdateAsync(f);

            }
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals_Driver_Assignment)]
        public async Task FuneralDriverAssigment(int packedId, long employeePersonId)
        {
            var funeral = await _funeralRepository.GetAll().Where(x => x.FuneralPackageId == packedId).ToListAsync();
            //TODO: Açıklama düzenlenecek
            if (funeral == null)
                throw new UserFriendlyException("Cenaze yok");
            if (funeral.Count == 0)
                throw new UserFriendlyException("Cenaze yok");

            var employeeOrgUnit = await _userOrganizationUnitRepository.GetAll()
                    .Where(uou => uou.UserId == employeePersonId)
                    .Select(s => s.OrganizationUnitId).FirstOrDefaultAsync();

            /*
             E: Yetki kontrolünün düzeltilmesi gerek. Yücel kitapevi ekranından atama yapamıyor. Şuan sadece kendi ekranından atama yapabiliyor. Paketin atamasını yapamıyor.
             */

            foreach (var f in funeral)
            {
                if (f.ContractorOrgUnitId.Value != employeeOrgUnit)
                    throw new UserFriendlyException("Yetkisiz personel");
            }

            foreach (var f in funeral)
            {
                f.EmployeePersonId = employeePersonId;
                f.Status= FuneralStatus.Appointed;
                await _funeralRepository.UpdateAsync(f);

            }
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals_Funeral_Assignment)]
        public async Task FuneralAssigment(List<int> funeralIds, int contractorCompanyId, long? employeePersonId = null, int? vehicleId = null)
        {
            var funeral = await _funeralRepository
                .GetAllIncluding()
                .Include(x => x.Flights)
                    .Include(x => x.Addresses)
                        .ThenInclude(x => x.QuarterFk)
                            .ThenInclude(x => x.DistrictFk)
                            .Where(x => x.FuneralPackageId == null && funeralIds.Contains(x.Id)).ToListAsync();
            //TODO: Açıklama düzenlenecek
            if (funeral == null)
                throw new UserFriendlyException("Cenaze yok");
            if (funeral.Count == 0)
                throw new UserFriendlyException("Cenaze yok");

            var region = funeral.GroupBy(g => g.Addresses.FirstOrDefault().QuarterFk.DistrictFk.RegionId).ToList();

            if (region.Count > 1)
            {
                //TODO: Açıklama düzenlenecek
                throw new UserFriendlyException("Birden fazla bölge var");
            }

            foreach (var f in funeral)
            {
                var flight = f.Flights.LastOrDefault();

                var contract = await _lookup_contractRepository.GetAll()
                .Where(x => x.RegionId == region.FirstOrDefault().Key
                    && x.StartDate <= flight.LandingDate
                    && x.EndDate >= flight.LandingDate)
                .FirstOrDefaultAsync();
                //TODO: Açıklama düzenlenecek
                if (contract == null)
                    throw new UserFriendlyException("Sözleşme bulunamadı");
                f.ContractId = contract.Id;
            }

            

            var company = await _lookup_companyRepository.GetAsync(contractorCompanyId);

            var packedId = await _lookup_funeralPackageRepository.InsertAndGetIdAsync(new FuneralPackage { Code = company.Prefix+"-"+Guid.NewGuid().ToString("N").Substring(0,6), Status = FuneralStatus.Approved });

            foreach (var f in funeral)
            {
                f.ContractorOrgUnitId = company.OrganizationUnitId;
                f.FuneralPackageId = packedId;
                f.Status = FuneralStatus.Approved;
                await _funeralRepository.UpdateAsync(f);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            if (employeePersonId.HasValue)
            {
                await FuneralDriverAssigment(packedId, employeePersonId.Value);
            }
            if (vehicleId.HasValue)
            {
                await FuneralVehicleAssignment(packedId, vehicleId.Value);
            }

        }

        [AbpAuthorize(AppPermissions.Pages_Funerals_Edit)]
        protected virtual async Task Update(CreateOrEditFuneralDto input)
        {
            var funeral = await _funeralRepository.GetAllIncluding(x => x.Documents).FirstOrDefaultAsync(x => x.Id == (int)input.Id);

            /* Silinen ve eklenen documentleri algılama */
            /*var sameDocuments = input.Documents.FindAll(x => funeral.Documents.Any(y => y.Path == x.Path));
            var diffDocuments = funeral.Documents.FindAll(x => !sameDocuments.Any(y => y.Path == x.Path));
            var addedDocuments = input.Documents.FindAll(x => x.Id == null);

            funeral.Documents.ForEach((document) =>
            {
                if (input.Documents.Any(x => x.Id == document.Id) || diffDocuments.Any(x => x.Id == document.Id))
                {
                    return;
                }
                input.Documents.Add(ObjectMapper.Map<CreateOrEditFuneralDocumentDto>(document));
            });

            /* Var olanı silmemesi için hepsini eşitliyoruz 
            input.Documents.FindAll(x => x.Id != 0 && x.Id != null).ForEach(item =>
            {
                item.Id = 0;
            });

            foreach (var deletedDocument in diffDocuments)
            {
                await _blobStorageModule.DeleteDocumentBlob(deletedDocument.Path.Value.ToString("N"));
            }


            funeral.Documents?.Clear();
            */

            ObjectMapper.Map(input, funeral);

        }

        [AbpAuthorize(AppPermissions.Pages_Funerals_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _funeralRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFuneralsToExcel(GetAllFuneralsForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                        ? (FuneralStatus)input.StatusFilter
                        : default;

            var filteredFunerals = _funeralRepository.GetAll()
                        .Include(e => e.TypeFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.OwnerOrgUnitFk)
                        .Include(e => e.GiverOrgUnitFk)
                        .Include(e => e.ContractorOrgUnitFk)
                        .Include(e => e.EmployeePersonFk)
                        .Include(e => e.FuneralPackageFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TransferNo.Contains(input.Filter) || e.MemberNo.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Surname.Contains(input.Filter) || e.PassportNo.Contains(input.Filter) || e.LadingNo.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransferNoFilter), e => e.TransferNo.Contains(input.TransferNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MemberNoFilter), e => e.MemberNo.Contains(input.MemberNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurnameFilter), e => e.Surname.Contains(input.SurnameFilter))
                        .WhereIf(input.MinTcNoFilter != null, e => e.TcNo >= input.MinTcNoFilter)
                        .WhereIf(input.MaxTcNoFilter != null, e => e.TcNo <= input.MaxTcNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PassportNoFilter), e => e.PassportNo.Contains(input.PassportNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LadingNoFilter), e => e.LadingNo.Contains(input.LadingNoFilter))
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(input.MinOperationDateFilter != null, e => e.OperationDate >= input.MinOperationDateFilter)
                        .WhereIf(input.MaxOperationDateFilter != null, e => e.OperationDate <= input.MaxOperationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralTypeDescriptionFilter), e => e.TypeFk != null && e.TypeFk.Description == input.FuneralTypeDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.ContactFk == null || e.ContactFk.Name == null ? "" : e.ContactFk.Name.ToString()
, e.ContactFk == null || e.ContactFk.Surname == null ? "" : e.ContactFk.Surname.ToString()
, e.ContactFk == null || e.ContactFk.IdentifyNo == null ? "" : e.ContactFk.IdentifyNo.ToString()
) == input.ContactDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OwnerOrganizationUnitDisplayNameFilter), e => e.OwnerOrgUnitFk != null && e.OwnerOrgUnitFk.DisplayName == input.OwnerOrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GiverOrganizationUnitDisplayNameFilter), e => e.GiverOrgUnitFk != null && e.GiverOrgUnitFk.DisplayName == input.GiverOrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContractorOrganizationUnitDisplayNameFilter), e => e.ContractorOrgUnitFk != null && e.ContractorOrgUnitFk.DisplayName == input.ContractorOrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.EmployeePersonFk != null && e.EmployeePersonFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralPackageCodeFilter), e => e.FuneralPackageFk != null && e.FuneralPackageFk.Code == input.FuneralPackageCodeFilter);

            var query = (from o in filteredFunerals
                         join o1 in _lookup_funeralTypeRepository.GetAll() on o.TypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_organizationUnitRepository.GetAll() on o.OwnerOrgUnitId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_organizationUnitRepository.GetAll() on o.GiverOrgUnitId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_organizationUnitRepository.GetAll() on o.ContractorOrgUnitId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_userRepository.GetAll() on o.EmployeePersonId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         join o7 in _lookup_funeralPackageRepository.GetAll() on o.FuneralPackageId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetFuneralForViewDto()
                         {
                             Funeral = new FuneralDto
                             {
                                 TransferNo = o.TransferNo,
                                 MemberNo = o.MemberNo,
                                 Name = o.Name,
                                 Surname = o.Surname,
                                 TcNo = o.TcNo,
                                 PassportNo = o.PassportNo,
                                 LadingNo = o.LadingNo,
                                 Status = o.Status,
                                 OperationDate = o.OperationDate,
                                 Id = o.Id
                             },
                             FuneralTypeDescription = s1 == null || s1.Description == null ? "" : s1.Description.ToString(),
                             ContactDisplayProperty = string.Format("{0} {1} {2}", s2 == null || s2.Name == null ? "" : s2.Name.ToString()
, s2 == null || s2.Surname == null ? "" : s2.Surname.ToString()
, s2 == null || s2.IdentifyNo == null ? "" : s2.IdentifyNo.ToString()
),
                             OwnerOrganizationUnitDisplayName = s3 == null || s3.DisplayName == null ? "" : s3.DisplayName.ToString(),
                             GiverOrganizationUnitDisplayName = s4 == null || s4.DisplayName == null ? "" : s4.DisplayName.ToString(),
                             ContractorOrganizationUnitDisplayName = s5 == null || s5.DisplayName == null ? "" : s5.DisplayName.ToString(),
                             UserName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                             FuneralPackageCode = s7 == null || s7.Code == null ? "" : s7.Code.ToString()
                         });

            var funeralListDtos = await query.ToListAsync();

            return _funeralsExcelExporter.ExportToFile(funeralListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals)]
        public async Task<PagedResultDto<FuneralFuneralTypeLookupTableDto>> GetAllFuneralTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_funeralTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var funeralTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FuneralFuneralTypeLookupTableDto>();
            foreach (var funeralType in funeralTypeList)
            {
                lookupTableDtoList.Add(new FuneralFuneralTypeLookupTableDto
                {
                    Id = funeralType.Id,
                    DisplayName = funeralType.Description?.ToString()
                });
            }

            return new PagedResultDto<FuneralFuneralTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals)]
        public async Task<PagedResultDto<FuneralContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => string.Format("{0} {1} {2}", e.Name, e.Surname, e.IdentifyNo).Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FuneralContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new FuneralContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = string.Format("{0} {1} {2}", contact.Name, contact.Surname, contact.IdentifyNo)
                });
            }

            return new PagedResultDto<FuneralContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_Funerals)]
        public async Task<List<FuneralOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForTableDropdown()
        {
            return await _lookup_organizationUnitRepository.GetAll()
                .Select(organizationUnit => new FuneralOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit == null || organizationUnit.DisplayName == null ? "" : organizationUnit.DisplayName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals)]
        public async Task<List<FuneralUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new FuneralUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals)]
        public async Task<List<FuneralFuneralPackageLookupTableDto>> GetAllFuneralPackageForTableDropdown()
        {
            return await _lookup_funeralPackageRepository.GetAll()
                .Select(funeralPackage => new FuneralFuneralPackageLookupTableDto
                {
                    Id = funeralPackage.Id,
                    DisplayName = funeralPackage == null || funeralPackage.Code == null ? "" : funeralPackage.Code.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals)]
        public async Task<List<FuneralFuneralTypeLookupTableDto>> GetAllFuneralTypeForTableDropdown()
        {
            return await _lookup_funeralTypeRepository.GetAll()
                .Select(funeralType => new FuneralFuneralTypeLookupTableDto
                {
                    Id = funeralType.Id,
                    DisplayName = funeralType == null || funeralType.Description == null ? "" : funeralType.Description.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals)]
        public async Task<List<EmployeeLookupTableDto>> GetAllEmployeeForTableDropdown(int? companyId = null, string roleName = null)
        {
            long? orgUnitId = null;

            if (companyId.HasValue)
            {
                var company = await _lookup_companyRepository.GetAll().FirstOrDefaultAsync(x => x.Id == companyId);
                orgUnitId = company.OrganizationUnitId;
            }

            int? roleId = null;

            if (roleName != null)
            {
                var role = _lookup_roleRepository.GetAll().FirstOrDefault(x => x.Name == roleName);
                roleId = role?.Id;
            }

            return await _lookup_userRepository.GetAllIncluding(x => x.OrganizationUnits, x => x.Roles)
                .WhereIf(orgUnitId.HasValue, x => x.OrganizationUnits.Any(y => y.OrganizationUnitId == orgUnitId.Value))
                .WhereIf(roleId.HasValue, x => x.Roles.Any(y => y.RoleId == roleId))
                .Select(employee => new EmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.FullName
                }).ToListAsync();
        }
        [AbpAuthorize(AppPermissions.Pages_Funerals)]
        public async Task<List<VehiclesLookupTableDto>> GetAllVehicleForTableDropdown(int? companyId = null)
        {
            return await _lookup_vehicleRepository.GetAll()
                .WhereIf(companyId.HasValue, x => x.CompanyId == companyId)
                .Select(vehicle => new VehiclesLookupTableDto
                {
                    Id = vehicle.Id,
                    DisplayName = vehicle.Plate + "( " + vehicle.Capactiy + " Kapasite)"
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals)]
        public async Task<List<FuneralContractLookupTableDto>> GetAllContractForTableDropdown()
        {
            return await _lookup_contractRepository.GetAll()
                .Select(contract => new FuneralContractLookupTableDto
                {
                    Id = contract.Id,
                    DisplayName = contract == null || contract.Formule == null ? "" : contract.Formule.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Funerals)]

        public async Task<PagedResultDto<GetFuneraPackagelForViewDto>> GetAllGroupByPackage()
        {
            var userOrgUnits = _customAbpSession.OrganizationUnits;
            var userRoles = _customAbpSession.Roles;

            var filteredFunerals = _funeralRepository.GetAll()
                        .Include(e => e.TypeFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.OwnerOrgUnitFk)
                        .Include(e => e.GiverOrgUnitFk)
                        .Include(e => e.ContractorOrgUnitFk)
                        .Include(e => e.EmployeePersonFk)
                        .Include(e => e.FuneralPackageFk)
                        .Include(e => e.Flights)
                        .Include(e => e.Documents)
                        .Include(e => e.Addresses)
                        .Where(x=> x.FuneralPackageId != null && x.FuneralPackageId!=0)
                        .AsQueryable();
                        

            if (!userRoles.Contains(StaticRoleNames.Tenants.Manager))
            {
                //Eğer yönetici rolünde ise kısıt verilmeden listelenecek.
                if (userRoles.Contains(StaticRoleNames.Tenants.Employee))
                {
                    //Kitabevi ise sadece kendi sahibi olduğu cenazeleri veya kendine atanan cenazeleri görür
                    filteredFunerals = filteredFunerals.Where(x => userOrgUnits.Contains(x.GiverOrgUnitId) || userOrgUnits.Contains(x.ContractorOrgUnitId.Value));
                }
                if (userRoles.Contains(StaticRoleNames.Tenants.Driver))
                {
                    //Şöförler sadece kendine atanan cenazeleri görör
                    filteredFunerals = filteredFunerals.Where(x => x.EmployeePersonId == _customAbpSession.UserId.Value);
                }
                if (userRoles.Contains(StaticRoleNames.Tenants.Enterer))
                {
                    //Cenaze girişi yapanlar sadece kendi cenaze girişi yaptıkları cenazeleri görürler
                    filteredFunerals = filteredFunerals.Where(x => userOrgUnits.Contains(x.OwnerOrgUnitId));
                }
            }

            var funerals = await filteredFunerals.ToListAsync();
            var groupedFunerals = funerals.GroupBy(x => x.FuneralPackageId);


            return new PagedResultDto<GetFuneraPackagelForViewDto>(
                groupedFunerals.Count(),
                groupedFunerals.Select(x =>
                new GetFuneraPackagelForViewDto
                {
                    Package = ObjectMapper.Map<FuneralPackageDto>(x.First().FuneralPackageFk),
                    Funerals = ObjectMapper.Map<List<FuneralDto>>(x.ToList())
                }).ToList()
            ); 

        }

        public async Task ClearPackageFromFuneral(int funeralId)
        {
            var funeral = _funeralRepository.FirstOrDefault(funeralId);
            if (funeral == null)
            {
                throw new UserFriendlyException("Cenaze yok");
            }

            var packageID = funeral.FuneralPackageId;

            funeral.FuneralPackageId = null;
            funeral.ContractorOrgUnitId = null;
            funeral.VehicleId = null;
            funeral.EmployeePersonId = null;

            await _funeralRepository.UpdateAsync(funeral);


            var countInPackage = await _funeralRepository.GetAll().Where(x => x.FuneralPackageId == packageID).CountAsync();
            if((countInPackage-1) <= 0)
            {
                await _lookup_funeralPackageRepository.DeleteAsync(packageID.Value);
            }
            


        }

    }
}