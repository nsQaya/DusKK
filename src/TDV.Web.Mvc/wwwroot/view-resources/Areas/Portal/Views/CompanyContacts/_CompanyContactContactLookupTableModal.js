(function ($) {
  app.modals.ContactLookupTableModal = function () {
    var _modalManager;

    var _companyContactsService = abp.services.app.companyContacts;
    var _$contactTable = $('#ContactTable');

    this.init = function (modalManager) {
      _modalManager = modalManager;
    };

    var dataTable = _$contactTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _companyContactsService.getAllContactForLookupTable,
        inputFilter: function () {
          return {
            filter: $('#ContactTableFilter').val(),
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

    $('#ContactTable tbody').on('click', '[id*=selectbtn]', function () {
      var data = dataTable.row($(this).parents('tr')).data();
      _modalManager.setResult(data);
      _modalManager.close();
    });

    function getContact() {
      dataTable.ajax.reload();
    }

    $('#GetContactButton').click(function (e) {
      e.preventDefault();
      getContact();
    });

    $('#SelectButton').click(function (e) {
      e.preventDefault();
    });

    $('#ContactTableFilter').keypress(function (e) {
      if (e.which === 13 && e.target.tagName.toLocaleLowerCase() != 'textarea') {
        getContact();
      }
    });
  };
})(jQuery);
