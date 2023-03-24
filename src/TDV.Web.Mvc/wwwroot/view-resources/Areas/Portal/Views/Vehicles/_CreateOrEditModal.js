(function ($) {
  app.modals.CreateOrEditVehicleModal = function () {
    var _vehiclesService = abp.services.app.vehicles;

    var _modalManager;
    var _$vehicleInformationForm = null;

    var _VehiclecompanyLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Vehicles/CompanyLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Vehicles/_VehicleCompanyLookupTableModal.js',
      modalClass: 'CompanyLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;
      var args = _modalManager.getArgs();

 
      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      if (args.companyId != null && args.companyId!=0){
          modal.find("#Vehicle_CompanyId").val(args.companyId);
          $(modal.find(".my-3")[0]).hide();
      }

      _$vehicleInformationForm = _modalManager.getModal().find('form[name=VehicleInformationsForm]');
      _$vehicleInformationForm.validate();
    };

    $('#OpenCompanyLookupTableButton').click(function () {
      var vehicle = _$vehicleInformationForm.serializeFormToObject();

      _VehiclecompanyLookupTableModal.open(
        { id: vehicle.companyId, displayName: vehicle.companyDisplayProperty },
        function (data) {
          _$vehicleInformationForm.find('input[name=companyDisplayProperty]').val(data.displayName);
          _$vehicleInformationForm.find('input[name=companyId]').val(data.id);
        }
      );
    });

    $('#ClearCompanyDisplayPropertyButton').click(function () {
      _$vehicleInformationForm.find('input[name=companyDisplayProperty]').val('');
      _$vehicleInformationForm.find('input[name=companyId]').val('');
    });

    this.save = function () {
      if (!_$vehicleInformationForm.valid()) {
        return;
      }
      if ($('#Vehicle_CompanyId').prop('required') && $('#Vehicle_CompanyId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Company')));
        return;
      }

      var vehicle = _$vehicleInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _vehiclesService
        .createOrEdit(vehicle)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditVehicleModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
