(function () {
  $(function () {
    var _$funeralsTable = $('#FuneralAssigmentTable');
    var _funeralsService = abp.services.app.funerals;
    var _entityTypeFullName = 'TDV.Burial.Funeral';

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
        getFunerals();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.startDate = null;
        getFunerals();
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
        getFunerals();
      })
      .on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $selectedDate.endDate = null;
        getFunerals();
      });

    var _permissions = {
      create: abp.auth.hasPermission('Pages.Funerals.Create'),
      edit: abp.auth.hasPermission('Pages.Funerals.Edit'),
      delete: abp.auth.hasPermission('Pages.Funerals.Delete'),
    };

      var _viewdriverAssigmentModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Funerals/ViewdriverAssigmentModal',
        modalClass: 'ViewDriverAssigmentModal',
    });

    var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
    function entityHistoryIsEnabled() {
      return (
        abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
        abp.custom.EntityHistory &&
        abp.custom.EntityHistory.IsEnabled &&
        _.filter(abp.custom.EntityHistory.EnabledEntities, (entityType) => entityType === _entityTypeFullName)
          .length === 1
      );
    }

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

    var dataTable = _$funeralsTable.DataTable({
      paging: true,
      serverSide: true,
      processing: true,
      listAction: {
        ajaxFunction: _funeralsService.getAll,
        inputFilter: function () {
          return {
            filter: $('#FuneralsTableFilter').val(),
            transferNoFilter: $('#TransferNoFilterId').val(),
            memberNoFilter: $('#MemberNoFilterId').val(),
            nameFilter: $('#NameFilterId').val(),
            surnameFilter: $('#SurnameFilterId').val(),
            minTcNoFilter: $('#MinTcNoFilterId').val(),
            maxTcNoFilter: $('#MaxTcNoFilterId').val(),
            passportNoFilter: $('#PassportNoFilterId').val(),
            ladingNoFilter: $('#LadingNoFilterId').val(),
            statusFilter: $('#StatusFilterId').val(),
            minOperationDateFilter: getDateFilter($('#MinOperationDateFilterId')),
            maxOperationDateFilter: getMaxDateFilter($('#MaxOperationDateFilterId')),
            funeralTypeDescriptionFilter: $('#FuneralTypeDescriptionFilterId').val(),
            contactDisplayPropertyFilter: $('#ContactDisplayPropertyFilterId').val(),
              OwnerOrganizationUnitDisplayNameFilter: $('#OwnerOrganizationUnitDisplayNameFilterId').val(),
            GiverOrganizationUnitDisplayNameFilter: $('#GiverOrganizationUnitDisplayNameFilterId').val(),
            ContractorOrganizationUnitDisplayNameFilter: $('#ContractorOrganizationUnitDisplayNameFilterId').val(),
            userNameFilter: $('#UserNameFilterId').val(),
            funeralPackageCodeFilter: $('#FuneralPackageCodeFilterId').val(),
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
                  window.location = '/Portal/Funerals/ViewFuneral/' + data.record.funeral.id;
                },
              },
              {
                text: app.localize('Edit'),
                visible: function () {
                  return _permissions.edit;
                },
                action: function (data) {
                  window.location = '/Portal/Funerals/CreateOrEdit/' + data.record.funeral.id;
                },
              },
              {
                text: app.localize('History'),
                iconStyle: 'fas fa-history mr-2',
                visible: function () {
                  return entityHistoryIsEnabled();
                },
                action: function (data) {
                  _entityTypeHistoryModal.open({
                    entityTypeFullName: _entityTypeFullName,
                    entityId: data.record.funeral.id,
                  });
                },
              },
              {
                text: app.localize('Delete'),
                visible: function () {
                  return _permissions.delete;
                },
                action: function (data) {
                  deleteFuneral(data.record.funeral);
                },
              },
            ],
          },
        },
        {
          targets: 2,
          data: 'funeral.transferNo',
          name: 'transferNo',
        },
        {
          targets: 3,
          data: 'funeral.memberNo',
          name: 'memberNo',
        },
        {
          targets: 4,
          data: 'funeral.name',
          name: 'name',
        },
        {
          targets: 5,
          data: 'funeral.surname',
          name: 'surname',
        },
        {
          targets: 6,
          data: 'funeral.tcNo',
          name: 'tcNo',
        },
        {
          targets: 7,
          data: 'funeral.passportNo',
          name: 'passportNo',
        },
        {
          targets: 8,
          data: 'funeral.ladingNo',
          name: 'ladingNo',
        },
        {
          targets: 9,
          data: 'funeral.status',
          name: 'status',
          render: function (status) {
            return app.localize('Enum_FuneralStatus_' + status);
          },
        },
        {
          targets: 10,
          data: 'funeral.operationDate',
          name: 'operationDate',
          render: function (operationDate) {
            if (operationDate) {
              return moment(operationDate).format('L');
            }
            return '';
          },
        },
        {
          targets: 11,
          data: 'funeralTypeDescription',
          name: 'typeFk.description',
        },
        {
          targets: 12,
          data: 'contactDisplayProperty',
          name: 'contactFk.displayProperty',
        },
        {
          targets: 13,
          data: 'ownerOrganizationUnitDisplayName',
          name: 'ownerOrgUnitFk.displayName',
        },
        {
          targets: 14,
          data: 'giverOrganizationUnitDisplayName',
          name: 'giverOrgUnitFk.displayName',
        },
        {
          targets: 15,
          data: 'contractorOrganizationUnitDisplayName',
          name: 'contractorOrgUnitFk.displayName',
        },
        {
          targets: 16,
          data: 'userName',
          name: 'employeePersonFk.name',
        },
        {
          targets: 17,
          data: 'funeralPackageCode',
          name: 'funeralPackageFk.code',
        },
      ],
    });

    function getFunerals() {
      dataTable.ajax.reload();
    }

    function deleteFuneral(funeral) {
      abp.message.confirm('', app.localize('AreYouSure'), function (isConfirmed) {
        if (isConfirmed) {
          _funeralsService
            .delete({
              id: funeral.id,
            })
            .done(function () {
              getFunerals(true);
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

    $('#ExportToExcelButton').click(function () {
      _funeralsService
        .getFuneralsToExcel({
          filter: $('#FuneralsTableFilter').val(),
          transferNoFilter: $('#TransferNoFilterId').val(),
          memberNoFilter: $('#MemberNoFilterId').val(),
          nameFilter: $('#NameFilterId').val(),
          surnameFilter: $('#SurnameFilterId').val(),
          minTcNoFilter: $('#MinTcNoFilterId').val(),
          maxTcNoFilter: $('#MaxTcNoFilterId').val(),
          passportNoFilter: $('#PassportNoFilterId').val(),
          ladingNoFilter: $('#LadingNoFilterId').val(),
          statusFilter: $('#StatusFilterId').val(),
          minOperationDateFilter: getDateFilter($('#MinOperationDateFilterId')),
          maxOperationDateFilter: getMaxDateFilter($('#MaxOperationDateFilterId')),
          funeralTypeDescriptionFilter: $('#FuneralTypeDescriptionFilterId').val(),
          contactDisplayPropertyFilter: $('#ContactDisplayPropertyFilterId').val(),
            OwnerOrganizationUnitDisplayNameFilter: $('#OwnerOrganizationUnitDisplayNameFilterId').val(),
          GiverOrganizationUnitDisplayNameFilter: $('#GiverOrganizationUnitDisplayNameFilterId').val(),
          ContractorOrganizationUnitDisplayNameFilter: $('#ContractorOrganizationUnitDisplayNameFilterId').val(),
          userNameFilter: $('#UserNameFilterId').val(),
          funeralPackageCodeFilter: $('#FuneralPackageCodeFilterId').val(),
        })
        .done(function (result) {
          app.downloadTempFile(result);
        });
    });

    abp.event.on('app.createOrEditFuneralModalSaved', function () {
      getFunerals();
    });

    $('#GetFuneralsButton').click(function (e) {
      e.preventDefault();
      getFunerals();
    });

    $(document).keypress(function (e) {
      if (e.which === 13) {
        getFunerals();
      }
    });

    $('.reload-on-change').change(function (e) {
      getFunerals();
    });

    $('.reload-on-keyup').keyup(function (e) {
      getFunerals();
    });

    $('#btn-reset-filters').click(function (e) {
      $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
      getFunerals();
    });
  });
})();
