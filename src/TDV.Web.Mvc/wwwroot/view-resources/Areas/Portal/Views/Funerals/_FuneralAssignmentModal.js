(function ($) {
    app.modals.FuneralAssignmentModal = function () {
        var _funeralsService = abp.services.app.funerals;
        var _$funeralInformationForm = null;
        
        var _funeralIDs= 

        this.init = function (modalManager) {
            _modalManager = modalManager;
    
            var modal = _modalManager.getModal();
            _funeralIDs= _modalManager.getArgs().ids;

            modal.find("#companyId").select2({
                placeholder: app.localize("SelectACompany"),
                theme: "bootstrap5",
                selectionCssClass: 'form-select',
                width: '100%',
                dropdownParent: modal
            });

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


            $("#companyId").change(function(){
                _funeralsService.getAllEmployeeForTableDropdown($("#companyId").val(),"Driver").then(res=>{
                    $("#employeeId").html(`<option value='' disabled selected> ${app.localize('SelectAEmployee')} </option>`);
                    res.forEach(company => {
                        $("#employeeId").append(`<option value="${company.id}">${company.displayName}</option>`);
                    });
                })

                _funeralsService.getAllVehicleForTableDropdown($("#companyId").val(),"Driver").then(res=>{
                    $("#vehicleId").html(`<option value='' disabled selected> ${app.localize('SelectAVehicle')} </option>`);
                    res.forEach(vehicle => {
                        $("#vehicleId").append(`<option value="${vehicle.id}">${vehicle.displayName}</option>`);
                    });
                })

            })
        };

        this.save = function () {
            if (!_$funeralInformationForm.valid()) {
              return;
            }

            var assigment = _$funeralInformationForm.serializeFormToObject();
            console.log(assigment);

            _funeralsService.funeralAssigment(_funeralIDs, assigment.companyId, assigment.employeeId, assigment.vehicleId)
            .done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.funeralAssigmentModalSaved');
              })
              .always(function () {
                _modalManager.setBusy(false);
              });
        }
    }
})(jQuery);