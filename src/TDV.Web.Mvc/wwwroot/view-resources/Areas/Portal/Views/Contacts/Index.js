(function () {
  $(function () {
    var _$contactsTable = $('#ContactsTable');
    var _contactsService = abp.services.app.contacts;

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
        getContacts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getContacts();
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
        getContacts();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getContacts();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Contacts.Create'),
      edit: abp.auth.hasPermission('Pages.Contacts.Edit'),
      delete: abp.auth.hasPermission('Pages.Contacts.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Contacts/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Contacts/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditContactModal',
    });

    var _viewContactModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Contacts/ViewcontactModal',
      modalClass: 'ViewContactModal',
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

    var dataTable = _$contactsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _contactsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#ContactsTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            surnameFilter: $('#SurnameFilterId').val(),
            identifyNoFilter: $('#IdentifyNoFilterId').val(),
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
                  _viewContactModal.open({ id: data.record.contact.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.contact.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteContact(data.record.contact);
                },
              },
            ],
          },
        },
        {
          className: 'details-control',
          targets: 2,
          orderable: false,
          autoWidth: false,
          visible: abp.auth.hasPermission('Pages.ContactDetails'),
          render: function () {
            return `<button class="btn btn-primary btn-xs Edit_ContactDetail_ContactId">${app.localize(
              'EditContactDetail'
            )}</button>`;
          },
        },
        {
          targets: 3,
          data: 'contact.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'contact.surname',
          name: 'surname',
        },
        {
          targets: 5,
          data: 'contact.identifyNo',
          name: 'identifyNo',
        },
      ],
    });

    function getContacts() {
      dataTable.ajax.reload();
    }

    function deleteContact(contact) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _contactsService
            .delete({
              id: contact.id,
            })
            .done(function () {
              getContacts(true);
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

    $('#CreateNewContactButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _contactsService
        .getContactsToExcel({
          filter: $('#ContactsTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          surnameFilter: $('#SurnameFilterId').val(),
          identifyNoFilter: $('#IdentifyNoFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditContactModalSaved', function () {
      getContacts();
    });

    $('#GetContactsButton').click(function (e) {
      e.preventDefault();
      getContacts();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getContacts();
      }
    });

    $('.reload-on-change').change(function (e) {
      getContacts();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getContacts();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getContacts();
    });

    var currentOpenedDetailRow;
    function openDetailRow(e, url) {
      var tr = $(e).closest('tr');
      var row = dataTable.row(tr);

      if (row.child.isShown()) {
        row.child.hide();
        tr.removeClass('shown');
        currentOpenedDetailRow = null;
      } else {
        if (currentOpenedDetailRow) currentOpenedDetailRow.child.hide();

        $.get(url).then((data) => {
          row.child(data).show();
          tr.addClass('shown');
          currentOpenedDetailRow = row;
        });
      }
    }

    _$contactsTable.on('click', '.Edit_ContactDetail_ContactId', function () {
      var tr = $(this).closest('tr');
      var row = dataTable.row(tr);
      openDetailRow(this, '/Portal/MasterDetailChild_Contact_ContactDetails?ContactId=' + row.data().contact.id);
    });
  });
})();
