(function () {
  $(function () {
    var _$talepsTable = $('#TalepsTable');
    var _talepsService = abp.services.app.taleps;

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
        getTaleps();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getTaleps();
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
        getTaleps();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getTaleps();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Taleps.Create'),
      edit: abp.auth.hasPermission('Pages.Taleps.Edit'),
      delete: abp.auth.hasPermission('Pages.Taleps.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Taleps/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Taleps/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditTalepModal',
    });

    var _viewTalepModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Taleps/ViewtalepModal',
      modalClass: 'ViewTalepModal',
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

    var dataTable = _$talepsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _talepsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#TalepsTableFilter').val(),
            minTalepMiktarFilter: $('#MinTalepMiktarFilterId').val(),
            maxTalepMiktarFilter: $('#MaxTalepMiktarFilterId').val(),
            olcuBrFilter: $('#OlcuBrFilterId').val(),
            minFiyatFilter: $('#MinFiyatFilterId').val(),
            maxFiyatFilter: $('#MaxFiyatFilterId').val(),
            minTutarFilter: $('#MinTutarFilterId').val(),
            maxTutarFilter: $('#MaxTutarFilterId').val(),
            stokAdiFilter: $('#StokAdiFilterId').val(),
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
                  _viewTalepModal.open({ id: data.record.talep.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.talep.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteTalep(data.record.talep);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'talep.talepMiktar',
          name: 'talepMiktar',
        },
        {
          targets: 3,
          data: 'talep.olcuBr',
          name: 'olcuBr',
        },
        {
          targets: 4,
          data: 'talep.fiyat',
          name: 'fiyat',
        },
        {
          targets: 5,
          data: 'talep.tutar',
          name: 'tutar',
        },
        {
          targets: 6,
          data: 'stokAdi',
          name: 'stokFk.adi',
        },
      ],
    });

    function getTaleps() {
      dataTable.ajax.reload();
    }

    function deleteTalep(talep) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _talepsService
            .delete({
              id: talep.id,
            })
            .done(function () {
              getTaleps(true);
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

    $('#CreateNewTalepButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _talepsService
        .getTalepsToExcel({
          filter: $('#TalepsTableFilter').val(),
          minTalepMiktarFilter: $('#MinTalepMiktarFilterId').val(),
          maxTalepMiktarFilter: $('#MaxTalepMiktarFilterId').val(),
          olcuBrFilter: $('#OlcuBrFilterId').val(),
          minFiyatFilter: $('#MinFiyatFilterId').val(),
          maxFiyatFilter: $('#MaxFiyatFilterId').val(),
          minTutarFilter: $('#MinTutarFilterId').val(),
          maxTutarFilter: $('#MaxTutarFilterId').val(),
          stokAdiFilter: $('#StokAdiFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditTalepModalSaved', function () {
      getTaleps();
    });

    $('#GetTalepsButton').click(function (e) {
      e.preventDefault();
      getTaleps();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getTaleps();
      }
    });

    $('.reload-on-change').change(function (e) {
      getTaleps();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getTaleps();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getTaleps();
    });
  });
})();
