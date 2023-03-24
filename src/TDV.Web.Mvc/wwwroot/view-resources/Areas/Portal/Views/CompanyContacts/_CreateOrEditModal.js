(function ($) {
  app.modals.CreateOrEditCompanyContactModal = function () {
    var _companyContactsService = abp.services.app.companyContacts;

    var _modalManager;
    var _$companyContactInformationForm = null;

    var _CompanyContactcompanyLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyContacts/CompanyLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/Portal/Views/CompanyContacts/_CompanyContactCompanyLookupTableModal.js',
      modalClass: 'CompanyLookupTableModal',
    });
    var _CompanyContactcontactLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyContacts/ContactLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/Portal/Views/CompanyContacts/_CompanyContactContactLookupTableModal.js',
      modalClass: 'ContactLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;
      var args= _modalManager.getArgs();
      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      if (args.companyId != null && args.companyId!=0){
        modal.find("#CompanyContact_CompanyId").val(args.companyId);
        $(modal.find(".my-3")[0]).hide();
      }

      _$companyContactInformationForm = _modalManager.getModal().find('form[name=CompanyContactInformationsForm]');
      _$companyContactInformationForm.validate();
    };

    $('#OpenCompanyLookupTableButton').click(function () {
      var companyContact = _$companyContactInformationForm.serializeFormToObject();

      _CompanyContactcompanyLookupTableModal.open(
        { id: companyContact.companyId, displayName: companyContact.companyDisplayProperty },
        function (data) {
          _$companyContactInformationForm.find('input[name=companyDisplayProperty]').val(data.displayName);
          _$companyContactInformationForm.find('input[name=companyId]').val(data.id);
        }
      );
    });

    $('#ClearCompanyDisplayPropertyButton').click(function () {
      _$companyContactInformationForm.find('input[name=companyDisplayProperty]').val('');
      _$companyContactInformationForm.find('input[name=companyId]').val('');
    });

    $('#OpenContactLookupTableButton').click(function () {
      var companyContact = _$companyContactInformationForm.serializeFormToObject();

      _CompanyContactcontactLookupTableModal.open(
        { id: companyContact.contactId, displayName: companyContact.contactName },
        function (data) {
          _$companyContactInformationForm.find('input[name=contactName]').val(data.displayName);
          _$companyContactInformationForm.find('input[name=contactId]').val(data.id);
        }
      );
    });

    $('#ClearContactNameButton').click(function () {
      _$companyContactInformationForm.find('input[name=contactName]').val('');
      _$companyContactInformationForm.find('input[name=contactId]').val('');
    });

    this.save = function () {
      if (!_$companyContactInformationForm.valid()) {
        return;
      }
      if ($('#CompanyContact_CompanyId').prop('required') && $('#CompanyContact_CompanyId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Company')));
        return;
      }
      if ($('#CompanyContact_ContactId').prop('required') && $('#CompanyContact_ContactId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Contact')));
        return;
      }

      var companyContact = _$companyContactInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _companyContactsService
        .createOrEdit(companyContact)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditCompanyContactModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
