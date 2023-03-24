﻿(function ($) {
  app.modals.CountryLookupTableModal = function () {
    var _modalManager;

    var _citiesService = abp.services.app.cities;
    var _$countryTable = $('#CountryTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$countryTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _citiesService.getAllCountryForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#CountryTableFilter').val(),
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

    $('#CountryTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getCountry() {
      dataTable.ajax.reload();
    }

    $('#GetCountryButton').click(function (e) {
      e.preventDefault();
      getCountry();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#CountryTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getCountry();
      }
    });
  };
})(jQuery);
