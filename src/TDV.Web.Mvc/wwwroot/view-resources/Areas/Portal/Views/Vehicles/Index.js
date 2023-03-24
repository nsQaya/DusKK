(function () {
  $(function () {
    var _$vehiclesTable = $('#VehiclesTable');
    var _vehiclesService = abp.services.app.vehicles;

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
        getVehicles();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getVehicles();
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
        getVehicles();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getVehicles();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Vehicles.Create'),
      edit: abp.auth.hasPermission('Pages.Vehicles.Edit'),
      delete: abp.auth.hasPermission('Pages.Vehicles.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Vehicles/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Vehicles/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditVehicleModal',
    });

    var _viewVehicleModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Vehicles/ViewvehicleModal',
      modalClass: 'ViewVehicleModal',
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

    var dataTable = _$vehiclesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _vehiclesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#VehiclesTableFilter').val(),
            plateFilter: $('#PlateFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            minEndExaminationDateFilter: getDateFilter($('#MinEndExaminationDateFilterId')),
            maxEndExaminationDateFilter: getMaxDateFilter($('#MaxEndExaminationDateFilterId')),
            minEndInsuranceDateFilter: getDateFilter($('#MinEndInsuranceDateFilterId')),
            maxEndInsuranceDateFilter: getMaxDateFilter($('#MaxEndInsuranceDateFilterId')),
            minEndGuarantyDateFilter: getDateFilter($('#MinEndGuarantyDateFilterId')),
            maxEndGuarantyDateFilter: getMaxDateFilter($('#MaxEndGuarantyDateFilterId')),
            minCapactiyFilter: $('#MinCapactiyFilterId').val(),
            maxCapactiyFilter: $('#MaxCapactiyFilterId').val(),
            minYearFilter: $('#MinYearFilterId').val(),
            maxYearFilter: $('#MaxYearFilterId').val(),
            brandFilter: $('#BrandFilterId').val(),
            trackNoFilter: $('#TrackNoFilterId').val(),
            companyDisplayPropertyFilter: $('#CompanyDisplayPropertyFilterId').val(),
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
                  _viewVehicleModal.open({ id: data.record.vehicle.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.vehicle.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteVehicle(data.record.vehicle);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'vehicle.plate',
          name: 'plate',
        },
        {
          targets: 3,
          data: 'vehicle.description',
          name: 'description',
        },
        {
          targets: 4,
          data: 'vehicle.endExaminationDate',
          name: 'endExaminationDate',
          render: function (endExaminationDate) {
            if (endExaminationDate) {
              return moment(endExaminationDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 5,
          data: 'vehicle.endInsuranceDate',
          name: 'endInsuranceDate',
          render: function (endInsuranceDate) {
            if (endInsuranceDate) {
              return moment(endInsuranceDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 6,
          data: 'vehicle.endGuarantyDate',
          name: 'endGuarantyDate',
          render: function (endGuarantyDate) {
            if (endGuarantyDate) {
              return moment(endGuarantyDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 7,
          data: 'vehicle.capactiy',
          name: 'capactiy',
        },
        {
          targets: 8,
          data: 'vehicle.year',
          name: 'year',
        },
        {
          targets: 9,
          data: 'vehicle.brand',
          name: 'brand',
        },
        {
          targets: 10,
          data: 'vehicle.trackNo',
          name: 'trackNo',
        },
        {
          targets: 11,
          data: 'companyDisplayProperty',
          name: 'companyFk.displayProperty',
        },
      ],
    });

    function getVehicles() {
      dataTable.ajax.reload();
    }

    function deleteVehicle(vehicle) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _vehiclesService
            .delete({
              id: vehicle.id,
            })
            .done(function () {
              getVehicles(true);
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

    $('#CreateNewVehicleButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _vehiclesService
        .getVehiclesToExcel({
          filter: $('#VehiclesTableFilter').val(),
          plateFilter: $('#PlateFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          minEndExaminationDateFilter: getDateFilter($('#MinEndExaminationDateFilterId')),
          maxEndExaminationDateFilter: getMaxDateFilter($('#MaxEndExaminationDateFilterId')),
          minEndInsuranceDateFilter: getDateFilter($('#MinEndInsuranceDateFilterId')),
          maxEndInsuranceDateFilter: getMaxDateFilter($('#MaxEndInsuranceDateFilterId')),
          minEndGuarantyDateFilter: getDateFilter($('#MinEndGuarantyDateFilterId')),
          maxEndGuarantyDateFilter: getMaxDateFilter($('#MaxEndGuarantyDateFilterId')),
          minCapactiyFilter: $('#MinCapactiyFilterId').val(),
          maxCapactiyFilter: $('#MaxCapactiyFilterId').val(),
          minYearFilter: $('#MinYearFilterId').val(),
          maxYearFilter: $('#MaxYearFilterId').val(),
          brandFilter: $('#BrandFilterId').val(),
          trackNoFilter: $('#TrackNoFilterId').val(),
          companyDisplayPropertyFilter: $('#CompanyDisplayPropertyFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditVehicleModalSaved', function () {
      getVehicles();
    });

    $('#GetVehiclesButton').click(function (e) {
      e.preventDefault();
      getVehicles();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getVehicles();
      }
    });

    $('.reload-on-change').change(function (e) {
      getVehicles();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getVehicles();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getVehicles();
    });
  });
})();
