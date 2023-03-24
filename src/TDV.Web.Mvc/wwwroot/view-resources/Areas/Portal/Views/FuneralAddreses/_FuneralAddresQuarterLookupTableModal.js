(function ($) {
  app.modals.QuarterLookupTableModal = function () {
    var _modalManager;

    var _funeralAddresesService = abp.services.app.funeralAddreses;
    var _$quarterTable = $('#QuarterTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$quarterTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralAddresesService.getAllQuarterForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#QuarterTableFilter').val(),
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

    $('#QuarterTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getQuarter() {
      dataTable.ajax.reload();
    }

    $('#GetQuarterButton').click(function (e) {
      e.preventDefault();
      getQuarter();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#QuarterTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getQuarter();
      }
    });
  };
})(jQuery);
