(function () {
  $(function () {
    var _$funeralTypesTable = $('#FuneralTypesTable');
    var _funeralTypesService = abp.services.app.funeralTypes;

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
        getFuneralTypes();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getFuneralTypes();
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
        getFuneralTypes();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getFuneralTypes();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.FuneralTypes.Create'),
      edit: abp.auth.hasPermission('Pages.FuneralTypes.Edit'),
      delete: abp.auth.hasPermission('Pages.FuneralTypes.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralTypes/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/FuneralTypes/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditFuneralTypeModal',
    });

    var _viewFuneralTypeModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralTypes/ViewfuneralTypeModal',
      modalClass: 'ViewFuneralTypeModal',
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

    var dataTable = _$funeralTypesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralTypesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#FuneralTypesTableFilter').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            isDefaultFilter: $('#IsDefaultFilterId').val(),
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
                  _viewFuneralTypeModal.open({ id: data.record.funeralType.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.funeralType.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteFuneralType(data.record.funeralType);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'funeralType.description',
          name: 'description',
        },
        {
          targets: 3,
          data: 'funeralType.isDefault',
          name: 'isDefault',
          render: function (isDefault) {
            if (isDefault) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
      ],
    });

    function getFuneralTypes() {
      dataTable.ajax.reload();
    }

    function deleteFuneralType(funeralType) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _funeralTypesService
            .delete({
              id: funeralType.id,
            })
            .done(function () {
              getFuneralTypes(true);
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

    $('#CreateNewFuneralTypeButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _funeralTypesService
        .getFuneralTypesToExcel({
          filter: $('#FuneralTypesTableFilter').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          isDefaultFilter: $('#IsDefaultFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditFuneralTypeModalSaved', function () {
      getFuneralTypes();
    });

    $('#GetFuneralTypesButton').click(function (e) {
      e.preventDefault();
      getFuneralTypes();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getFuneralTypes();
      }
    });

    $('.reload-on-change').change(function (e) {
      getFuneralTypes();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getFuneralTypes();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getFuneralTypes();
    });
  });
})();
