(function ($) {
  app.modals.MasterDetailChild_FixedPrice_FixedPriceLookupTableModal = function () {
    var _modalManager;

    var _fixedPriceDetailsService = abp.services.app.fixedPriceDetails;
    var _$fixedPriceTable = $('#FixedPriceTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$fixedPriceTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _fixedPriceDetailsService.getAllFixedPriceForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#FixedPriceTableFilter').val(),
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

    $('#FixedPriceTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getFixedPrice() {
      dataTable.ajax.reload();
    }

    $('#GetFixedPriceButton').click(function (e) {
      e.preventDefault();
      getFixedPrice();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#FixedPriceTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getFixedPrice();
      }
    });
  };
})(jQuery);
