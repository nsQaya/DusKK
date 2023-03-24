(function () {
  $(function () {
    var _$contactDetailsTable = $('#MasterDetailChild_Contact_ContactDetailsTable');
    var _contactDetailsService = abp.services.app.contactDetails;

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
        getContactDetails();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getContactDetails();
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
        getContactDetails();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getContactDetails();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.ContactDetails.Create'),
      edit: abp.auth.hasPermission('Pages.ContactDetails.Edit'),
      delete: abp.auth.hasPermission('Pages.ContactDetails.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/MasterDetailChild_Contact_ContactDetails/CreateOrEditModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/MasterDetailChild_Contact_ContactDetails/_CreateOrEditModal.js',
      modalClass: 'MasterDetailChild_Contact_CreateOrEditContactDetailModal',
    });

    var _viewContactDetailModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/MasterDetailChild_Contact_ContactDetails/ViewcontactDetailModal',
      modalClass: 'MasterDetailChild_Contact_ViewContactDetailModal',
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

    var dataTable = _$contactDetailsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _contactDetailsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#MasterDetailChild_Contact_ContactDetailsTableFilter').val(),
            typeFilter: $('#MasterDetailChild_Contact_TypeFilterId').val(),
            valueFilter: $('#MasterDetailChild_Contact_ValueFilterId').val(),
            contactIdFilter: $('#MasterDetailChild_Contact_ContactDetailsId').val(),
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
                  _viewContactDetailModal.open({ id: data.record.contactDetail.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.contactDetail.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteContactDetail(data.record.contactDetail);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'contactDetail.type',
          name: 'type',
          render: function (type) {
            return app.localize('Enum_ContactType_' + type);
          },
        },
        {
          targets: 3,
          data: 'contactDetail.value',
          name: 'value',
        },
      ],
    });

    function getContactDetails() {
      dataTable.ajax.reload();
    }

    function deleteContactDetail(contactDetail) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _contactDetailsService
            .delete({
              id: contactDetail.id,
            })
            .done(function () {
              getContactDetails(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#MasterDetailChild_Contact_ShowAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_Contact_ShowAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_Contact_HideAdvancedFiltersSpan').show();
      $('#MasterDetailChild_Contact_AdvacedAuditFiltersArea').slideDown();
    });

    $('#MasterDetailChild_Contact_HideAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_Contact_HideAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_Contact_ShowAdvancedFiltersSpan').show();
      $('#MasterDetailChild_Contact_AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewContactDetailButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _contactDetailsService
        .getContactDetailsToExcel({
          filter: $('#ContactDetailsTableFilter').val(),
          typeFilter: $('#MasterDetailChild_Contact_TypeFilterId').val(),
          valueFilter: $('#MasterDetailChild_Contact_ValueFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditContactDetailModalSaved', function () {
      getContactDetails();
    });

    $('#GetContactDetailsButton').click(function (e) {
      e.preventDefault();
      getContactDetails();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getContactDetails();
      }
    });

    $('.reload-on-change').change(function (e) {
      getContactDetails();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getContactDetails();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getContactDetails();
    });
  });
})();
