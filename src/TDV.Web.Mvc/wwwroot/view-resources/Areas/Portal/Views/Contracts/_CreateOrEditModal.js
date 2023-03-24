(function ($) {
  app.modals.CreateOrEditContractModal = function () {
    var _contractsService = abp.services.app.contracts;

    var _modalManager;
    var _$contractInformationForm = null;

    var _ContractregionLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Contracts/RegionLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Contracts/_ContractRegionLookupTableModal.js',
      modalClass: 'RegionLookupTableModal',
    });
    var _ContractcompanyLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Contracts/CompanyLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Contracts/_ContractCompanyLookupTableModal.js',
      modalClass: 'CompanyLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$contractInformationForm = _modalManager.getModal().find('form[name=ContractInformationsForm]');
      _$contractInformationForm.validate();
    };

    $('#OpenRegionLookupTableButton').click(function () {
      var contract = _$contractInformationForm.serializeFormToObject();

      _ContractregionLookupTableModal.open(
        { id: contract.regionId, displayName: contract.regionName },
        function (data) {
          _$contractInformationForm.find('input[name=regionName]').val(data.displayName);
          _$contractInformationForm.find('input[name=regionId]').val(data.id);
        }
      );
    });

    $('#ClearRegionNameButton').click(function () {
      _$contractInformationForm.find('input[name=regionName]').val('');
      _$contractInformationForm.find('input[name=regionId]').val('');
    });

    $('#OpenCompanyLookupTableButton').click(function () {
      var contract = _$contractInformationForm.serializeFormToObject();

      _ContractcompanyLookupTableModal.open(
        { id: contract.companyId, displayName: contract.companyDisplayProperty },
        function (data) {
          _$contractInformationForm.find('input[name=companyDisplayProperty]').val(data.displayName);
          _$contractInformationForm.find('input[name=companyId]').val(data.id);
        }
      );
    });

    $('#ClearCompanyDisplayPropertyButton').click(function () {
      _$contractInformationForm.find('input[name=companyDisplayProperty]').val('');
      _$contractInformationForm.find('input[name=companyId]').val('');
    });

    this.save = function () {
      if (!_$contractInformationForm.valid()) {
        return;
      }
      if ($('#Contract_RegionId').prop('required') && $('#Contract_RegionId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Region')));
        return;
      }
      if ($('#Contract_CompanyId').prop('required') && $('#Contract_CompanyId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Company')));
        return;
      }

      var contract = _$contractInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _contractsService
        .createOrEdit(contract)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditContractModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
