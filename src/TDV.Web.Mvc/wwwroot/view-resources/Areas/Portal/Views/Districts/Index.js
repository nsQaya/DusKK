(function () {
  $(function () {
    var _$districtsTable = $('#DistrictsTable');
    var _districtsService = abp.services.app.districts;

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
        getDistricts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getDistricts();
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
        getDistricts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getDistricts();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Districts.Create'),
      edit: abp.auth.hasPermission('Pages.Districts.Edit'),
      delete: abp.auth.hasPermission('Pages.Districts.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Districts/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Districts/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditDistrictModal',
    });

    var _viewDistrictModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Districts/ViewdistrictModal',
      modalClass: 'ViewDistrictModal',
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

    var dataTable = _$districtsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _districtsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#DistrictsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            minOrderFilter: $('#MinOrderFilterId').val(),
            maxOrderFilter: $('#MaxOrderFilterId').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
            cityDisplayPropertyFilter: $('#CityDisplayPropertyFilterId').val(),
            regionNameFilter: $('#RegionNameFilterId').val(),
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
                  _viewDistrictModal.open({ id: data.record.district.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.district.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteDistrict(data.record.district);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'district.name',
          name: 'name',
        },
        {
          targets: 3,
          data: 'district.order',
          name: 'order',
        },
        {
          targets: 4,
          data: 'district.isActive',
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
          data: 'cityDisplayProperty',
          name: 'cityFk.displayProperty',
        },
        {
          targets: 6,
          data: 'regionName',
          name: 'regionFk.name',
        },
      ],
    });

    function getDistricts() {
      dataTable.ajax.reload();
    }

    function deleteDistrict(district) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _districtsService
            .delete({
              id: district.id,
            })
            .done(function () {
              getDistricts(true);
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

    $('#CreateNewDistrictButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _districtsService
        .getDistrictsToExcel({
          filter: $('#DistrictsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          minOrderFilter: $('#MinOrderFilterId').val(),
          maxOrderFilter: $('#MaxOrderFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
          cityDisplayPropertyFilter: $('#CityDisplayPropertyFilterId').val(),
          regionNameFilter: $('#RegionNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditDistrictModalSaved', function () {
      getDistricts();
    });

    $('#GetDistrictsButton').click(function (e) {
      e.preventDefault();
      getDistricts();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getDistricts();
      }
    });

    $('.reload-on-change').change(function (e) {
      getDistricts();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getDistricts();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getDistricts();
    });
  });
})();
