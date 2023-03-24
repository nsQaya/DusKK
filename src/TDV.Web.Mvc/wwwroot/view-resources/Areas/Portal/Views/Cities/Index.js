(function () {
  $(function () {
    var _$citiesTable = $('#CitiesTable');
    var _citiesService = abp.services.app.cities;

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
        getCities();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getCities();
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
        getCities();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getCities();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Cities.Create'),
      edit: abp.auth.hasPermission('Pages.Cities.Edit'),
      delete: abp.auth.hasPermission('Pages.Cities.Delete'),
    };

    var _createOrEditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Cities/CreateOrEditModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Cities/_CreateOrEditModal.js',
      modalClass: 'CreateOrEditCityModal',
    });

    var _viewCityModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Cities/ViewcityModal',
      modalClass: 'ViewCityModal',
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

    var dataTable = _$citiesTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _citiesService.getAll,
        inputFilter: function () {
          return {
            filter: $('#CitiesTableFilter').val(),
            codeFilter: $('#CodeFilterId').val(),
            nameFilter: $('#NameFilterId').val(),
            minOrderFilter: $('#MinOrderFilterId').val(),
            maxOrderFilter: $('#MaxOrderFilterId').val(),
            isActiveFilter: $('#IsActiveFilterId').val(),
            countryDisplayPropertyFilter: $('#CountryDisplayPropertyFilterId').val(),
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
                  _viewCityModal.open({ id: data.record.city.id });
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  _createOrEditModal.open({ id: data.record.city.id });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteCity(data.record.city);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'city.code',
          name: 'code',
        },
        {
          targets: 3,
          data: 'city.name',
          name: 'name',
        },
        {
          targets: 4,
          data: 'city.order',
          name: 'order',
        },
        {
          targets: 5,
          data: 'city.isActive',
          name: 'isActive',
          render: function (isActive) {
            if (isActive) {
              return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
            }
            return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
          },
        },
        {
          targets: 6,
          data: 'countryDisplayProperty',
          name: 'countryFk.displayProperty',
        },
      ],
    });

    function getCities() {
      dataTable.ajax.reload();
    }

    function deleteCity(city) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _citiesService
            .delete({
              id: city.id,
            })
            .done(function () {
              getCities(true);
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

    $('#CreateNewCityButton').click(function () {
      _createOrEditModal.open();
    });

    $('#ExportToExcelButton').click(function () {
      _citiesService
        .getCitiesToExcel({
          filter: $('#CitiesTableFilter').val(),
          codeFilter: $('#CodeFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
          minOrderFilter: $('#MinOrderFilterId').val(),
          maxOrderFilter: $('#MaxOrderFilterId').val(),
          isActiveFilter: $('#IsActiveFilterId').val(),
          countryDisplayPropertyFilter: $('#CountryDisplayPropertyFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditCityModalSaved', function () {
      getCities();
    });

    $('#GetCitiesButton').click(function (e) {
      e.preventDefault();
      getCities();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getCities();
      }
    });

    $('.reload-on-change').change(function (e) {
      getCities();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getCities();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getCities();
    });
  });
})();
