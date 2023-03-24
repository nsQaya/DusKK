(function () {
  $(function () {
    var _$airportsTable = $('#AirportsTable');
    var _airportsService = abp.services.app.airports;

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
        getAirports();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getAirports();
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
        getAirports();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getAirports();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Airports.Create'),
      edit: abp.auth.hasPermission('Pages.Airports.Edit'),
      delete: abp.auth.hasPermission('Pages.Airports.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Airports/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Airports/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditAirportModal',
    });

    var _viewAirportModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Airports/ViewairportModal',
      modalClass: 'ViewAirportModal',
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

    var dataTable = _$airportsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _airportsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#AirportsTableFilter').val(),
            codeFilter: $('#CodeFilterId').val(),
            nameFilter: $('#NameFilterId').val(),
            minOrderFilter: $('#MinOrderFilterId').val(),
            maxOrderFilter: $('#MaxOrderFilterId').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
            countryDisplayPropertyFilter: $('#CountryDisplayPropertyFilterId').val(),
            cityDisplayPropertyFilter: $('#CityDisplayPropertyFilterId').val(),
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
                  _viewAirportModal.open({ id: data.record.airport.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.airport.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteAirport(data.record.airport);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'airport.code',
          name: 'code',
        },
        {
          targets: 3,
          data: 'airport.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'airport.description',
          name: 'description',
        },
        {
          targets: 5,
          data: 'airport.order',
          name: 'order',
        },
        {
          targets: 6,
          data: 'airport.isActive',
          name: 'isActive',
          render: function (isActive) {
            if (isActive) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 7,
          data: 'countryDisplayProperty',
          name: 'countryFk.displayProperty',
        },
        {
          targets: 8,
          data: 'cityDisplayProperty',
          name: 'cityFk.displayProperty',
        },
      ],
    });

    function getAirports() {
      dataTable.ajax.reload();
    }

    function deleteAirport(airport) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _airportsService
            .delete({
              id: airport.id,
            })
            .done(function () {
              getAirports(true);
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

    $('#CreateNewAirportButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _airportsService
        .getAirportsToExcel({
          filter: $('#AirportsTableFilter').val(),
          codeFilter: $('#CodeFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
          minOrderFilter: $('#MinOrderFilterId').val(),
          maxOrderFilter: $('#MaxOrderFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
          countryDisplayPropertyFilter: $('#CountryDisplayPropertyFilterId').val(),
          cityDisplayPropertyFilter: $('#CityDisplayPropertyFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditAirportModalSaved', function () {
      getAirports();
    });

    $('#GetAirportsButton').click(function (e) {
      e.preventDefault();
      getAirports();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getAirports();
      }
    });

    $('.reload-on-change').change(function (e) {
      getAirports();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getAirports();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getAirports();
    });
  });
})();
