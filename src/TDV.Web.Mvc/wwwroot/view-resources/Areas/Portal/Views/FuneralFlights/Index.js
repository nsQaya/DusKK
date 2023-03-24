(function () {
  $(function () {
    var _$funeralFlightsTable = $('#FuneralFlightsTable');
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
      viewUrl: abp.appPath + 'Portal/FuneralFlights/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/FuneralFlights/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditFuneralFlightModal',
    });

    var _viewFuneralFlightModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralFlights/ViewfuneralFlightModal',
      modalClass: 'ViewFuneralFlightModal',
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
            filter: $('#FuneralFlightsTableFilter').val(),
            noFilter: $('#NoFilterId').val(),
            codeFilter: $('#CodeFilterId').val(),
            minLiftOffDateFilter: getDateFilter($('#MinLiftOffDateFilterId')),
            maxLiftOffDateFilter: getMaxDateFilter($('#MaxLiftOffDateFilterId')),
            minLandingDateFilter: getDateFilter($('#MinLandingDateFilterId')),
            maxLandingDateFilter: getMaxDateFilter($('#MaxLandingDateFilterId')),
            funeralNameFilter: $('#FuneralNameFilterId').val(),
            airlineCompanyCodeFilter: $('#AirlineCompanyCodeFilterId').val(),
            airportNameFilter: $('#AirportNameFilterId').val(),
            airportName2Filter: $('#AirportName2FilterId').val(),
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
          data: 'funeralName',
          name: 'funeralFk.name',
        },
        {
          targets: 7,
          data: 'airlineCompanyCode',
          name: 'airlineCompanyFk.code',
        },
        {
          targets: 8,
          data: 'airportName',
          name: 'liftOffAirportFk.name',
        },
        {
          targets: 9,
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

    $('#CreateNewFuneralFlightButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _funeralFlightsService
        .getFuneralFlightsToExcel({
          filter: $('#FuneralFlightsTableFilter').val(),
          noFilter: $('#NoFilterId').val(),
          codeFilter: $('#CodeFilterId').val(),
          minLiftOffDateFilter: getDateFilter($('#MinLiftOffDateFilterId')),
          maxLiftOffDateFilter: getMaxDateFilter($('#MaxLiftOffDateFilterId')),
          minLandingDateFilter: getDateFilter($('#MinLandingDateFilterId')),
          maxLandingDateFilter: getMaxDateFilter($('#MaxLandingDateFilterId')),
          funeralNameFilter: $('#FuneralNameFilterId').val(),
          airlineCompanyCodeFilter: $('#AirlineCompanyCodeFilterId').val(),
          airportNameFilter: $('#AirportNameFilterId').val(),
          airportName2Filter: $('#AirportName2FilterId').val(),
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
