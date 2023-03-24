(function ($) {
    app.modals.DriverAssignmentModal = function () {
        var _$funeralInformationForm = null;
        var _funeralsService = abp.services.app.funerals;
        
        var packageId=null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
    
            var modal = _modalManager.getModal();
            packageId= _modalManager.getArgs().packageId;

            modal.find("#employeeId").select2({
                placeholder: app.localize("SelectAEmployee"),
                theme: "bootstrap5",
                selectionCssClass: 'form-select',
                width: '100%',
                dropdownParent: modal
            });

            modal.find("#vehicleId").select2({
                placeholder: app.localize("SelectAVehicle"),
                theme: "bootstrap5",
                selectionCssClass: 'form-select',
                width: '100%',
                dropdownParent: modal
            });
        
            
            _$funeralInformationForm = _modalManager.getModal().find('form[name=FuneralInformationsForm]');
            _$funeralInformationForm.validate();

        };

        this.save = function () {
            if (!_$funeralInformationForm.valid()) {
              return;
            }

            var assigment = _$funeralInformationForm.serializeFormToObject();
            
            var tasks= [];
            
            if(assigment.vehicleId!=""){
                tasks.push(_funeralsService.funeralVehicleAssignment(packageId,assigment.vehicleId));
            }

            if(assigment.employeeId!=""){
                tasks.push(_funeralsService.funeralDriverAssigment(packageId,assigment.employeeId));
            }

            Promise.all(tasks)
            .then(()=>{
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.funeralAssigmentModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });

        }
    }
})(jQuery);