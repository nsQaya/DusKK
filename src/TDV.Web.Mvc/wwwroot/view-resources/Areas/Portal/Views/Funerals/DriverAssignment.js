(function () {
  $(function () {


    var _$funeralsTable = $('#FuneralsTable');
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

    var _assignModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Funerals/DriverAssignmentModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Funerals/_DriverAssignmentModal.js',
      modalClass: 'DriverAssignmentModal',
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
        ajaxFunction: _funeralsService.getAllGroupByPackage,
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
            ownerOrganizationUnitDisplayNameFilter: $('#OwnerOrganizationUnitDisplayNameFilterId').val(),
            giverOrganizationUnitDisplayNameFilter: $('#GiverOrganizationUnitDisplayNameFilterId').val(),
            contractorOrganizationUnitDisplayNameFilter: $('#ContractorOrganizationUnitDisplayNameFilterId').val(),
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
                /**
                 * ATAMA KISMI DURUM ONAYLADNI İSE GÖSTERİLECEK 
                 * STATUS KONTROLLERİ YAPILACAK EĞER STATUSU TAŞINMAYA BAŞLANMADI İSE ŞÖFÖR DEĞİŞİMİ VE ARABA DEĞİŞİMİ YAPILABİECEK
                 */
                text: app.localize('AssignToDriver'), 
                action: function(data){
                  _assignModal.open({ packageId: data.record.package.id });
                }
              },
              {
                text: app.localize('Reject'),
                visible: function(data){
                  var status= data.record.package.status;
                  return status==1;
                }
              },
              {
                text: app.localize('Accept'), // REJECT VE ACCEPTLER CENAZENİN DURUMUNA GÖRE GÖSTERİMİ DEĞİŞECEK
                visible: function(data){
                  var status= data.record.package.status;
                  return status==1;
                }
              }
            ],
          },
        },
        {
          className: 'details-control',
          targets: 2,
          orderable: false,
          autoWidth: false,
          visible: true,
          render: function () {
            return `<button class="btn btn-primary btn-xs">${app.localize(
              'ShowPackageInside'
            )}</button>`;
          },
        },
        {
          targets: 3,
          data: 'package.status',
          name: 'status',
        },
        {
          targets: 4,
          data: 'package.code',
          name: 'packageCode',
        }
      ],
    });

    const formatFuneral= (funeral)=>{
      return `
        <tr role="row">
          <td>
            <button class="btn btn-info showFuneral" data-id="${funeral.id}">${app.localize("Show")}</button>
              </br>
            <button class="btn btn-danger mt-1 removeFuneral" data-id="${funeral.id}">${app.localize("RemoveFromPackage")}</button>
          </td>
          <td>${funeral.transferNo}</td>
          <td>${funeral.name + ' '+ funeral.surname}</td>
          <td>${funeral.tcNo + ' '+ funeral.passportNo}</td>
        </tr>
      `
    }

    const formatFunerals= (datas)=>{
      console.log(datas);
      return `
      <table class="table align-middle table-row-dashed fs-6 gy-5 dataTable no-footer">
        <thead>
          <th>İşlemler</th>
          <th>Transfer No</th>
          <th>İsim Soyisim</th>
          <th>TcNo / PasaportNo</th>
        </thead>
        <tbody>
         ${datas.funerals.map(x=>formatFuneral(x))}
        </tbody>
      </table>`;
    }

    $('#FuneralsTable tbody').on('click', '.showFuneral', function () {
      var id= $(this).data("id");
      window.open(`/Portal/Funerals/ViewFuneral/${id}`);
    });
    $('#FuneralsTable tbody').on('click', '.removeFuneral', async function () {
      var id= $(this).data("id");
      await _funeralsService.clearPackageFromFuneral(id);
      abp.notify.info(app.localize('RemovedSuccessfully'));

      getFunerals();
    });

    $('#FuneralsTable tbody').on('click', 'td.details-control', function () {
      var tr = $(this).closest('tr');
      var button= $(tr).find(".details-control > button");
      var row = dataTable.row(tr);
      if (row.child.isShown()) {
        button.text(app.localize("ShowPackageInside"));
        row.child.hide();
        tr.removeClass('shown');
    } else {
        button.text(app.localize("HidePackageInside"));
        row.child(formatFunerals(row.data())).show();
        tr.addClass('shown');
    }
    })

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
