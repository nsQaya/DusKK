(function ($) {
  app.modals.FuneralWorkOrderDetailLookupTableModal = function () {
    var _modalManager;

    var _funeralTranportOrdersService = abp.services.app.funeralTranportOrders;
    var _$funeralWorkOrderDetailTable = $('#FuneralWorkOrderDetailTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$funeralWorkOrderDetailTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralTranportOrdersService.getAllFuneralWorkOrderDetailForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#FuneralWorkOrderDetailTableFilter').val(),
          };
        },
      },
      columnDefs: [
        {
          targets: 0,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent:
            "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" +
            app.localize('Select') +
            "' /></div>",
        },
        {
          autoWidth: false,
          orderable: false,
          targets: 1,
          data: 'displayName',
        },
      ],
    });

    $('#FuneralWorkOrderDetailTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getFuneralWorkOrderDetail() {
      dataTable.ajax.reload();
    }

    $('#GetFuneralWorkOrderDetailButton').click(function (e) {
      e.preventDefault();
      getFuneralWorkOrderDetail();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#FuneralWorkOrderDetailTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getFuneralWorkOrderDetail();
      }
    });
  };
})(jQuery);
