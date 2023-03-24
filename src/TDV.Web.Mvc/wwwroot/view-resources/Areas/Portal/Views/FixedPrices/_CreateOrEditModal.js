(function ($) {
  app.modals.CreateOrEditFixedPriceModal = function () {
    var _fixedPricesService = abp.services.app.fixedPrices;

    var _modalManager;
    var _$fixedPriceInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$fixedPriceInformationForm = _modalManager.getModal().find('form[name=FixedPriceInformationsForm]');
      _$fixedPriceInformationForm.validate();
    };

    this.save = function () {
      if (!_$fixedPriceInformationForm.valid()) {
        return;
      }

      var fixedPrice = _$fixedPriceInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _fixedPricesService
        .createOrEdit(fixedPrice)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditFixedPriceModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
