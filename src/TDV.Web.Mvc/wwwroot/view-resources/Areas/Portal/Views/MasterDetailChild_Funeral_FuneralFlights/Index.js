(function () {
  $(function () {
    var _$funeralFlightsTable = $('#MasterDetailChild_Funeral_FuneralFlightsTable');
    var _funeralFlightsService = abp.services.app.funeralFlights;

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
        getFuneralFlights();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getFuneralFlights();
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
        getFuneralFlights();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getFuneralFlights();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.FuneralFlights.Create'),
      edit: abp.auth.hasPermission('Pages.FuneralFlights.Edit'),
      delete: abp.auth.hasPermission('Pages.FuneralFlights.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/MasterDetailChild_Funeral_FuneralFlights/CreateOrEditModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/MasterDetailChild_Funeral_FuneralFlights/_CreateOrEditModal.js',
      modalClass: 'MasterDetailChild_Funeral_CreateOrEditFuneralFlightModal',
    });

    var _viewFuneralFlightModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/MasterDetailChild_Funeral_FuneralFlights/ViewfuneralFlightModal',
      modalClass: 'MasterDetailChild_Funeral_ViewFuneralFlightModal',
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

    var dataTable = _$funeralFlightsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralFlightsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#MasterDetailChild_Funeral_FuneralFlightsTableFilter').val(),
            noFilter: $('#MasterDetailChild_Funeral_NoFilterId').val(),
            codeFilter: $('#MasterDetailChild_Funeral_CodeFilterId').val(),
            minLiftOffDateFilter: getDateFilter($('#MasterDetailChild_Funeral_MinLiftOffDateFilterId')),
            maxLiftOffDateFilter: getMaxDateFilter($('#MasterDetailChild_Funeral_MaxLiftOffDateFilterId')),
            minLandingDateFilter: getDateFilter($('#MasterDetailChild_Funeral_MinLandingDateFilterId')),
            maxLandingDateFilter: getMaxDateFilter($('#MasterDetailChild_Funeral_MaxLandingDateFilterId')),
            airlineCompanyCodeFilter: $('#MasterDetailChild_Funeral_AirlineCompanyCodeFilterId').val(),
            airportNameFilter: $('#MasterDetailChild_Funeral_AirportNameFilterId').val(),
            airportName2Filter: $('#MasterDetailChild_Funeral_AirportName2FilterId').val(),
            funeralIdFilter: $('#MasterDetailChild_Funeral_FuneralFlightsId').val(),
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
                  _viewFuneralFlightModal.open({ id: data.record.funeralFlight.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.funeralFlight.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteFuneralFlight(data.record.funeralFlight);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'funeralFlight.no',
          name: 'no',
        },
        {
          targets: 3,
          data: 'funeralFlight.code',
          name: 'code',
        },
        {
          targets: 4,
          data: 'funeralFlight.liftOffDate',
          name: 'liftOffDate',
          render: function (liftOffDate) {
            if (liftOffDate) {
              return moment(liftOffDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 5,
          data: 'funeralFlight.landingDate',
          name: 'landingDate',
          render: function (landingDate) {
            if (landingDate) {
              return moment(landingDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 6,
          data: 'airlineCompanyCode',
          name: 'airlineCompanyFk.code',
        },
        {
          targets: 7,
          data: 'airportName',
          name: 'liftOffAirportFk.name',
        },
        {
          targets: 8,
          data: 'airportName2',
          name: 'langingAirportFk.name',
        },
      ],
    });

    function getFuneralFlights() {
      dataTable.ajax.reload();
    }

    function deleteFuneralFlight(funeralFlight) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _funeralFlightsService
            .delete({
              id: funeralFlight.id,
            })
            .done(function () {
              getFuneralFlights(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#MasterDetailChild_Funeral_ShowAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_Funeral_ShowAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_Funeral_HideAdvancedFiltersSpan').show();
      $('#MasterDetailChild_Funeral_AdvacedAuditFiltersArea').slideDown();
    });

    $('#MasterDetailChild_Funeral_HideAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_Funeral_HideAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_Funeral_ShowAdvancedFiltersSpan').show();
      $('#MasterDetailChild_Funeral_AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewFuneralFlightButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _funeralFlightsService
        .getFuneralFlightsToExcel({
          filter: $('#FuneralFlightsTableFilter').val(),
          noFilter: $('#MasterDetailChild_Funeral_NoFilterId').val(),
          codeFilter: $('#MasterDetailChild_Funeral_CodeFilterId').val(),
          minLiftOffDateFilter: getDateFilter($('#MasterDetailChild_Funeral_MinLiftOffDateFilterId')),
          maxLiftOffDateFilter: getMaxDateFilter($('#MasterDetailChild_Funeral_MaxLiftOffDateFilterId')),
          minLandingDateFilter: getDateFilter($('#MasterDetailChild_Funeral_MinLandingDateFilterId')),
          maxLandingDateFilter: getMaxDateFilter($('#MasterDetailChild_Funeral_MaxLandingDateFilterId')),
          airlineCompanyCodeFilter: $('#MasterDetailChild_Funeral_AirlineCompanyCodeFilterId').val(),
          airportNameFilter: $('#MasterDetailChild_Funeral_AirportNameFilterId').val(),
          airportName2Filter: $('#MasterDetailChild_Funeral_AirportName2FilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditFuneralFlightModalSaved', function () {
      getFuneralFlights();
    });

    $('#GetFuneralFlightsButton').click(function (e) {
      e.preventDefault();
      getFuneralFlights();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getFuneralFlights();
      }
    });

    $('.reload-on-change').change(function (e) {
      getFuneralFlights();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getFuneralFlights();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getFuneralFlights();
    });
  });
})();
