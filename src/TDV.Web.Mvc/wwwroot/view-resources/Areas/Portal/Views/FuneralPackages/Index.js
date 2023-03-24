(function () {
  $(function () {
    var _$funeralPackagesTable = $('#FuneralPackagesTable');
    var _funeralPackagesService = abp.services.app.funeralPackages;

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
        getFuneralPackages();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getFuneralPackages();
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
        getFuneralPackages();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getFuneralPackages();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.FuneralPackages.Create'),
      edit: abp.auth.hasPermission('Pages.FuneralPackages.Edit'),
      delete: abp.auth.hasPermission('Pages.FuneralPackages.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralPackages/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/FuneralPackages/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditFuneralPackageModal',
    });

    var _viewFuneralPackageModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralPackages/ViewfuneralPackageModal',
      modalClass: 'ViewFuneralPackageModal',
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

    var dataTable = _$funeralPackagesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralPackagesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#FuneralPackagesTableFilter').val(),
            statusFilter: $('#StatusFilterId').val(),
            codeFilter: $('#CodeFilterId').val(),
            descriptionFilter: $('#DescriptionFilterId').val(),
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
                  _viewFuneralPackageModal.open({ id: data.record.funeralPackage.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.funeralPackage.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteFuneralPackage(data.record.funeralPackage);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'funeralPackage.status',
          name: 'status',
          render: function (status) {
            return app.localize('Enum_FuneralStatus_' + status);
          },
        },
        {
          targets: 3,
          data: 'funeralPackage.code',
          name: 'code',
        },
        {
          targets: 4,
          data: 'funeralPackage.description',
          name: 'description',
        },
      ],
    });

    function getFuneralPackages() {
      dataTable.ajax.reload();
    }

    function deleteFuneralPackage(funeralPackage) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _funeralPackagesService
            .delete({
              id: funeralPackage.id,
            })
            .done(function () {
              getFuneralPackages(true);
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

    $('#CreateNewFuneralPackageButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _funeralPackagesService
        .getFuneralPackagesToExcel({
          filter: $('#FuneralPackagesTableFilter').val(),
          statusFilter: $('#StatusFilterId').val(),
          codeFilter: $('#CodeFilterId').val(),
          descriptionFilter: $('#DescriptionFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditFuneralPackageModalSaved', function () {
      getFuneralPackages();
    });

    $('#GetFuneralPackagesButton').click(function (e) {
      e.preventDefault();
      getFuneralPackages();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getFuneralPackages();
      }
    });

    $('.reload-on-change').change(function (e) {
      getFuneralPackages();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getFuneralPackages();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getFuneralPackages();
    });
  });
})();
