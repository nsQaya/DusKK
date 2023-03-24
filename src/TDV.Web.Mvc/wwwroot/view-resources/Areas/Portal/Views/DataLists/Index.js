(function () {
  $(function () {
    var _$dataListsTable = $('#DataListsTable');
    var _dataListsService = abp.services.app.dataLists;

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
        getDataLists();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getDataLists();
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
        getDataLists();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getDataLists();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.DataLists.Create'),
      edit: abp.auth.hasPermission('Pages.DataLists.Edit'),
      delete: abp.auth.hasPermission('Pages.DataLists.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/DataLists/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/DataLists/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditDataListModal',
    });

    var _viewDataListModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/DataLists/ViewdataListModal',
      modalClass: 'ViewDataListModal',
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

    var dataTable = _$dataListsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _dataListsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#DataListsTableFilter').val(),
            typeFilter: $('#TypeFilterId').val(),
            valueFilter: $('#ValueFilterId').val(),
            minOrderNumberFilter: $('#MinOrderNumberFilterId').val(),
            maxOrderNumberFilter: $('#MaxOrderNumberFilterId').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
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
                  _viewDataListModal.open({ id: data.record.dataList.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.dataList.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteDataList(data.record.dataList);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'dataList.type',
          name: 'type',
        },
        {
          targets: 3,
          data: 'dataList.value',
          name: 'value',
        },
        {
          targets: 4,
          data: 'dataList.orderNumber',
          name: 'orderNumber',
        },
        {
          targets: 5,
          data: 'dataList.isActive',
          name: 'isActive',
          render: function (isActive) {
            if (isActive) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
      ],
    });

    function getDataLists() {
      dataTable.ajax.reload();
    }

    function deleteDataList(dataList) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _dataListsService
            .delete({
              id: dataList.id,
            })
            .done(function () {
              getDataLists(true);
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

    $('#CreateNewDataListButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _dataListsService
        .getDataListsToExcel({
          filter: $('#DataListsTableFilter').val(),
          typeFilter: $('#TypeFilterId').val(),
          valueFilter: $('#ValueFilterId').val(),
          minOrderNumberFilter: $('#MinOrderNumberFilterId').val(),
          maxOrderNumberFilter: $('#MaxOrderNumberFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditDataListModalSaved', function () {
      getDataLists();
    });

    $('#GetDataListsButton').click(function (e) {
      e.preventDefault();
      getDataLists();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getDataLists();
      }
    });

    $('.reload-on-change').change(function (e) {
      getDataLists();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getDataLists();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getDataLists();
    });
  });
})();
