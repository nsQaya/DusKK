(function ($) {
  app.modals.CreateOrEditFixedPriceDetailModal = function () {
    var _fixedPriceDetailsService = abp.services.app.fixedPriceDetails;

    var _modalManager;
    var _$fixedPriceDetailInformationForm = null;

    var _FixedPriceDetailfixedPriceLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FixedPriceDetails/FixedPriceLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/FixedPriceDetails/_FixedPriceDetailFixedPriceLookupTableModal.js',
      modalClass: 'FixedPriceLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$fixedPriceDetailInformationForm = _modalManager.getModal().find('form[name=FixedPriceDetailInformationsForm]');
      _$fixedPriceDetailInformationForm.validate();
    };

    $('#OpenFixedPriceLookupTableButton').click(function () {
      var fixedPriceDetail = _$fixedPriceDetailInformationForm.serializeFormToObject();

      _FixedPriceDetailfixedPriceLookupTableModal.open(
        { id: fixedPriceDetail.fixedPriceId, displayName: fixedPriceDetail.fixedPriceName },
        function (data) {
          _$fixedPriceDetailInformationForm.find('input[name=fixedPriceName]').val(data.displayName);
          _$fixedPriceDetailInformationForm.find('input[name=fixedPriceId]').val(data.id);
        }
      );
    });

    $('#ClearFixedPriceNameButton').click(function () {
      _$fixedPriceDetailInformationForm.find('input[name=fixedPriceName]').val('');
      _$fixedPriceDetailInformationForm.find('input[name=fixedPriceId]').val('');
    });

    this.save = function () {
      if (!_$fixedPriceDetailInformationForm.valid()) {
        return;
      }
      if ($('#FixedPriceDetail_FixedPriceId').prop('required') && $('#FixedPriceDetail_FixedPriceId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('FixedPrice')));
        return;
      }

      var fixedPriceDetail = _$fixedPriceDetailInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _fixedPriceDetailsService
        .createOrEdit(fixedPriceDetail)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditFixedPriceDetailModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
