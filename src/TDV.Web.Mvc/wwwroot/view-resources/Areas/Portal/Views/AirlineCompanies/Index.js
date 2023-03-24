(function () {
  $(function () {
    var _$airlineCompaniesTable = $('#AirlineCompaniesTable');
    var _airlineCompaniesService = abp.services.app.airlineCompanies;

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
        getAirlineCompanies();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getAirlineCompanies();
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
        getAirlineCompanies();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getAirlineCompanies();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.AirlineCompanies.Create'),
      edit: abp.auth.hasPermission('Pages.AirlineCompanies.Edit'),
      delete: abp.auth.hasPermission('Pages.AirlineCompanies.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/AirlineCompanies/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/AirlineCompanies/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditAirlineCompanyModal',
    });

    var _viewAirlineCompanyModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/AirlineCompanies/ViewairlineCompanyModal',
      modalClass: 'ViewAirlineCompanyModal',
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

    var dataTable = _$airlineCompaniesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _airlineCompaniesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#AirlineCompaniesTableFilter').val(),
            codeFilter: $('#CodeFilterId').val(),
            ladingPrefixFilter: $('#LadingPrefixFilterId').val(),
            flightPrefixFilter: $('#FlightPrefixFilterId').val(),
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
                  _viewAirlineCompanyModal.open({ id: data.record.airlineCompany.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.airlineCompany.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteAirlineCompany(data.record.airlineCompany);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'airlineCompany.code',
          name: 'code',
        },
        {
          targets: 3,
          data: 'airlineCompany.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'airlineCompany.ladingPrefix',
          name: 'ladingPrefix',
        },
        {
          targets: 5,
          data: 'airlineCompany.flightPrefix',
          name: 'flightPrefix',
        },
      ],
    });

    function getAirlineCompanies() {
      dataTable.ajax.reload();
    }

    function deleteAirlineCompany(airlineCompany) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _airlineCompaniesService
            .delete({
              id: airlineCompany.id,
            })
            .done(function () {
              getAirlineCompanies(true);
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

    $('#CreateNewAirlineCompanyButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _airlineCompaniesService
        .getAirlineCompaniesToExcel({
          filter: $('#AirlineCompaniesTableFilter').val(),
          codeFilter: $('#CodeFilterId').val(),
          ladingPrefixFilter: $('#LadingPrefixFilterId').val(),
          flightPrefixFilter: $('#FlightPrefixFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditAirlineCompanyModalSaved', function () {
      getAirlineCompanies();
    });

    $('#GetAirlineCompaniesButton').click(function (e) {
      e.preventDefault();
      getAirlineCompanies();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getAirlineCompanies();
      }
    });

    $('.reload-on-change').change(function (e) {
      getAirlineCompanies();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getAirlineCompanies();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getAirlineCompanies();
    });
  });
})();
