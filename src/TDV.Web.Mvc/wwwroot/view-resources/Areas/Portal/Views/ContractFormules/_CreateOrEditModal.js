(function ($) {
  app.modals.CreateOrEditContractFormuleModal = function () {
    var _contractFormulesService = abp.services.app.contractFormules;

    var _modalManager;
    var _$contractFormuleInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$contractFormuleInformationForm = _modalManager.getModal().find('form[name=ContractFormuleInformationsForm]');
      _$contractFormuleInformationForm.validate();
    };

    this.save = function () {
      if (!_$contractFormuleInformationForm.valid()) {
        return;
      }

      var contractFormule = _$contractFormuleInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _contractFormulesService
        .createOrEdit(contractFormule)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditContractFormuleModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
