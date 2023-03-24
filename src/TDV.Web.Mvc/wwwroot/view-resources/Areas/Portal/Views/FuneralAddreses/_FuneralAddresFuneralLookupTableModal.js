(function ($) {
  app.modals.FuneralLookupTableModal = function () {
    var _modalManager;

    var _funeralAddresesService = abp.services.app.funeralAddreses;
    var _$funeralTable = $('#FuneralTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$funeralTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralAddresesService.getAllFuneralForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#FuneralTableFilter').val(),
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

    $('#FuneralTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getFuneral() {
      dataTable.ajax.reload();
    }

    $('#GetFuneralButton').click(function (e) {
      e.preventDefault();
      getFuneral();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#FuneralTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getFuneral();
      }
    });
  };
})(jQuery);
