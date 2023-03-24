(function ($) {
  app.modals.CreateOrEditContactDetailModal = function () {
    var _contactDetailsService = abp.services.app.contactDetails;

    var _modalManager;
    var _$contactDetailInformationForm = null;

    var _ContactDetailcontactLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/ContactDetails/ContactLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/Portal/Views/ContactDetails/_ContactDetailContactLookupTableModal.js',
      modalClass: 'ContactLookupTableModal',
    });

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

    $('#OpenContactLookupTableButton').click(function () {
      var contactDetail = _$contactDetailInformationForm.serializeFormToObject();

      _ContactDetailcontactLookupTableModal.open(
        { id: contactDetail.contactId, displayName: contactDetail.contactIdentifyNo },
        function (data) {
          _$contactDetailInformationForm.find('input[name=contactIdentifyNo]').val(data.displayName);
          _$contactDetailInformationForm.find('input[name=contactId]').val(data.id);
        }
      );
    });

    $('#ClearContactIdentifyNoButton').click(function () {
      _$contactDetailInformationForm.find('input[name=contactIdentifyNo]').val('');
      _$contactDetailInformationForm.find('input[name=contactId]').val('');
    });

    this.save = function () {
      if (!_$contactDetailInformationForm.valid()) {
        return;
      }
      if ($('#ContactDetail_ContactId').prop('required') && $('#ContactDetail_ContactId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Contact')));
        return;
      }

      var contactDetail = _$contactDetailInformationForm.serializeFormToObject();

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
