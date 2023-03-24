(function ($) {
  app.modals.CreateOrEditFuneralTranportOrderModal = function () {
    var _funeralTranportOrdersService = abp.services.app.funeralTranportOrders;

    var _modalManager;
    var _$funeralTranportOrderInformationForm = null;

    var _FuneralTranportOrderfuneralWorkOrderDetailLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralTranportOrders/FuneralWorkOrderDetailLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/FuneralTranportOrders/_FuneralTranportOrderFuneralWorkOrderDetailLookupTableModal.js',
      modalClass: 'FuneralWorkOrderDetailLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$funeralTranportOrderInformationForm = _modalManager
        .getModal()
        .find('form[name=FuneralTranportOrderInformationsForm]');
      _$funeralTranportOrderInformationForm.validate();
    };

    $('#OpenFuneralWorkOrderDetailLookupTableButton').click(function () {
      var funeralTranportOrder = _$funeralTranportOrderInformationForm.serializeFormToObject();

      _FuneralTranportOrderfuneralWorkOrderDetailLookupTableModal.open(
        {
          id: funeralTranportOrder.funeralWorkOrderDetailId,
          displayName: funeralTranportOrder.funeralWorkOrderDetailDescription,
        },
        function (data) {
          _$funeralTranportOrderInformationForm
            .find('input[name=funeralWorkOrderDetailDescription]')
            .val(data.displayName);
          _$funeralTranportOrderInformationForm.find('input[name=funeralWorkOrderDetailId]').val(data.id);
        }
      );
    });

    $('#ClearFuneralWorkOrderDetailDescriptionButton').click(function () {
      _$funeralTranportOrderInformationForm.find('input[name=funeralWorkOrderDetailDescription]').val('');
      _$funeralTranportOrderInformationForm.find('input[name=funeralWorkOrderDetailId]').val('');
    });

    this.save = function () {
      if (!_$funeralTranportOrderInformationForm.valid()) {
        return;
      }
      if (
        $('#FuneralTranportOrder_FuneralWorkOrderDetailId').prop('required') &&
        $('#FuneralTranportOrder_FuneralWorkOrderDetailId').val() == ''
      ) {
        abp.message.error(app.localize('{0}IsRequired', app.localize('FuneralWorkOrderDetail')));
        return;
      }

      var funeralTranportOrder = _$funeralTranportOrderInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _funeralTranportOrdersService
        .createOrEdit(funeralTranportOrder)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditFuneralTranportOrderModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
