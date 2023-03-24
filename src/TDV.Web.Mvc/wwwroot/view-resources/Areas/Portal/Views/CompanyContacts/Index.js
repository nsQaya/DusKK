(function () {
  $(function () {
    var _$companyContactsTable = $('#CompanyContactsTable');
    var _companyContactsService = abp.services.app.companyContacts;

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
        getCompanyContacts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getCompanyContacts();
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
        getCompanyContacts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getCompanyContacts();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.CompanyContacts.Create'),
      edit: abp.auth.hasPermission('Pages.CompanyContacts.Edit'),
      delete: abp.auth.hasPermission('Pages.CompanyContacts.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyContacts/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/CompanyContacts/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditCompanyContactModal',
    });

    var _viewCompanyContactModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyContacts/ViewcompanyContactModal',
      modalClass: 'ViewCompanyContactModal',
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

    var dataTable = _$companyContactsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _companyContactsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#CompanyContactsTableFilter').val(),
            titleFilter: $('#TitleFilterId').val(),
            companyDisplayPropertyFilter: $('#CompanyDisplayPropertyFilterId').val(),
            contactNameFilter: $('#ContactNameFilterId').val(),
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
                  _viewCompanyContactModal.open({ id: data.record.companyContact.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.companyContact.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteCompanyContact(data.record.companyContact);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'companyContact.title',
          name: 'title',
        },
        {
          targets: 3,
          data: 'companyDisplayProperty',
          name: 'companyFk.displayProperty',
        },
        {
          targets: 4,
          data: 'contactName',
          name: 'contactFk.name',
        },
      ],
    });

    function getCompanyContacts() {
      dataTable.ajax.reload();
    }

    function deleteCompanyContact(companyContact) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _companyContactsService
            .delete({
              id: companyContact.id,
            })
            .done(function () {
              getCompanyContacts(true);
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

    $('#CreateNewCompanyContactButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _companyContactsService
        .getCompanyContactsToExcel({
          filter: $('#CompanyContactsTableFilter').val(),
          titleFilter: $('#TitleFilterId').val(),
          companyDisplayPropertyFilter: $('#CompanyDisplayPropertyFilterId').val(),
          contactNameFilter: $('#ContactNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditCompanyContactModalSaved', function () {
      getCompanyContacts();
    });

    $('#GetCompanyContactsButton').click(function (e) {
      e.preventDefault();
      getCompanyContacts();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getCompanyContacts();
      }
    });

    $('.reload-on-change').change(function (e) {
      getCompanyContacts();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getCompanyContacts();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getCompanyContacts();
    });
  });
})();
