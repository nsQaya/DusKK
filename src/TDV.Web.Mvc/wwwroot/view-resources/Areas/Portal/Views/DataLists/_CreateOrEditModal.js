(function ($) {
  app.modals.CreateOrEditDataListModal = function () {
    var _dataListsService = abp.services.app.dataLists;

    var _modalManager;
    var _$dataListInformationForm = null;

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$dataListInformationForm = _modalManager.getModal().find('form[name=DataListInformationsForm]');
      _$dataListInformationForm.validate();
    };

    this.save = function () {
      if (!_$dataListInformationForm.valid()) {
        return;
      }

      var dataList = _$dataListInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _dataListsService
        .createOrEdit(dataList)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditDataListModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
