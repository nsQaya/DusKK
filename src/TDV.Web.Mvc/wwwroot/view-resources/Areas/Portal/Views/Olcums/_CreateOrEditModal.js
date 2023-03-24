(function ($) {
  app.modals.CreateOrEditOlcumModal = function () {
    var _olcumsService = abp.services.app.olcums;

    var _modalManager;
    var _$olcumInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$olcumInformationForm = _modalManager.getModal().find('form[name=OlcumInformationsForm]');
      _$olcumInformationForm.validate();
    };

    this.save = function () {
      if (!_$olcumInformationForm.valid()) {
        return;
      }

      var olcum = _$olcumInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _olcumsService
        .createOrEdit(olcum)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditOlcumModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
