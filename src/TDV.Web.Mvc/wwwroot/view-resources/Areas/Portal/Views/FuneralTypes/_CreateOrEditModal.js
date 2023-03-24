(function ($) {
  app.modals.CreateOrEditFuneralTypeModal = function () {
    var _funeralTypesService = abp.services.app.funeralTypes;

    var _modalManager;
    var _$funeralTypeInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$funeralTypeInformationForm = _modalManager.getModal().find('form[name=FuneralTypeInformationsForm]');
      _$funeralTypeInformationForm.validate();
    };

    this.save = function () {
      if (!_$funeralTypeInformationForm.valid()) {
        return;
      }

      var funeralType = _$funeralTypeInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _funeralTypesService
        .createOrEdit(funeralType)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditFuneralTypeModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
