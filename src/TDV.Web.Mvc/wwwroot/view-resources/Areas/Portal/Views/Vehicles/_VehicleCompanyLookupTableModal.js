(function ($) {
  app.modals.CompanyLookupTableModal = function () {
    var _modalManager;

    var _vehiclesService = abp.services.app.vehicles;
    var _$companyTable = $('#CompanyTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$companyTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _vehiclesService.getAllCompanyForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#CompanyTableFilter').val(),
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

    $('#CompanyTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getCompany() {
      dataTable.ajax.reload();
    }

    $('#GetCompanyButton').click(function (e) {
      e.preventDefault();
      getCompany();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#CompanyTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getCompany();
      }
    });
  };
})(jQuery);
