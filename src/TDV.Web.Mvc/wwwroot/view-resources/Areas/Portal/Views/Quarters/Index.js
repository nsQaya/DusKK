(function () {
  $(function () {
    var _$quartersTable = $('#QuartersTable');
    var _quartersService = abp.services.app.quarters;

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
        getQuarters();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getQuarters();
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
        getQuarters();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getQuarters();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Quarters.Create'),
      edit: abp.auth.hasPermission('Pages.Quarters.Edit'),
      delete: abp.auth.hasPermission('Pages.Quarters.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Quarters/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Quarters/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditQuarterModal',
    });

    var _viewQuarterModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Quarters/ViewquarterModal',
      modalClass: 'ViewQuarterModal',
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

    var dataTable = _$quartersTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _quartersService.getAll,
        inputFilter: function () {
          return {
            filter: $('#QuartersTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            minOrderFilter: $('#MinOrderFilterId').val(),
            maxOrderFilter: $('#MaxOrderFilterId').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
            districtNameFilter: $('#DistrictNameFilterId').val(),
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
                  _viewQuarterModal.open({ id: data.record.quarter.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.quarter.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteQuarter(data.record.quarter);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'quarter.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'quarter.order',
          name: 'order',
        },
        {
          targets: 4,
          data: 'quarter.isActive',
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
          data: 'districtName',
          name: 'districtFk.name',
        },
      ],
    });

    function getQuarters() {
      dataTable.ajax.reload();
    }

    function deleteQuarter(quarter) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _quartersService
            .delete({
              id: quarter.id,
            })
            .done(function () {
              getQuarters(true);
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

    $('#CreateNewQuarterButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _quartersService
        .getQuartersToExcel({
          filter: $('#QuartersTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          minOrderFilter: $('#MinOrderFilterId').val(),
          maxOrderFilter: $('#MaxOrderFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
          districtNameFilter: $('#DistrictNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditQuarterModalSaved', function () {
      getQuarters();
    });

    $('#GetQuartersButton').click(function (e) {
      e.preventDefault();
      getQuarters();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getQuarters();
      }
    });

    $('.reload-on-change').change(function (e) {
      getQuarters();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getQuarters();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getQuarters();
    });
  });
})();
