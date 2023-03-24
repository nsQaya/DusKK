(function ($) {
  app.modals.DistrictLookupTableModal = function () {
    var _modalManager;

    var _quartersService = abp.services.app.quarters;
    var _$districtTable = $('#DistrictTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$districtTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _quartersService.getAllDistrictForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#DistrictTableFilter').val(),
            cityId: _modalManager.getArgs().cityId || 0
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

    $('#DistrictTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getDistrict() {
      dataTable.ajax.reload();
    }

    $('#GetDistrictButton').click(function (e) {
      e.preventDefault();
      getDistrict();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#DistrictTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getDistrict();
      }
    });
  };
})(jQuery);
