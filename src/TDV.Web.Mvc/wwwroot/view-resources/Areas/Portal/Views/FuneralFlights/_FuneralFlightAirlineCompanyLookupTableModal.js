(function ($) {
  app.modals.AirlineCompanyLookupTableModal = function () {
    var _modalManager;

    var _funeralFlightsService = abp.services.app.funeralFlights;
    var _$airlineCompanyTable = $('#AirlineCompanyTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$airlineCompanyTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralFlightsService.getAllAirlineCompanyForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#AirlineCompanyTableFilter').val(),
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

    $('#AirlineCompanyTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getAirlineCompany() {
      dataTable.ajax.reload();
    }

    $('#GetAirlineCompanyButton').click(function (e) {
      e.preventDefault();
      getAirlineCompany();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#AirlineCompanyTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getAirlineCompany();
      }
    });
  };
})(jQuery);
