(function () {
  $(function () {
    var _$funeralDocumentsTable = $('#FuneralDocumentsTable');
    var _funeralDocumentsService = abp.services.app.funeralDocuments;

    var $selectedDate = {
      startDate: null,
      endDate: null,
    };

    $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
      $(this).val(picker.startDate.format('MM/DD/YYYY'));
    });

    $('.startDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.startDate = picker.startDate;
        getFuneralDocuments();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getFuneralDocuments();
      });

    $('.endDate')
      .daterangepicker({
        autoUpdateInput: false,
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      })
      .on('apply.daterangepicker', (ev, picker) => {
        $selectedDate.endDate = picker.startDate;
        getFuneralDocuments();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getFuneralDocuments();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.FuneralDocuments.Create'),
      edit: abp.auth.hasPermission('Pages.FuneralDocuments.Edit'),
      delete: abp.auth.hasPermission('Pages.FuneralDocuments.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralDocuments/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/FuneralDocuments/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditFuneralDocumentModal',
    });

    var _viewFuneralDocumentModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralDocuments/ViewfuneralDocumentModal',
      modalClass: 'ViewFuneralDocumentModal',
    });

    var getDateFilter = function (element) {
      if ($selectedDate.startDate == null) {
        return null;
      }
      return $selectedDate.startDate.format('YYYY-MM-DDT00:00:00Z');
    };

    var getMaxDateFilter = function (element) {
      if ($selectedDate.endDate == null) {
        return null;
      }
      return $selectedDate.endDate.format('YYYY-MM-DDT23:59:59Z');
    };

    var dataTable = _$funeralDocumentsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralDocumentsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#FuneralDocumentsTableFilter').val(),
            typeFilter: $('#TypeFilterId').val(),
            pathFilter: $('#PathFilterId').val(),
            guidFilter: $('#GuidFilterId').val(),
            funeralDisplayPropertyFilter: $('#FuneralDisplayPropertyFilterId').val(),
          };
        },
      },
      columnDefs: [
        {
          className: 'control responsive',
          orderable: false,
          render: function () {
            return '';
          },
          targets: 0,
        },
        {
          width: 120,
          targets: 1,
          data: null,
          orderable: false,
          autoWidth: false,
          defaultContent: '',
          rowAction: {
            cssClass: 'btn btn-brand dropdown-toggle',
            text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
            items: [
              {
                text: app.localize('View'),
                action: function (data) {
                  _viewFuneralDocumentModal.open({ id: data.record.funeralDocument.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.funeralDocument.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteFuneralDocument(data.record.funeralDocument);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'funeralDocument.type',
          name: 'type',
          render: function (type) {
            return app.localize('Enum_FuneralDocumentType_' + type);
          },
        },
        {
          targets: 3,
          data: 'funeralDocument.path',
          name: 'path',
          render: function (data) {
              return `<a href="https://ymticenaze.blob.core.windows.net${data}">Göster</a>`;
          },
        },
        {
          targets: 4,
          data: 'funeralDocument.guid',
          name: 'guid',
        },
        {
          targets: 5,
          data: 'funeralDisplayProperty',
          name: 'funeralFk.displayProperty',
        },
      ],
    });

    function getFuneralDocuments() {
      dataTable.ajax.reload();
    }

    function deleteFuneralDocument(funeralDocument) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _funeralDocumentsService
            .delete({
              id: funeralDocument.id,
            })
            .done(function () {
              getFuneralDocuments(true);
              abp.notify.success(app.localize('SuccessfullyDeleted'));
            });
        }
      });
    }

    $('#ShowAdvancedFiltersSpan').click(function () {
      $('#ShowAdvancedFiltersSpan').hide();
      $('#HideAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideDown();
    });

    $('#HideAdvancedFiltersSpan').click(function () {
      $('#HideAdvancedFiltersSpan').hide();
      $('#ShowAdvancedFiltersSpan').show();
      $('#AdvacedAuditFiltersArea').slideUp();
    });

    $('#CreateNewFuneralDocumentButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _funeralDocumentsService
        .getFuneralDocumentsToExcel({
          filter: $('#FuneralDocumentsTableFilter').val(),
          typeFilter: $('#TypeFilterId').val(),
          pathFilter: $('#PathFilterId').val(),
          guidFilter: $('#GuidFilterId').val(),
          funeralDisplayPropertyFilter: $('#FuneralDisplayPropertyFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditFuneralDocumentModalSaved', function () {
      getFuneralDocuments();
    });

    $('#GetFuneralDocumentsButton').click(function (e) {
      e.preventDefault();
      getFuneralDocuments();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getFuneralDocuments();
      }
    });

    $('.reload-on-change').change(function (e) {
      getFuneralDocuments();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getFuneralDocuments();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getFuneralDocuments();
    });
  });
})();
