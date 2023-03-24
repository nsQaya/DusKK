(function () {
  $(function () {
    var _$olcumsTable = $('#OlcumsTable');
    var _olcumsService = abp.services.app.olcums;

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
        getOlcums();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getOlcums();
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
        getOlcums();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getOlcums();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Olcums.Create'),
      edit: abp.auth.hasPermission('Pages.Olcums.Edit'),
      delete: abp.auth.hasPermission('Pages.Olcums.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Olcums/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Olcums/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditOlcumModal',
    });

    var _viewOlcumModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Olcums/ViewolcumModal',
      modalClass: 'ViewOlcumModal',
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

    var dataTable = _$olcumsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _olcumsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#OlcumsTableFilter').val(),
            olcuTipiFilter: $('#OlcuTipiFilterId').val(),
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
                  _viewOlcumModal.open({ id: data.record.olcum.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.olcum.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteOlcum(data.record.olcum);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'olcum.olcuTipi',
          name: 'olcuTipi',
        },
      ],
    });

    function getOlcums() {
      dataTable.ajax.reload();
    }

    function deleteOlcum(olcum) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _olcumsService
            .delete({
              id: olcum.id,
            })
            .done(function () {
              getOlcums(true);
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

    $('#CreateNewOlcumButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _olcumsService
        .getOlcumsToExcel({
          filter: $('#OlcumsTableFilter').val(),
          olcuTipiFilter: $('#OlcuTipiFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditOlcumModalSaved', function () {
      getOlcums();
    });

    $('#GetOlcumsButton').click(function (e) {
      e.preventDefault();
      getOlcums();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getOlcums();
      }
    });

    $('.reload-on-change').change(function (e) {
      getOlcums();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getOlcums();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getOlcums();
    });
  });
})();
