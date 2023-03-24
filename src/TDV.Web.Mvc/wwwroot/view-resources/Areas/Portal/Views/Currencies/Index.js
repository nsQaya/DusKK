(function () {
  $(function () {
    var _$currenciesTable = $('#CurrenciesTable');
    var _currenciesService = abp.services.app.currencies;

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
        getCurrencies();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getCurrencies();
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
        getCurrencies();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getCurrencies();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Currencies.Create'),
      edit: abp.auth.hasPermission('Pages.Currencies.Edit'),
      delete: abp.auth.hasPermission('Pages.Currencies.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Currencies/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Currencies/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditCurrencyModal',
    });

    var _viewCurrencyModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Currencies/ViewcurrencyModal',
      modalClass: 'ViewCurrencyModal',
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

    var dataTable = _$currenciesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _currenciesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#CurrenciesTableFilter').val(),
            codeFilter: $('#CodeFilterId').val(),
            symbolFilter: $('#SymbolFilterId').val(),
            minOrderNumberFilter: $('#MinOrderNumberFilterId').val(),
            maxOrderNumberFilter: $('#MaxOrderNumberFilterId').val(),
            minNumberCodeFilter: $('#MinNumberCodeFilterId').val(),
            maxNumberCodeFilter: $('#MaxNumberCodeFilterId').val(),
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
                  _viewCurrencyModal.open({ id: data.record.currency.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.currency.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteCurrency(data.record.currency);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'currency.code',
          name: 'code',
        },
        {
          targets: 3,
          data: 'currency.symbol',
          name: 'symbol',
        },
        {
          targets: 4,
          data: 'currency.orderNumber',
          name: 'orderNumber',
        },
        {
          targets: 5,
          data: 'currency.numberCode',
          name: 'numberCode',
        },
        {
          targets: 6,
          data: 'currency.isActive',
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

    function getCurrencies() {
      dataTable.ajax.reload();
    }

    function deleteCurrency(currency) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _currenciesService
            .delete({
              id: currency.id,
            })
            .done(function () {
              getCurrencies(true);
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

    $('#CreateNewCurrencyButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _currenciesService
        .getCurrenciesToExcel({
          filter: $('#CurrenciesTableFilter').val(),
          codeFilter: $('#CodeFilterId').val(),
          symbolFilter: $('#SymbolFilterId').val(),
          minOrderNumberFilter: $('#MinOrderNumberFilterId').val(),
          maxOrderNumberFilter: $('#MaxOrderNumberFilterId').val(),
          minNumberCodeFilter: $('#MinNumberCodeFilterId').val(),
          maxNumberCodeFilter: $('#MaxNumberCodeFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditCurrencyModalSaved', function () {
      getCurrencies();
    });

    $('#GetCurrenciesButton').click(function (e) {
      e.preventDefault();
      getCurrencies();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getCurrencies();
      }
    });

    $('.reload-on-change').change(function (e) {
      getCurrencies();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getCurrencies();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getCurrencies();
    });
  });
})();
