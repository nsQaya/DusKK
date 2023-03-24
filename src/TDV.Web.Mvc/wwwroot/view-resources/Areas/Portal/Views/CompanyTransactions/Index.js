(function () {
  $(function () {
    var _$companyTransactionsTable = $('#CompanyTransactionsTable');
    var _companyTransactionsService = abp.services.app.companyTransactions;

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
        getCompanyTransactions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getCompanyTransactions();
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
        getCompanyTransactions();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getCompanyTransactions();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.CompanyTransactions.Create'),
      edit: abp.auth.hasPermission('Pages.CompanyTransactions.Edit'),
      delete: abp.auth.hasPermission('Pages.CompanyTransactions.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyTransactions/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/CompanyTransactions/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditCompanyTransactionModal',
    });

    var _viewCompanyTransactionModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyTransactions/ViewcompanyTransactionModal',
      modalClass: 'ViewCompanyTransactionModal',
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

    var dataTable = _$companyTransactionsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _companyTransactionsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#CompanyTransactionsTableFilter').val(),
            inOutFilter: $('#InOutFilterId').val(),
            minDateFilter: getDateFilter($('#MinDateFilterId')),
            maxDateFilter: getMaxDateFilter($('#MaxDateFilterId')),
            noFilter: $('#NoFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            minAmountFilter: $('#MinAmountFilterId').val(),
            maxAmountFilter: $('#MaxAmountFilterId').val(),
            minPriceFilter: $('#MinPriceFilterId').val(),
            maxPriceFilter: $('#MaxPriceFilterId').val(),
            minTaxRateFilter: $('#MinTaxRateFilterId').val(),
            maxTaxRateFilter: $('#MaxTaxRateFilterId').val(),
            minTotalFilter: $('#MinTotalFilterId').val(),
            maxTotalFilter: $('#MaxTotalFilterId').val(),
            isTransferredFilter: $('#IsTransferredFilterId').val(),
            companyTaxAdministrationFilter: $('#CompanyTaxAdministrationFilterId').val(),
            funeralDisplayPropertyFilter: $('#FuneralDisplayPropertyFilterId').val(),
            dataListValueFilter: $('#DataListValueFilterId').val(),
            currencyCodeFilter: $('#CurrencyCodeFilterId').val(),
            dataListValue2Filter: $('#DataListValue2FilterId').val(),
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
                  _viewCompanyTransactionModal.open({ id: data.record.companyTransaction.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.companyTransaction.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteCompanyTransaction(data.record.companyTransaction);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'companyTransaction.inOut',
          name: 'inOut',
        },
        {
          targets: 3,
          data: 'companyTransaction.date',
          name: 'date',
          render: function (date) {
            if (date) {
              return moment(date).format('L');
            }
            return '';
          },
        },
        {
          targets: 4,
          data: 'companyTransaction.no',
          name: 'no',
        },
        {
          targets: 5,
          data: 'companyTransaction.description',
          name: 'description',
        },
        {
          targets: 6,
          data: 'companyTransaction.amount',
          name: 'amount',
        },
        {
          targets: 7,
          data: 'companyTransaction.price',
          name: 'price',
        },
        {
          targets: 8,
          data: 'companyTransaction.taxRate',
          name: 'taxRate',
        },
        {
          targets: 9,
          data: 'companyTransaction.total',
          name: 'total',
        },
        {
          targets: 10,
          data: 'companyTransaction.isTransferred',
          name: 'isTransferred',
          render: function (isTransferred) {
            if (isTransferred) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 11,
          data: 'companyTaxAdministration',
          name: 'companyFk.taxAdministration',
        },
        {
          targets: 12,
          data: 'funeralDisplayProperty',
          name: 'funeralFk.displayProperty',
        },
        {
          targets: 13,
          data: 'dataListValue',
          name: 'typeFk.value',
        },
        {
          targets: 14,
          data: 'currencyCode',
          name: 'currencyFk.code',
        },
        {
          targets: 15,
          data: 'dataListValue2',
          name: 'unitTypeFk.value',
        },
      ],
    });

    function getCompanyTransactions() {
      dataTable.ajax.reload();
    }

    function deleteCompanyTransaction(companyTransaction) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _companyTransactionsService
            .delete({
              id: companyTransaction.id,
            })
            .done(function () {
              getCompanyTransactions(true);
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

    $('#CreateNewCompanyTransactionButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _companyTransactionsService
        .getCompanyTransactionsToExcel({
          filter: $('#CompanyTransactionsTableFilter').val(),
          inOutFilter: $('#InOutFilterId').val(),
          minDateFilter: getDateFilter($('#MinDateFilterId')),
          maxDateFilter: getMaxDateFilter($('#MaxDateFilterId')),
          noFilter: $('#NoFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          minAmountFilter: $('#MinAmountFilterId').val(),
          maxAmountFilter: $('#MaxAmountFilterId').val(),
          minPriceFilter: $('#MinPriceFilterId').val(),
          maxPriceFilter: $('#MaxPriceFilterId').val(),
          minTaxRateFilter: $('#MinTaxRateFilterId').val(),
          maxTaxRateFilter: $('#MaxTaxRateFilterId').val(),
          minTotalFilter: $('#MinTotalFilterId').val(),
          maxTotalFilter: $('#MaxTotalFilterId').val(),
          isTransferredFilter: $('#IsTransferredFilterId').val(),
          companyTaxAdministrationFilter: $('#CompanyTaxAdministrationFilterId').val(),
          funeralDisplayPropertyFilter: $('#FuneralDisplayPropertyFilterId').val(),
          dataListValueFilter: $('#DataListValueFilterId').val(),
          currencyCodeFilter: $('#CurrencyCodeFilterId').val(),
          dataListValue2Filter: $('#DataListValue2FilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditCompanyTransactionModalSaved', function () {
      getCompanyTransactions();
    });

    $('#GetCompanyTransactionsButton').click(function (e) {
      e.preventDefault();
      getCompanyTransactions();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getCompanyTransactions();
      }
    });

    $('.reload-on-change').change(function (e) {
      getCompanyTransactions();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getCompanyTransactions();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getCompanyTransactions();
    });
  });
})();
