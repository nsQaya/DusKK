(function ($) {
  app.modals.CreateOrEditFuneralAddresModal = function () {
    var _funeralAddresesService = abp.services.app.funeralAddreses;

    var _modalManager;
    var _$funeralAddresInformationForm = null;

    var _FuneralAddresfuneralLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralAddreses/FuneralLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/Portal/Views/FuneralAddreses/_FuneralAddresFuneralLookupTableModal.js',
      modalClass: 'FuneralLookupTableModal',
    });
    var _FuneralAddresquarterLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralAddreses/QuarterLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/Portal/Views/FuneralAddreses/_FuneralAddresQuarterLookupTableModal.js',
      modalClass: 'QuarterLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$funeralAddresInformationForm = _modalManager.getModal().find('form[name=FuneralAddresInformationsForm]');
      _$funeralAddresInformationForm.validate();
    };

    $('#OpenFuneralLookupTableButton').click(function () {
      var funeralAddres = _$funeralAddresInformationForm.serializeFormToObject();

      _FuneralAddresfuneralLookupTableModal.open(
        { id: funeralAddres.funeralId, displayName: funeralAddres.funeralDisplayProperty },
        function (data) {
          _$funeralAddresInformationForm.find('input[name=funeralDisplayProperty]').val(data.displayName);
          _$funeralAddresInformationForm.find('input[name=funeralId]').val(data.id);
        }
      );
    });

    $('#ClearFuneralDisplayPropertyButton').click(function () {
      _$funeralAddresInformationForm.find('input[name=funeralDisplayProperty]').val('');
      _$funeralAddresInformationForm.find('input[name=funeralId]').val('');
    });

    $('#OpenQuarterLookupTableButton').click(function () {
      var funeralAddres = _$funeralAddresInformationForm.serializeFormToObject();

      _FuneralAddresquarterLookupTableModal.open(
        { id: funeralAddres.quarterId, displayName: funeralAddres.quarterName },
        function (data) {
          _$funeralAddresInformationForm.find('input[name=quarterName]').val(data.displayName);
          _$funeralAddresInformationForm.find('input[name=quarterId]').val(data.id);
        }
      );
    });

    $('#ClearQuarterNameButton').click(function () {
      _$funeralAddresInformationForm.find('input[name=quarterName]').val('');
      _$funeralAddresInformationForm.find('input[name=quarterId]').val('');
    });

    this.save = function () {
      if (!_$funeralAddresInformationForm.valid()) {
        return;
      }
      if ($('#FuneralAddres_FuneralId').prop('required') && $('#FuneralAddres_FuneralId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Funeral')));
        return;
      }
      if ($('#FuneralAddres_QuarterId').prop('required') && $('#FuneralAddres_QuarterId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Quarter')));
        return;
      }

      var funeralAddres = _$funeralAddresInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _funeralAddresesService
        .createOrEdit(funeralAddres)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditFuneralAddresModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
