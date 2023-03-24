(function () {
    $(function () {
        var _funeralDocumentsService = abp.services.app.funeralDocuments;
        var _$funeralDocumentsTable = $('#FuneralDocumentsTable');

       _$funeralDocumentsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
              ajaxFunction: _funeralDocumentsService.getAll,
              inputFilter: function () {
                return {
                  funeralID: $('#funeralID').val(),
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
                data: 'funeralDocument',
                render: function (funeralDocument) {
                  if (!funeralDocument.path) {
                    return '';
                  }
                  return `<a href="/Portal/FuneralDocuments/DownloadPathFile?id=${funeralDocument.path}" target="_blank">${app.localize('Show')}</a>`;
                },
              }
            ],
          });

          
    });
})();