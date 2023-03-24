(function ($) {
  app.modals.CreateOrEditStokModal = function () {
    var _stoksService = abp.services.app.stoks;

    var _modalManager;
    var _$stokInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$stokInformationForm = _modalManager.getModal().find('form[name=StokInformationsForm]');
      _$stokInformationForm.validate();
    };

    this.save = function () {
      if (!_$stokInformationForm.valid()) {
        return;
      }

      var stok = _$stokInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _stoksService
        .createOrEdit(stok)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditStokModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
