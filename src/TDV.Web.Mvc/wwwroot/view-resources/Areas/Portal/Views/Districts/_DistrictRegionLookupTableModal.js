(function ($) {
  app.modals.RegionLookupTableModal = function () {
    var _modalManager;

    var _districtsService = abp.services.app.districts;
    var _$regionTable = $('#RegionTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$regionTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _districtsService.getAllRegionForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#RegionTableFilter').val(),
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

    $('#RegionTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getRegion() {
      dataTable.ajax.reload();
    }

    $('#GetRegionButton').click(function (e) {
      e.preventDefault();
      getRegion();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#RegionTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getRegion();
      }
    });
  };
})(jQuery);
