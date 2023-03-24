(function ($) {
  app.modals.CreateOrEditFuneralDocumentModal = function () {
    var _funeralDocumentsService = abp.services.app.funeralDocuments;

    var _modalManager;
    var _$funeralDocumentInformationForm = null;

    var _FuneralDocumentfuneralLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralDocuments/FuneralLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/Portal/Views/FuneralDocuments/_FuneralDocumentFuneralLookupTableModal.js',
      modalClass: 'FuneralLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$funeralDocumentInformationForm = _modalManager.getModal().find('form[name=FuneralDocumentInformationsForm]');
      _$funeralDocumentInformationForm.validate();
    };

    $('#OpenFuneralLookupTableButton').click(function () {
      var funeralDocument = _$funeralDocumentInformationForm.serializeFormToObject();

      _FuneralDocumentfuneralLookupTableModal.open(
        { id: funeralDocument.funeralId, displayName: funeralDocument.funeralDisplayProperty },
        function (data) {
          _$funeralDocumentInformationForm.find('input[name=funeralDisplayProperty]').val(data.displayName);
          _$funeralDocumentInformationForm.find('input[name=funeralId]').val(data.id);
        }
      );
    });

    $('#ClearFuneralDisplayPropertyButton').click(function () {
      _$funeralDocumentInformationForm.find('input[name=funeralDisplayProperty]').val('');
      _$funeralDocumentInformationForm.find('input[name=funeralId]').val('');
    });

    this.save = function () {
      if (!_$funeralDocumentInformationForm.valid()) {
        return;
      }
      if ($('#FuneralDocument_FuneralId').prop('required') && $('#FuneralDocument_FuneralId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Funeral')));
        return;
      }

      var funeralDocument = _$funeralDocumentInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _funeralDocumentsService
        .createOrEdit(funeralDocument)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditFuneralDocumentModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
