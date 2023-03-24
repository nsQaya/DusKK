(function ($) {
  app.modals.CreateOrEditTalepModal = function () {
    var _talepsService = abp.services.app.taleps;

    var _modalManager;
    var _$talepInformationForm = null;

    var _TalepstokLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Taleps/StokLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Taleps/_TalepStokLookupTableModal.js',
      modalClass: 'StokLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$talepInformationForm = _modalManager.getModal().find('form[name=TalepInformationsForm]');
      _$talepInformationForm.validate();
    };

    $('#OpenStokLookupTableButton').click(function () {
      var talep = _$talepInformationForm.serializeFormToObject();

      _TalepstokLookupTableModal.open({ id: talep.stokId, displayName: talep.stokAdi }, function (data) {
        _$talepInformationForm.find('input[name=stokAdi]').val(data.displayName);
        _$talepInformationForm.find('input[name=stokId]').val(data.id);
      });
    });

    $('#ClearStokAdiButton').click(function () {
      _$talepInformationForm.find('input[name=stokAdi]').val('');
      _$talepInformationForm.find('input[name=stokId]').val('');
    });

    this.save = function () {
      if (!_$talepInformationForm.valid()) {
        return;
      }
      if ($('#Talep_StokId').prop('required') && $('#Talep_StokId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Stok')));
        return;
      }

      var talep = _$talepInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _talepsService
        .createOrEdit(talep)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditTalepModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
