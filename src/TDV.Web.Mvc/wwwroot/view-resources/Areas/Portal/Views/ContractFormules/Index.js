(function () {
  $(function () {
    var _$contractFormulesTable = $('#ContractFormulesTable');
    var _contractFormulesService = abp.services.app.contractFormules;

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
        getContractFormules();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getContractFormules();
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
        getContractFormules();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getContractFormules();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.ContractFormules.Create'),
      edit: abp.auth.hasPermission('Pages.ContractFormules.Edit'),
      delete: abp.auth.hasPermission('Pages.ContractFormules.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/ContractFormules/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/ContractFormules/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditContractFormuleModal',
    });

    var _viewContractFormuleModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/ContractFormules/ViewcontractFormuleModal',
      modalClass: 'ViewContractFormuleModal',
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

    var dataTable = _$contractFormulesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _contractFormulesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ContractFormulesTableFilter').val(),
            formuleFilter: $('#FormuleFilterId').val(),
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
                  _viewContractFormuleModal.open({ id: data.record.contractFormule.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.contractFormule.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteContractFormule(data.record.contractFormule);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'contractFormule.formule',
          name: 'formule',
        },
        {
          targets: 3,
          data: 'contractFormule.description',
          name: 'description',
        },
      ],
    });

    function getContractFormules() {
      dataTable.ajax.reload();
    }

    function deleteContractFormule(contractFormule) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _contractFormulesService
            .delete({
              id: contractFormule.id,
            })
            .done(function () {
              getContractFormules(true);
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

    $('#CreateNewContractFormuleButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _contractFormulesService
        .getContractFormulesToExcel({
          filter: $('#ContractFormulesTableFilter').val(),
          formuleFilter: $('#FormuleFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditContractFormuleModalSaved', function () {
      getContractFormules();
    });

    $('#GetContractFormulesButton').click(function (e) {
      e.preventDefault();
      getContractFormules();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getContractFormules();
      }
    });

    $('.reload-on-change').change(function (e) {
      getContractFormules();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getContractFormules();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getContractFormules();
    });
  });
})();
