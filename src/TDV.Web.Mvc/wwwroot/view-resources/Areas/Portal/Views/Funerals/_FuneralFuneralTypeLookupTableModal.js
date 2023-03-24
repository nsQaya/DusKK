(function ($) {
  app.modals.FuneralTypeLookupTableModal = function () {
    var _modalManager;

    var _funeralsService = abp.services.app.funerals;
    var _$funeralTypeTable = $('#FuneralTypeTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$funeralTypeTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralsService.getAllFuneralTypeForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#FuneralTypeTableFilter').val(),
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

    $('#FuneralTypeTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getFuneralType() {
      dataTable.ajax.reload();
    }

    $('#GetFuneralTypeButton').click(function (e) {
      e.preventDefault();
      getFuneralType();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#FuneralTypeTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getFuneralType();
      }
    });
  };
})(jQuery);
