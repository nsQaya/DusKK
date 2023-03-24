(function ($) {
  app.modals.StokLookupTableModal = function () {
    var _modalManager;

    var _stokOlcusService = abp.services.app.stokOlcus;
    var _$stokTable = $('#StokTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$stokTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _stokOlcusService.getAllStokForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#StokTableFilter').val(),
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

    $('#StokTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getStok() {
      dataTable.ajax.reload();
    }

    $('#GetStokButton').click(function (e) {
      e.preventDefault();
      getStok();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#StokTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getStok();
      }
    });
  };
})(jQuery);
