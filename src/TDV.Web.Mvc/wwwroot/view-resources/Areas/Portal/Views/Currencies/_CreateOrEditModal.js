(function ($) {
  app.modals.CreateOrEditCurrencyModal = function () {
    var _currenciesService = abp.services.app.currencies;

    var _modalManager;
    var _$currencyInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$currencyInformationForm = _modalManager.getModal().find('form[name=CurrencyInformationsForm]');
      _$currencyInformationForm.validate();
    };

    this.save = function () {
      if (!_$currencyInformationForm.valid()) {
        return;
      }

      var currency = _$currencyInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _currenciesService
        .createOrEdit(currency)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditCurrencyModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
