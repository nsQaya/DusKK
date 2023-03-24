(function ($) {
  app.modals.CreateOrEditFuneralModal = function () {
    var _funeralsService = abp.services.app.funerals;

    var _modalManager;
    var _$funeralInformationForm = null;

    var _FuneralfuneralTypeLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Funerals/FuneralTypeLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Funerals/_FuneralFuneralTypeLookupTableModal.js',
      modalClass: 'FuneralTypeLookupTableModal',
    });
    var _FuneralcontactLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Funerals/ContactLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Funerals/_FuneralContactLookupTableModal.js',
      modalClass: 'ContactLookupTableModal',
    });
    var _FuneralcompanyLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Funerals/CompanyLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Funerals/_FuneralCompanyLookupTableModal.js',
      modalClass: 'CompanyLookupTableModal',
    });
    var _FuneralorganizationUnitLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Funerals/OrganizationUnitLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Funerals/_FuneralOrganizationUnitLookupTableModal.js',
      modalClass: 'OrganizationUnitLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$funeralInformationForm = _modalManager.getModal().find('form[name=FuneralInformationsForm]');

      _$funeralInformationForm.validate();
    };

    $('#OpenFuneralTypeLookupTableButton').click(function () {
      var funeral = _$funeralInformationForm.serializeFormToObject();

      _FuneralfuneralTypeLookupTableModal.open(
        { id: funeral.typeId, displayName: funeral.funeralTypeDescription },
        function (data) {
          _$funeralInformationForm.find('input[name=funeralTypeDescription]').val(data.displayName);
          _$funeralInformationForm.find('input[name=typeId]').val(data.id);
        }
      );
    });

    $('#ClearFuneralTypeDescriptionButton').click(function () {
      _$funeralInformationForm.find('input[name=funeralTypeDescription]').val('');
      _$funeralInformationForm.find('input[name=typeId]').val('');
    });

    $('#OpenContactLookupTableButton').click(function () {
      var funeral = _$funeralInformationForm.serializeFormToObject();

      _FuneralcontactLookupTableModal.open(
        { id: funeral.contactId, displayName: funeral.contactDisplayProperty },
        function (data) {
          _$funeralInformationForm.find('input[name=contactDisplayProperty]').val(data.displayName);
          _$funeralInformationForm.find('input[name=contactId]').val(data.id);
        }
      );
    });

    $('#ClearContactDisplayPropertyButton').click(function () {
      _$funeralInformationForm.find('input[name=contactDisplayProperty]').val('');
      _$funeralInformationForm.find('input[name=contactId]').val('');
    });

    $('#OpenCompanyLookupTableButton').click(function () {
      var funeral = _$funeralInformationForm.serializeFormToObject();

      _FuneralcompanyLookupTableModal.open(
        { id: funeral.senderCompanyId, displayName: funeral.companyDisplayProperty },
        function (data) {
          _$funeralInformationForm.find('input[name=companyDisplayProperty]').val(data.displayName);
          _$funeralInformationForm.find('input[name=senderCompanyId]').val(data.id);
        }
      );
    });

    $('#ClearCompanyDisplayPropertyButton').click(function () {
      _$funeralInformationForm.find('input[name=companyDisplayProperty]').val('');
      _$funeralInformationForm.find('input[name=senderCompanyId]').val('');
    });

    $('#OpenOrganizationUnitLookupTableButton').click(function () {
      var funeral = _$funeralInformationForm.serializeFormToObject();

      _FuneralorganizationUnitLookupTableModal.open(
        { id: funeral.organizationUnitId, displayName: funeral.organizationUnitDisplayName },
        function (data) {
          _$funeralInformationForm.find('input[name=organizationUnitDisplayName]').val(data.displayName);
          _$funeralInformationForm.find('input[name=organizationUnitId]').val(data.id);
        }
      );
    });

    $('#ClearOrganizationUnitDisplayNameButton').click(function () {
      _$funeralInformationForm.find('input[name=organizationUnitDisplayName]').val('');
      _$funeralInformationForm.find('input[name=organizationUnitId]').val('');
    });

    this.save = function () {
      if (!_$funeralInformationForm.valid()) {
        return;
      }
      if ($('#Funeral_TypeId').prop('required') && $('#Funeral_TypeId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('FuneralType')));
        return;
      }
      if ($('#Funeral_ContactId').prop('required') && $('#Funeral_ContactId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Contact')));
        return;
      }
      if ($('#Funeral_SenderCompanyId').prop('required') && $('#Funeral_SenderCompanyId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Company')));
        return;
      }
      if ($('#Funeral_OrganizationUnitId').prop('required') && $('#Funeral_OrganizationUnitId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('OrganizationUnit')));
        return;
      }

      var funeral = _$funeralInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _funeralsService
        .createOrEdit(funeral)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditFuneralModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
