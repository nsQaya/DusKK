(function () {
  $(function () {
    var _$stokOlcusTable = $('#StokOlcusTable');
    var _stokOlcusService = abp.services.app.stokOlcus;

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
        getStokOlcus();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getStokOlcus();
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
        getStokOlcus();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getStokOlcus();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.StokOlcus.Create'),
      edit: abp.auth.hasPermission('Pages.StokOlcus.Edit'),
      delete: abp.auth.hasPermission('Pages.StokOlcus.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/StokOlcus/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/StokOlcus/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditStokOlcuModal',
    });

    var _viewStokOlcuModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/StokOlcus/ViewstokOlcuModal',
      modalClass: 'ViewStokOlcuModal',
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

    var dataTable = _$stokOlcusTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _stokOlcusService.getAll,
        inputFilter: function () {
          return {
            filter: $('#StokOlcusTableFilter').val(),
            minAltFilter: $('#MinAltFilterId').val(),
            maxAltFilter: $('#MaxAltFilterId').val(),
            minUstFilter: $('#MinUstFilterId').val(),
            maxUstFilter: $('#MaxUstFilterId').val(),
            degerFilter: $('#DegerFilterId').val(),
            stokAdiFilter: $('#StokAdiFilterId').val(),
            olcumOlcuTipiFilter: $('#OlcumOlcuTipiFilterId').val(),
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
                  _viewStokOlcuModal.open({ id: data.record.stokOlcu.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.stokOlcu.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteStokOlcu(data.record.stokOlcu);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'stokOlcu.alt',
          name: 'alt',
        },
        {
          targets: 3,
          data: 'stokOlcu.ust',
          name: 'ust',
        },
        {
          targets: 4,
          data: 'stokOlcu.deger',
          name: 'deger',
        },
        {
          targets: 5,
          data: 'stokAdi',
          name: 'stokFk.adi',
        },
        {
          targets: 6,
          data: 'olcumOlcuTipi',
          name: 'olcumFk.olcuTipi',
        },
      ],
    });

    function getStokOlcus() {
      dataTable.ajax.reload();
    }

    function deleteStokOlcu(stokOlcu) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _stokOlcusService
            .delete({
              id: stokOlcu.id,
            })
            .done(function () {
              getStokOlcus(true);
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

    $('#CreateNewStokOlcuButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _stokOlcusService
        .getStokOlcusToExcel({
          filter: $('#StokOlcusTableFilter').val(),
          minAltFilter: $('#MinAltFilterId').val(),
          maxAltFilter: $('#MaxAltFilterId').val(),
          minUstFilter: $('#MinUstFilterId').val(),
          maxUstFilter: $('#MaxUstFilterId').val(),
          degerFilter: $('#DegerFilterId').val(),
          stokAdiFilter: $('#StokAdiFilterId').val(),
          olcumOlcuTipiFilter: $('#OlcumOlcuTipiFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditStokOlcuModalSaved', function () {
      getStokOlcus();
    });

    $('#GetStokOlcusButton').click(function (e) {
      e.preventDefault();
      getStokOlcus();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getStokOlcus();
      }
    });

    $('.reload-on-change').change(function (e) {
      getStokOlcus();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getStokOlcus();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getStokOlcus();
    });
  });
})();
