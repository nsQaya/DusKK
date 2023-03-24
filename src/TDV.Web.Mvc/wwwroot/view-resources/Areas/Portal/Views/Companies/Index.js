(function () {
  $(function () {
    var _$companiesTable = $('#CompaniesTable');
    var _companiesService = abp.services.app.companies;

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.startDate = picker.startDate;
        getCompanies();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getCompanies();
      });

    $('.endDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.endDate = picker.startDate;
        getCompanies();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getCompanies();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Companies.Create'),
      edit: abp.auth.hasPermission('Pages.Companies.Edit'),
      delete: abp.auth.hasPermission('Pages.Companies.Delete'),
    };

    var _viewCompanyModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Companies/ViewcompanyModal',
      modalClass: 'ViewCompanyModal',
    });

    var getDateFilter = function (element) {
      if ($selectedDate.startDate == null) {
        return null;
      }
      return $selectedDate.startDate.format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if ($selectedDate.endDate == null) {
        return null;
      }
      return $selectedDate.endDate.format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$companiesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _companiesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#CompaniesTableFilter').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
            typeFilter: $('#TypeFilterId').val(),
            taxAdministrationFilter: $('#TaxAdministrationFilterId').val(),
            taxNoFilter: $('#TaxNoFilterId').val(),
            websiteFilter: $('#WebsiteFilterId').val(),
            phoneFilter: $('#PhoneFilterId').val(),
            faxFilter: $('#FaxFilterId').val(),
            emailFilter: $('#EmailFilterId').val(),
            addressFilter: $('#AddressFilterId').val(),
            runningCodeFilter: $('#RunningCodeFilterId').val(),
            prefixFilter: $('#PrefixFilterId').val(),
            OwnerOrganizationUnitDisplayName: $('#OwnerOrganizationUnitDisplayNameId').val(),
            cityDisplayPropertyFilter: $('#CityDisplayPropertyFilterId').val(),
            quarterNameFilter: $('#QuarterNameFilterId').val(),
          };
        },
      },
      columnDefs: [
        {
          className: 'control responsive',
          orderable: false,
          render: function () {
            return '';
          },
          targets: 0,
        },
        {
          width: 120,
          targets: 1,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('View'),
                action: function (data) {
                  window.location = '/Portal/Companies/ViewCompany/' + data.record.company.id;
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/Portal/Companies/CreateOrEdit/' + data.record.company.id;
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteCompany(data.record.company);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'organizationUnitDisplayName',
          name: 'name',
        },
        {
          targets: 3,
          data: 'company.isActive',
          name: 'isActive',
          render: function (isActive) {
            if (isActive) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 4,
          data: 'company.type',
          name: 'type',
          render: function (type) {
            return app.localize('Enum_CompanyType_' + type);
          },
        },
        {
          targets: 5,
          data: 'company.taxAdministration',
          name: 'taxAdministration',
        },
        {
          targets: 6,
          data: 'company.taxNo',
          name: 'taxNo',
        },
        {
          targets: 7,
          data: 'company.website',
          name: 'website',
        },
        {
          targets: 8,
          data: 'company.phone',
          name: 'phone',
        },
        {
          targets: 9,
          data: 'company.fax',
          name: 'fax',
        },
        {
          targets: 10,
          data: 'company.email',
          name: 'email',
        },
        {
          targets: 11,
          data: 'company.address',
          name: 'address',
        },
        {
          targets: 12,
          data: 'company.runningCode',
          name: 'runningCode',
        },
        {
          targets: 13,
          data: 'company.prefix',
          name: 'prefix',
        },
        {
          targets: 14,
          data: 'cityDisplayProperty',
          name: 'cityFk.displayProperty',
        },
        {
          targets: 15,
          data: 'quarterName',
          name: 'quarterFk.name',
        },
      ],
    });

    function getCompanies() {
      dataTable.ajax.reload();
    }

    function deleteCompany(company) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _companiesService
            .delete({
              id: company.id,
            })
            .done(function () {
              getCompanies(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#ShowAdvancedFiltersSpan').click(function () {
      $('#ShowAdvancedFiltersSpan').hide();
      $('#HideAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideDown();
    });

    $('#HideAdvancedFiltersSpan').click(function () {
      $('#HideAdvancedFiltersSpan').hide();
      $('#ShowAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideUp();
    });

    $('#ExportToExcelButton').click(function () {
      _companiesService
        .getCompaniesToExcel({
          filter: $('#CompaniesTableFilter').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
          typeFilter: $('#TypeFilterId').val(),
          taxAdministrationFilter: $('#TaxAdministrationFilterId').val(),
          taxNoFilter: $('#TaxNoFilterId').val(),
          websiteFilter: $('#WebsiteFilterId').val(),
          phoneFilter: $('#PhoneFilterId').val(),
          faxFilter: $('#FaxFilterId').val(),
          emailFilter: $('#EmailFilterId').val(),
          addressFilter: $('#AddressFilterId').val(),
          runningCodeFilter: $('#RunningCodeFilterId').val(),
          prefixFilter: $('#PrefixFilterId').val(),
          OwnerOrganizationUnitDisplayName: $('#OwnerOrganizationUnitDisplayNameId').val(),
          cityDisplayPropertyFilter: $('#CityDisplayPropertyFilterId').val(),
          quarterNameFilter: $('#QuarterNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditCompanyModalSaved', function () {
      getCompanies();
    });

    $('#GetCompaniesButton').click(function (e) {
      e.preventDefault();
      getCompanies();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getCompanies();
      }
    });

    $('.reload-on-change').change(function (e) {
      getCompanies();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getCompanies();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getCompanies();
    });
  });
})();
