(function () {
  $(function () {
    var _$userDetailsTable = $('#UserDetailsTable');
    var _userDetailsService = abp.services.app.userDetails;

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
        getUserDetails();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getUserDetails();
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
        getUserDetails();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getUserDetails();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.UserDetails.Create'),
      edit: abp.auth.hasPermission('Pages.UserDetails.Edit'),
      delete: abp.auth.hasPermission('Pages.UserDetails.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/UserDetails/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/UserDetails/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditUserDetailModal',
    });

    var _viewUserDetailModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/UserDetails/ViewuserDetailModal',
      modalClass: 'ViewUserDetailModal',
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

    var dataTable = _$userDetailsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _userDetailsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#UserDetailsTableFilter').val(),
            userNameFilter: $('#UserNameFilterId').val(),
            contactDisplayPropertyFilter: $('#ContactDisplayPropertyFilterId').val(),
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
                  _viewUserDetailModal.open({ id: data.record.userDetail.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.userDetail.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteUserDetail(data.record.userDetail);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'userName',
          name: 'userFk.name',
        },
        {
          targets: 3,
          data: 'contactDisplayProperty',
          name: 'contactFk.displayProperty',
        },
      ],
    });

    function getUserDetails() {
      dataTable.ajax.reload();
    }

    function deleteUserDetail(userDetail) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _userDetailsService
            .delete({
              id: userDetail.id,
            })
            .done(function () {
              getUserDetails(true);
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

    $('#CreateNewUserDetailButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _userDetailsService
        .getUserDetailsToExcel({
          filter: $('#UserDetailsTableFilter').val(),
          userNameFilter: $('#UserNameFilterId').val(),
          contactDisplayPropertyFilter: $('#ContactDisplayPropertyFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditUserDetailModalSaved', function () {
      getUserDetails();
    });

    $('#GetUserDetailsButton').click(function (e) {
      e.preventDefault();
      getUserDetails();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getUserDetails();
      }
    });

    $('.reload-on-change').change(function (e) {
      getUserDetails();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getUserDetails();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getUserDetails();
    });
  });
})();
