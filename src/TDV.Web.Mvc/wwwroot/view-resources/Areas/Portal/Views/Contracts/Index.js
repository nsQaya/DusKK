(function () {
  $(function () {
    var _$contractsTable = $('#ContractsTable');
    var _contractsService = abp.services.app.contracts;

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
        getContracts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getContracts();
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
        getContracts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getContracts();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Contracts.Create'),
      edit: abp.auth.hasPermission('Pages.Contracts.Edit'),
      delete: abp.auth.hasPermission('Pages.Contracts.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Contracts/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Contracts/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditContractModal',
    });

    var _viewContractModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Contracts/ViewcontractModal',
      modalClass: 'ViewContractModal',
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

    var dataTable = _$contractsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _contractsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ContractsTableFilter').val(),
            minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
            maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
            minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
            maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
            currencyTypeFilter: $('#CurrencyTypeFilterId').val(),
            regionNameFilter: $('#RegionNameFilterId').val(),
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
                  _viewContractModal.open({ id: data.record.contract.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.contract.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteContract(data.record.contract);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'contract.formule',
          name: 'formule',
        },
        {
          targets: 3,
          data: 'contract.startDate',
          name: 'startDate',
          render: function (startDate) {
            if (startDate) {
              return moment(startDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 4,
          data: 'contract.endDate',
          name: 'endDate',
          render: function (endDate) {
            if (endDate) {
              return moment(endDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 5,
          data: 'contract.currencyType',
          name: 'currencyType',
          render: function (currencyType) {
            return app.localize('Enum_CurrencyType_' + currencyType);
          },
        },
        {
          targets: 6,
          data: 'regionName',
          name: 'regionFk.name',
        },
        {
          targets: 7,
          data: 'companyDisplayProperty',
          name: 'companyFk.displayProperty',
        },
      ],
    });

    function getContracts() {
      dataTable.ajax.reload();
    }

    function deleteContract(contract) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _contractsService
            .delete({
              id: contract.id,
            })
            .done(function () {
              getContracts(true);
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

    $('#CreateNewContractButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _contractsService
        .getContractsToExcel({
          filter: $('#ContractsTableFilter').val(),
          minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
          maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
          minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
          maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
          currencyTypeFilter: $('#CurrencyTypeFilterId').val(),
          regionNameFilter: $('#RegionNameFilterId').val(),
          companyDisplayPropertyFilter: $('#CompanyDisplayPropertyFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditContractModalSaved', function () {
      getContracts();
    });

    $('#GetContractsButton').click(function (e) {
      e.preventDefault();
      getContracts();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getContracts();
      }
    });

    $('.reload-on-change').change(function (e) {
      getContracts();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getContracts();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getContracts();
    });
  });
})();
