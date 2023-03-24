(function () {
  $(function () {
    var _$fixedPriceDetailsTable = $('#MasterDetailChild_FixedPrice_FixedPriceDetailsTable');
    var _fixedPriceDetailsService = abp.services.app.fixedPriceDetails;

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
        getFixedPriceDetails();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getFixedPriceDetails();
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
        getFixedPriceDetails();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getFixedPriceDetails();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.FixedPriceDetails.Create'),
      edit: abp.auth.hasPermission('Pages.FixedPriceDetails.Edit'),
      delete: abp.auth.hasPermission('Pages.FixedPriceDetails.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/MasterDetailChild_FixedPrice_FixedPriceDetails/CreateOrEditModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/MasterDetailChild_FixedPrice_FixedPriceDetails/_CreateOrEditModal.js',
      modalClass: 'MasterDetailChild_FixedPrice_CreateOrEditFixedPriceDetailModal',
    });

    var _viewFixedPriceDetailModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/MasterDetailChild_FixedPrice_FixedPriceDetails/ViewfixedPriceDetailModal',
      modalClass: 'MasterDetailChild_FixedPrice_ViewFixedPriceDetailModal',
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

    var dataTable = _$fixedPriceDetailsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _fixedPriceDetailsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#MasterDetailChild_FixedPrice_FixedPriceDetailsTableFilter').val(),
            typeFilter: $('#MasterDetailChild_FixedPrice_TypeFilterId').val(),
            minStartDateFilter: getDateFilter($('#MasterDetailChild_FixedPrice_MinStartDateFilterId')),
            maxStartDateFilter: getMaxDateFilter($('#MasterDetailChild_FixedPrice_MaxStartDateFilterId')),
            minEndDateFilter: getDateFilter($('#MasterDetailChild_FixedPrice_MinEndDateFilterId')),
            maxEndDateFilter: getMaxDateFilter($('#MasterDetailChild_FixedPrice_MaxEndDateFilterId')),
            currencyTypeFilter: $('#MasterDetailChild_FixedPrice_CurrencyTypeFilterId').val(),
            minPriceFilter: $('#MasterDetailChild_FixedPrice_MinPriceFilterId').val(),
            maxPriceFilter: $('#MasterDetailChild_FixedPrice_MaxPriceFilterId').val(),
            fixedPriceIdFilter: $('#MasterDetailChild_FixedPrice_FixedPriceDetailsId').val(),
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
                  _viewFixedPriceDetailModal.open({ id: data.record.fixedPriceDetail.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.fixedPriceDetail.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteFixedPriceDetail(data.record.fixedPriceDetail);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'fixedPriceDetail.type',
          name: 'type',
          render: function (type) {
            return app.localize('Enum_PaymentMethodType_' + type);
          },
        },
        {
          targets: 3,
          data: 'fixedPriceDetail.startDate',
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
          data: 'fixedPriceDetail.endDate',
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
          data: 'fixedPriceDetail.currencyType',
          name: 'currencyType',
          render: function (currencyType) {
            return app.localize('Enum_CurrencyType_' + currencyType);
          },
        },
        {
          targets: 6,
          data: 'fixedPriceDetail.price',
          name: 'price',
        },
      ],
    });

    function getFixedPriceDetails() {
      dataTable.ajax.reload();
    }

    function deleteFixedPriceDetail(fixedPriceDetail) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _fixedPriceDetailsService
            .delete({
              id: fixedPriceDetail.id,
            })
            .done(function () {
              getFixedPriceDetails(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#MasterDetailChild_FixedPrice_ShowAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_FixedPrice_ShowAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_FixedPrice_HideAdvancedFiltersSpan').show();
      $('#MasterDetailChild_FixedPrice_AdvacedAuditFiltersArea').slideDown();
    });

    $('#MasterDetailChild_FixedPrice_HideAdvancedFiltersSpan').click(function () {
      $('#MasterDetailChild_FixedPrice_HideAdvancedFiltersSpan').hide();
      $('#MasterDetailChild_FixedPrice_ShowAdvancedFiltersSpan').show();
      $('#MasterDetailChild_FixedPrice_AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewFixedPriceDetailButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _fixedPriceDetailsService
        .getFixedPriceDetailsToExcel({
          filter: $('#FixedPriceDetailsTableFilter').val(),
          typeFilter: $('#MasterDetailChild_FixedPrice_TypeFilterId').val(),
          minStartDateFilter: getDateFilter($('#MasterDetailChild_FixedPrice_MinStartDateFilterId')),
          maxStartDateFilter: getMaxDateFilter($('#MasterDetailChild_FixedPrice_MaxStartDateFilterId')),
          minEndDateFilter: getDateFilter($('#MasterDetailChild_FixedPrice_MinEndDateFilterId')),
          maxEndDateFilter: getMaxDateFilter($('#MasterDetailChild_FixedPrice_MaxEndDateFilterId')),
          currencyTypeFilter: $('#MasterDetailChild_FixedPrice_CurrencyTypeFilterId').val(),
          minPriceFilter: $('#MasterDetailChild_FixedPrice_MinPriceFilterId').val(),
          maxPriceFilter: $('#MasterDetailChild_FixedPrice_MaxPriceFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditFixedPriceDetailModalSaved', function () {
      getFixedPriceDetails();
    });

    $('#GetFixedPriceDetailsButton').click(function (e) {
      e.preventDefault();
      getFixedPriceDetails();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getFixedPriceDetails();
      }
    });

    $('.reload-on-change').change(function (e) {
      getFixedPriceDetails();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getFixedPriceDetails();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getFixedPriceDetails();
    });
  });
})();
