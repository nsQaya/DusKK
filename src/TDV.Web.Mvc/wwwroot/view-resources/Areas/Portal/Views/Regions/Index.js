(function () {
  $(function () {
    var _$regionsTable = $('#RegionsTable');
    var _regionsService = abp.services.app.regions;

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
        getRegions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getRegions();
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
        getRegions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getRegions();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Regions.Create'),
      edit: abp.auth.hasPermission('Pages.Regions.Edit'),
      delete: abp.auth.hasPermission('Pages.Regions.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Regions/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Regions/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditRegionModal',
    });

    var _viewRegionModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Regions/ViewregionModal',
      modalClass: 'ViewRegionModal',
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

    var dataTable = _$regionsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _regionsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#RegionsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            minOrderFilter: $('#MinOrderFilterId').val(),
            maxOrderFilter: $('#MaxOrderFilterId').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
            fixedPriceNameFilter: $('#FixedPriceNameFilterId').val(),
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
                  _viewRegionModal.open({ id: data.record.region.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.region.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteRegion(data.record.region);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'region.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'region.order',
          name: 'order',
        },
        {
          targets: 4,
          data: 'region.isActive',
          name: 'isActive',
          render: function (isActive) {
            if (isActive) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 5,
          data: 'fixedPriceName',
          name: 'fixedPriceFk.name',
        },
      ],
    });

    function getRegions() {
      dataTable.ajax.reload();
    }

    function deleteRegion(region) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _regionsService
            .delete({
              id: region.id,
            })
            .done(function () {
              getRegions(true);
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

    $('#CreateNewRegionButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _regionsService
        .getRegionsToExcel({
          filter: $('#RegionsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          minOrderFilter: $('#MinOrderFilterId').val(),
          maxOrderFilter: $('#MaxOrderFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
          fixedPriceNameFilter: $('#FixedPriceNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditRegionModalSaved', function () {
      getRegions();
    });

    $('#GetRegionsButton').click(function (e) {
      e.preventDefault();
      getRegions();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getRegions();
      }
    });

    $('.reload-on-change').change(function (e) {
      getRegions();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getRegions();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getRegions();
    });
  });
})();
