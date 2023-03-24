(function ($) {
  app.modals.CityLookupTableModal = function () {
    var _modalManager;

    var _airportsService = abp.services.app.airports;
    var _$cityTable = $('#CityTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$cityTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _airportsService.getAllCityForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#CityTableFilter').val(),
            countryId: _modalManager.getArgs().countryId || 0
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

    $('#CityTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getCity() {
      dataTable.ajax.reload();
    }

    $('#GetCityButton').click(function (e) {
      e.preventDefault();
      getCity();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#CityTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getCity();
      }
    });
  };
})(jQuery);
