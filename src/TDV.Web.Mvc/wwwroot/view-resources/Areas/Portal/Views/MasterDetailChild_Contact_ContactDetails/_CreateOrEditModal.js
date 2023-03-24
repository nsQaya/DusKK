(function ($) {
  app.modals.MasterDetailChild_Contact_CreateOrEditContactDetailModal = function () {
    var _contactDetailsService = abp.services.app.contactDetails;

    var _modalManager;
    var _$contactDetailInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$contactDetailInformationForm = _modalManager.getModal().find('form[name=ContactDetailInformationsForm]');
      _$contactDetailInformationForm.validate();
    };

    this.save = function () {
      if (!_$contactDetailInformationForm.valid()) {
        return;
      }

      var contactDetail = _$contactDetailInformationForm.serializeFormToObject();

      contactDetail.contactId = $('#MasterDetailChild_Contact_ContactDetailsId').val();

      _modalManager.setBusy(true);
      _contactDetailsService
        .createOrEdit(contactDetail)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditContactDetailModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
