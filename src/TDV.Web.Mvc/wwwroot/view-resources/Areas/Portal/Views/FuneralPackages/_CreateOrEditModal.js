(function ($) {
  app.modals.CreateOrEditFuneralPackageModal = function () {
    var _funeralPackagesService = abp.services.app.funeralPackages;

    var _modalManager;
    var _$funeralPackageInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$funeralPackageInformationForm = _modalManager.getModal().find('form[name=FuneralPackageInformationsForm]');
      _$funeralPackageInformationForm.validate();
    };

    this.save = function () {
      if (!_$funeralPackageInformationForm.valid()) {
        return;
      }

      var funeralPackage = _$funeralPackageInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _funeralPackagesService
        .createOrEdit(funeralPackage)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditFuneralPackageModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
