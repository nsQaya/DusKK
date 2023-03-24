(function ($) {
  app.modals.CreateOrEditDistrictModal = function () {
    var _districtsService = abp.services.app.districts;

    var _modalManager;
    var _$districtInformationForm = null;

    var _DistrictcityLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Districts/CityLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Districts/_DistrictCityLookupTableModal.js',
      modalClass: 'CityLookupTableModal',
    });
    var _DistrictregionLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Districts/RegionLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Districts/_DistrictRegionLookupTableModal.js',
      modalClass: 'RegionLookupTableModal',
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
        placeholder: app.localize("SelectACountry"),
        theme: "bootstrap5",
        selectionCssClass: 'form-select',
        width: '100%',
        dropdownParent: modal,
      });

      _$districtInformationForm = _modalManager.getModal().find('form[name=DistrictInformationsForm]');
      _$districtInformationForm.validate();
    };

    $('#OpenCityLookupTableButton').click(function () {
      var district = _$districtInformationForm.serializeFormToObject();

      _DistrictcityLookupTableModal.open(
        { id: district.cityId, displayName: district.cityDisplayProperty },
        function (data) {
          _$districtInformationForm.find('input[name=cityDisplayProperty]').val(data.displayName);
          _$districtInformationForm.find('input[name=cityId]').val(data.id);
        }
      );
    });

    $('#ClearCityDisplayPropertyButton').click(function () {
      _$districtInformationForm.find('input[name=cityDisplayProperty]').val('');
      _$districtInformationForm.find('input[name=cityId]').val('');
    });

    $('#OpenRegionLookupTableButton').click(function () {
      var district = _$districtInformationForm.serializeFormToObject();

      _DistrictregionLookupTableModal.open(
        { id: district.regionId, displayName: district.regionName },
        function (data) {
          _$districtInformationForm.find('input[name=regionName]').val(data.displayName);
          _$districtInformationForm.find('input[name=regionId]').val(data.id);
        }
      );
    });

    $('#ClearRegionNameButton').click(function () {
      _$districtInformationForm.find('input[name=regionName]').val('');
      _$districtInformationForm.find('input[name=regionId]').val('');
    });

    $("#countryId").change(async function () {
      var res = await _districtsService.getAllCityForTableDropdown($("#countryId").val());
      
      $("#cityId").html(`<option value='' disabled selected> ${app.localize('SelectACity')} </option>`);

      res.forEach(city => {
        $("#cityId").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    this.save = function () {
      if (!_$districtInformationForm.valid()) {
        return;
      }
      if ($('#District_CityId').prop('required') && $('#District_CityId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('City')));
        return;
      }
      if ($('#District_RegionId').prop('required') && $('#District_RegionId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Region')));
        return;
      }

      var district = _$districtInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _districtsService
        .createOrEdit(district)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditDistrictModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
