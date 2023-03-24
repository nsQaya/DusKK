(function () {
  $(function () {
    var _$funeralTranportOrdersTable = $('#FuneralTranportOrdersTable');
    var _funeralTranportOrdersService = abp.services.app.funeralTranportOrders;

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
        getFuneralTranportOrders();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getFuneralTranportOrders();
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
        getFuneralTranportOrders();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getFuneralTranportOrders();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.FuneralTranportOrders.Create'),
      edit: abp.auth.hasPermission('Pages.FuneralTranportOrders.Edit'),
      delete: abp.auth.hasPermission('Pages.FuneralTranportOrders.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralTranportOrders/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/FuneralTranportOrders/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditFuneralTranportOrderModal',
    });

    var _viewFuneralTranportOrderModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralTranportOrders/ViewfuneralTranportOrderModal',
      modalClass: 'ViewFuneralTranportOrderModal',
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

    var dataTable = _$funeralTranportOrdersTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralTranportOrdersService.getAll,
        inputFilter: function () {
          return {
            filter: $('#FuneralTranportOrdersTableFilter').val(),
            minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
            maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
            minStartKMFilter: $('#MinStartKMFilterId').val(),
            maxStartKMFilter: $('#MaxStartKMFilterId').val(),
            minOperationDateFilter: getDateFilter($('#MinOperationDateFilterId')),
            maxOperationDateFilter: getMaxDateFilter($('#MaxOperationDateFilterId')),
            minOperationKMFilter: $('#MinOperationKMFilterId').val(),
            maxOperationKMFilter: $('#MaxOperationKMFilterId').val(),
            minDeliveryDateFilter: getDateFilter($('#MinDeliveryDateFilterId')),
            maxDeliveryDateFilter: getMaxDateFilter($('#MaxDeliveryDateFilterId')),
            minDeliveryKMFilter: $('#MinDeliveryKMFilterId').val(),
            maxDeliveryKMFilter: $('#MaxDeliveryKMFilterId').val(),
            minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
            maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
            minEndKMFilter: $('#MinEndKMFilterId').val(),
            maxEndKMFilter: $('#MaxEndKMFilterId').val(),
            receiverFullNameFilter: $('#ReceiverFullNameFilterId').val(),
            receiverKinshipDegreeFilter: $('#ReceiverKinshipDegreeFilterId').val(),
            funeralWorkOrderDetailDescriptionFilter: $('#FuneralWorkOrderDetailDescriptionFilterId').val(),
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
                  _viewFuneralTranportOrderModal.open({ id: data.record.funeralTranportOrder.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.funeralTranportOrder.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteFuneralTranportOrder(data.record.funeralTranportOrder);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'funeralTranportOrder.startDate',
          name: 'startDate',
          render: function (startDate) {
            if (startDate) {
              return moment(startDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 3,
          data: 'funeralTranportOrder.startKM',
          name: 'startKM',
        },
        {
          targets: 4,
          data: 'funeralTranportOrder.operationDate',
          name: 'operationDate',
          render: function (operationDate) {
            if (operationDate) {
              return moment(operationDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 5,
          data: 'funeralTranportOrder.operationKM',
          name: 'operationKM',
        },
        {
          targets: 6,
          data: 'funeralTranportOrder.deliveryDate',
          name: 'deliveryDate',
          render: function (deliveryDate) {
            if (deliveryDate) {
              return moment(deliveryDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 7,
          data: 'funeralTranportOrder.deliveryKM',
          name: 'deliveryKM',
        },
        {
          targets: 8,
          data: 'funeralTranportOrder.endDate',
          name: 'endDate',
          render: function (endDate) {
            if (endDate) {
              return moment(endDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 9,
          data: 'funeralTranportOrder.endKM',
          name: 'endKM',
        },
        {
          targets: 10,
          data: 'funeralTranportOrder.receiverFullName',
          name: 'receiverFullName',
        },
        {
          targets: 11,
          data: 'funeralTranportOrder.receiverKinshipDegree',
          name: 'receiverKinshipDegree',
        },
        {
          targets: 12,
          data: 'funeralWorkOrderDetailDescription',
          name: 'funeralWorkOrderDetailFk.description',
        },
      ],
    });

    function getFuneralTranportOrders() {
      dataTable.ajax.reload();
    }

    function deleteFuneralTranportOrder(funeralTranportOrder) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _funeralTranportOrdersService
            .delete({
              id: funeralTranportOrder.id,
            })
            .done(function () {
              getFuneralTranportOrders(true);
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

    $('#CreateNewFuneralTranportOrderButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _funeralTranportOrdersService
        .getFuneralTranportOrdersToExcel({
          filter: $('#FuneralTranportOrdersTableFilter').val(),
          minStartDateFilter: getDateFilter($('#MinStartDateFilterId')),
          maxStartDateFilter: getMaxDateFilter($('#MaxStartDateFilterId')),
          minStartKMFilter: $('#MinStartKMFilterId').val(),
          maxStartKMFilter: $('#MaxStartKMFilterId').val(),
          minOperationDateFilter: getDateFilter($('#MinOperationDateFilterId')),
          maxOperationDateFilter: getMaxDateFilter($('#MaxOperationDateFilterId')),
          minOperationKMFilter: $('#MinOperationKMFilterId').val(),
          maxOperationKMFilter: $('#MaxOperationKMFilterId').val(),
          minDeliveryDateFilter: getDateFilter($('#MinDeliveryDateFilterId')),
          maxDeliveryDateFilter: getMaxDateFilter($('#MaxDeliveryDateFilterId')),
          minDeliveryKMFilter: $('#MinDeliveryKMFilterId').val(),
          maxDeliveryKMFilter: $('#MaxDeliveryKMFilterId').val(),
          minEndDateFilter: getDateFilter($('#MinEndDateFilterId')),
          maxEndDateFilter: getMaxDateFilter($('#MaxEndDateFilterId')),
          minEndKMFilter: $('#MinEndKMFilterId').val(),
          maxEndKMFilter: $('#MaxEndKMFilterId').val(),
          receiverFullNameFilter: $('#ReceiverFullNameFilterId').val(),
          receiverKinshipDegreeFilter: $('#ReceiverKinshipDegreeFilterId').val(),
          funeralWorkOrderDetailDescriptionFilter: $('#FuneralWorkOrderDetailDescriptionFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditFuneralTranportOrderModalSaved', function () {
      getFuneralTranportOrders();
    });

    $('#GetFuneralTranportOrdersButton').click(function (e) {
      e.preventDefault();
      getFuneralTranportOrders();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getFuneralTranportOrders();
      }
    });

    $('.reload-on-change').change(function (e) {
      getFuneralTranportOrders();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getFuneralTranportOrders();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getFuneralTranportOrders();
    });
  });
})();
