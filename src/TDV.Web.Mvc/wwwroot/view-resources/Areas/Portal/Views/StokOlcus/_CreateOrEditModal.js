(function ($) {
  app.modals.CreateOrEditStokOlcuModal = function () {
    var _stokOlcusService = abp.services.app.stokOlcus;

    var _modalManager;
    var _$stokOlcuInformationForm = null;

    var _StokOlcustokLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/StokOlcus/StokLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/StokOlcus/_StokOlcuStokLookupTableModal.js',
      modalClass: 'StokLookupTableModal',
    });
    var _StokOlcuolcumLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/StokOlcus/OlcumLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/StokOlcus/_StokOlcuOlcumLookupTableModal.js',
      modalClass: 'OlcumLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$stokOlcuInformationForm = _modalManager.getModal().find('form[name=StokOlcuInformationsForm]');
      _$stokOlcuInformationForm.validate();
    };

    $('#OpenStokLookupTableButton').click(function () {
      var stokOlcu = _$stokOlcuInformationForm.serializeFormToObject();

      _StokOlcustokLookupTableModal.open({ id: stokOlcu.stokId, displayName: stokOlcu.stokAdi }, function (data) {
        _$stokOlcuInformationForm.find('input[name=stokAdi]').val(data.displayName);
        _$stokOlcuInformationForm.find('input[name=stokId]').val(data.id);
      });
    });

    $('#ClearStokAdiButton').click(function () {
      _$stokOlcuInformationForm.find('input[name=stokAdi]').val('');
      _$stokOlcuInformationForm.find('input[name=stokId]').val('');
    });

    $('#OpenOlcumLookupTableButton').click(function () {
      var stokOlcu = _$stokOlcuInformationForm.serializeFormToObject();

      _StokOlcuolcumLookupTableModal.open(
        { id: stokOlcu.olcumId, displayName: stokOlcu.olcumOlcuTipi },
        function (data) {
          _$stokOlcuInformationForm.find('input[name=olcumOlcuTipi]').val(data.displayName);
          _$stokOlcuInformationForm.find('input[name=olcumId]').val(data.id);
        }
      );
    });

    $('#ClearOlcumOlcuTipiButton').click(function () {
      _$stokOlcuInformationForm.find('input[name=olcumOlcuTipi]').val('');
      _$stokOlcuInformationForm.find('input[name=olcumId]').val('');
    });

    this.save = function () {
      if (!_$stokOlcuInformationForm.valid()) {
        return;
      }
      if ($('#StokOlcu_StokId').prop('required') && $('#StokOlcu_StokId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Stok')));
        return;
      }
      if ($('#StokOlcu_OlcumId').prop('required') && $('#StokOlcu_OlcumId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Olcum')));
        return;
      }

      var stokOlcu = _$stokOlcuInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _stokOlcusService
        .createOrEdit(stokOlcu)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditStokOlcuModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
