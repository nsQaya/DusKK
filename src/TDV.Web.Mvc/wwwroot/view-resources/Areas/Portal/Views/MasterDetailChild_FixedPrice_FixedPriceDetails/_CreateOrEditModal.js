(function ($) {
  app.modals.MasterDetailChild_FixedPrice_CreateOrEditFixedPriceDetailModal = function () {
    var _fixedPriceDetailsService = abp.services.app.fixedPriceDetails;

    var _modalManager;
    var _$fixedPriceDetailInformationForm = null;

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

    this.save = function () {
      if (!_$fixedPriceDetailInformationForm.valid()) {
        return;
      }

      var fixedPriceDetail = _$fixedPriceDetailInformationForm.serializeFormToObject();

      fixedPriceDetail.fixedPriceId = $('#MasterDetailChild_FixedPrice_FixedPriceDetailsId').val();

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
