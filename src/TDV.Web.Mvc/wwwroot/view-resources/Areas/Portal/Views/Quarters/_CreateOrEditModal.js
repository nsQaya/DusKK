(function ($) {
  app.modals.CreateOrEditQuarterModal = function () {
    var _quartersService = abp.services.app.quarters;
    var _districtsService = abp.services.app.districts;


    var _modalManager;
    var _$quarterInformationForm = null;

    var _QuarterdistrictLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Quarters/DistrictLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Quarters/_QuarterDistrictLookupTableModal.js',
      modalClass: 'DistrictLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      modal.find("#regionId").select2({
        placeholder: app.localize("SelectARegion"),
        theme: "bootstrap5",
        selectionCssClass: 'form-select',
        width: '100%',
        dropdownParent: modal
      });

      modal.find("#countryId").select2({
        placeholder: app.localize("SelectACountry"),
        theme: "bootstrap5",
        selectionCssClass: 'form-select',
        width: '100%',
        dropdownParent: modal
      });

      modal.find("#cityId").select2({
        placeholder: app.localize("SelectACity"),
        theme: "bootstrap5",
        selectionCssClass: 'form-select',
        width: '100%',
        dropdownParent: modal,
      });

      modal.find("#districtId").select2({
        placeholder: app.localize("SelectADistrict"),
        theme: "bootstrap5",
        selectionCssClass: 'form-select',
        width: '100%',
        dropdownParent: modal,
      });

      _$quarterInformationForm = _modalManager.getModal().find('form[name=QuarterInformationsForm]');
      _$quarterInformationForm.validate();
    };

    $('#OpenDistrictLookupTableButton').click(function () {
      var quarter = _$quarterInformationForm.serializeFormToObject();

      _QuarterdistrictLookupTableModal.open(
        { id: quarter.districtId, displayName: quarter.districtName },
        function (data) {
          _$quarterInformationForm.find('input[name=districtName]').val(data.displayName);
          _$quarterInformationForm.find('input[name=districtId]').val(data.id);
        }
      );
    });

    $('#ClearDistrictNameButton').click(function () {
      _$quarterInformationForm.find('input[name=districtName]').val('');
      _$quarterInformationForm.find('input[name=districtId]').val('');
    });

    $("#countryId").change(async function () {
      var res = await _districtsService.getAllCityForTableDropdown($("#countryId").val());

      $("#cityId").html(`<option value='' disabled selected> ${app.localize('SelectACity')} </option>`);
      $("#districtId").html(`<option value='' disabled selected> ${app.localize('SelectACity')} </option>`);

      res.forEach(city => {
        $("#cityId").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    $("#cityId").change(async function () {
      var res = await _quartersService.getAllDistrictForTableDropdown($("#cityId").val());
      $("#districtId").html(`<option value='' disabled selected> ${app.localize('SelectADistrict')} </option>`);

      res.forEach(city => {
        $("#districtId").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    this.save = function () {
      if (!_$quarterInformationForm.valid()) {
        return;
      }
      if ($('#Quarter_DistrictId').prop('required') && $('#Quarter_DistrictId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('District')));
        return;
      }

      var quarter = _$quarterInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _quartersService
        .createOrEdit(quarter)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditQuarterModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
