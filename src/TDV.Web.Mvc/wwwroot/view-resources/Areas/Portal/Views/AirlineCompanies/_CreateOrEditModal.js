(function ($) {
  app.modals.CreateOrEditAirlineCompanyModal = function () {
    var _airlineCompaniesService = abp.services.app.airlineCompanies;

    var _modalManager;
    var _$airlineCompanyInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$airlineCompanyInformationForm = _modalManager.getModal().find('form[name=AirlineCompanyInformationsForm]');
      _$airlineCompanyInformationForm.validate();
    };

    this.save = function () {
      if (!_$airlineCompanyInformationForm.valid()) {
        return;
      }

      var airlineCompany = _$airlineCompanyInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _airlineCompaniesService
        .createOrEdit(airlineCompany)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditAirlineCompanyModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
