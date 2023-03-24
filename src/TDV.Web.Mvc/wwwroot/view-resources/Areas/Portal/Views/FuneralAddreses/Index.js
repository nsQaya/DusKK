(function () {
  $(function () {
    var _$funeralAddresesTable = $('#FuneralAddresesTable');
    var _funeralAddresesService = abp.services.app.funeralAddreses;

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
        getFuneralAddreses();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getFuneralAddreses();
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
        getFuneralAddreses();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getFuneralAddreses();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.FuneralAddreses.Create'),
      edit: abp.auth.hasPermission('Pages.FuneralAddreses.Edit'),
      delete: abp.auth.hasPermission('Pages.FuneralAddreses.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralAddreses/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/FuneralAddreses/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditFuneralAddresModal',
    });

    var _viewFuneralAddresModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralAddreses/ViewfuneralAddresModal',
      modalClass: 'ViewFuneralAddresModal',
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

    var dataTable = _$funeralAddresesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralAddresesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#FuneralAddresesTableFilter').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
            addressFilter: $('#AddressFilterId').val(),
            funeralDisplayPropertyFilter: $('#FuneralDisplayPropertyFilterId').val(),
            quarterNameFilter: $('#QuarterNameFilterId').val(),
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
                  _viewFuneralAddresModal.open({ id: data.record.funeralAddres.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.funeralAddres.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteFuneralAddres(data.record.funeralAddres);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'funeralAddres.description',
          name: 'description',
        },
        {
          targets: 3,
          data: 'funeralAddres.address',
          name: 'address',
        },
        {
          targets: 4,
          data: 'funeralDisplayProperty',
          name: 'funeralFk.displayProperty',
        },
        {
          targets: 5,
          data: 'quarterName',
          name: 'quarterFk.name',
        },
      ],
    });

    function getFuneralAddreses() {
      dataTable.ajax.reload();
    }

    function deleteFuneralAddres(funeralAddres) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _funeralAddresesService
            .delete({
              id: funeralAddres.id,
            })
            .done(function () {
              getFuneralAddreses(true);
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

    $('#CreateNewFuneralAddresButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _funeralAddresesService
        .getFuneralAddresesToExcel({
          filter: $('#FuneralAddresesTableFilter').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
          addressFilter: $('#AddressFilterId').val(),
          funeralDisplayPropertyFilter: $('#FuneralDisplayPropertyFilterId').val(),
          quarterNameFilter: $('#QuarterNameFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditFuneralAddresModalSaved', function () {
      getFuneralAddreses();
    });

    $('#GetFuneralAddresesButton').click(function (e) {
      e.preventDefault();
      getFuneralAddreses();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getFuneralAddreses();
      }
    });

    $('.reload-on-change').change(function (e) {
      getFuneralAddreses();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getFuneralAddreses();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getFuneralAddreses();
    });
  });
})();
