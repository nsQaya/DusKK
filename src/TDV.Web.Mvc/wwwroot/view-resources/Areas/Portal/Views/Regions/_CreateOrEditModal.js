(function ($) {
  app.modals.CreateOrEditRegionModal = function () {
    var _regionsService = abp.services.app.regions;

    var _modalManager;
    var _$regionInformationForm = null;

    var _RegionfixedPriceLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Regions/FixedPriceLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Regions/_RegionFixedPriceLookupTableModal.js',
      modalClass: 'FixedPriceLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$regionInformationForm = _modalManager.getModal().find('form[name=RegionInformationsForm]');
      _$regionInformationForm.validate();
    };

    $('#OpenFixedPriceLookupTableButton').click(function () {
      var region = _$regionInformationForm.serializeFormToObject();

      _RegionfixedPriceLookupTableModal.open(
        { id: region.fixedPriceId, displayName: region.fixedPriceName },
        function (data) {
          _$regionInformationForm.find('input[name=fixedPriceName]').val(data.displayName);
          _$regionInformationForm.find('input[name=fixedPriceId]').val(data.id);
        }
      );
    });

    $('#ClearFixedPriceNameButton').click(function () {
      _$regionInformationForm.find('input[name=fixedPriceName]').val('');
      _$regionInformationForm.find('input[name=fixedPriceId]').val('');
    });

    this.save = function () {
      if (!_$regionInformationForm.valid()) {
        return;
      }
      if ($('#Region_FixedPriceId').prop('required') && $('#Region_FixedPriceId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('FixedPrice')));
        return;
      }

      var region = _$regionInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _regionsService
        .createOrEdit(region)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditRegionModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
