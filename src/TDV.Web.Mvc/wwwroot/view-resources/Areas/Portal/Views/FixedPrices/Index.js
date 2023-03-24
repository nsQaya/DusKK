(function () {
  $(function () {
    var _$fixedPricesTable = $('#FixedPricesTable');
    var _fixedPricesService = abp.services.app.fixedPrices;

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
        getFixedPrices();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getFixedPrices();
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
        getFixedPrices();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getFixedPrices();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.FixedPrices.Create'),
      edit: abp.auth.hasPermission('Pages.FixedPrices.Edit'),
      delete: abp.auth.hasPermission('Pages.FixedPrices.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FixedPrices/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/FixedPrices/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditFixedPriceModal',
    });

    var _viewFixedPriceModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FixedPrices/ViewfixedPriceModal',
      modalClass: 'ViewFixedPriceModal',
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

    var dataTable = _$fixedPricesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _fixedPricesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#FixedPricesTableFilter').val(),
            nameFilter: $('#NameFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
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
                  _viewFixedPriceModal.open({ id: data.record.fixedPrice.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.fixedPrice.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteFixedPrice(data.record.fixedPrice);
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
          visible: abp.auth.hasPermission('Pages.FixedPriceDetails'),
          render: function () {
            return `<button class="btn btn-primary btn-xs Edit_FixedPriceDetail_FixedPriceId">${app.localize(
              'EditFixedPriceDetail'
            )}</button>`;
          },
        },
        {
          targets: 3,
          data: 'fixedPrice.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'fixedPrice.description',
          name: 'description',
        },
      ],
    });

    function getFixedPrices() {
      dataTable.ajax.reload();
    }

    function deleteFixedPrice(fixedPrice) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _fixedPricesService
            .delete({
              id: fixedPrice.id,
            })
            .done(function () {
              getFixedPrices(true);
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

    $('#CreateNewFixedPriceButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _fixedPricesService
        .getFixedPricesToExcel({
          filter: $('#FixedPricesTableFilter').val(),
          nameFilter: $('#NameFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditFixedPriceModalSaved', function () {
      getFixedPrices();
    });

    $('#GetFixedPricesButton').click(function (e) {
      e.preventDefault();
      getFixedPrices();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getFixedPrices();
      }
    });

    $('.reload-on-change').change(function (e) {
      getFixedPrices();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getFixedPrices();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getFixedPrices();
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

    _$fixedPricesTable.on('click', '.Edit_FixedPriceDetail_FixedPriceId', function () {
      var tr = $(this).closest('tr');
      var row = dataTable.row(tr);
      openDetailRow(
        this,
        '/Portal/MasterDetailChild_FixedPrice_FixedPriceDetails?FixedPriceId=' + row.data().fixedPrice.id
      );
    });
  });
})();
