(function ($) {
  app.modals.AirportLookupTableModal = function () {
    var _modalManager;

    var _funeralFlightsService = abp.services.app.funeralFlights;
    var _$airportTable = $('#AirportTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$airportTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralFlightsService.getAllAirportForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#AirportTableFilter').val(),
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

    $('#AirportTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getAirport() {
      dataTable.ajax.reload();
    }

    $('#GetAirportButton').click(function (e) {
      e.preventDefault();
      getAirport();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#AirportTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getAirport();
      }
    });
  };
})(jQuery);
