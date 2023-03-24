(function () {
  $(function () {
    var _$stoksTable = $('#StoksTable');
    var _stoksService = abp.services.app.stoks;

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
        getStoks();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getStoks();
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
        getStoks();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getStoks();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Stoks.Create'),
      edit: abp.auth.hasPermission('Pages.Stoks.Edit'),
      delete: abp.auth.hasPermission('Pages.Stoks.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Stoks/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Stoks/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditStokModal',
    });

    var _viewStokModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Stoks/ViewstokModal',
      modalClass: 'ViewStokModal',
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

    var dataTable = _$stoksTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _stoksService.getAll,
        inputFilter: function () {
          return {
            filter: $('#StoksTableFilter').val(),
            koduFilter: $('#KoduFilterId').val(),
            adiFilter: $('#AdiFilterId').val(),
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
                  _viewStokModal.open({ id: data.record.stok.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.stok.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteStok(data.record.stok);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'stok.kodu',
          name: 'kodu',
        },
        {
          targets: 3,
          data: 'stok.adi',
          name: 'adi',
        },
      ],
    });

    function getStoks() {
      dataTable.ajax.reload();
    }

    function deleteStok(stok) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _stoksService
            .delete({
              id: stok.id,
            })
            .done(function () {
              getStoks(true);
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

    $('#CreateNewStokButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _stoksService
        .getStoksToExcel({
          filter: $('#StoksTableFilter').val(),
          koduFilter: $('#KoduFilterId').val(),
          adiFilter: $('#AdiFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditStokModalSaved', function () {
      getStoks();
    });

    $('#GetStoksButton').click(function (e) {
      e.preventDefault();
      getStoks();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getStoks();
      }
    });

    $('.reload-on-change').change(function (e) {
      getStoks();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getStoks();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getStoks();
    });
  });
})();
