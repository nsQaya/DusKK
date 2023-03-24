(function ($) {
  app.modals.CreateOrEditAirportModal = function () {
    var _airportsService = abp.services.app.airports;
    var _regionService = abp.services.app.regions;
    var _citiesService = abp.services.app.cities;
    var _districtsService = abp.services.app.districts;

    var _modalManager;
      var _$airportInformationForm = null;
      var _dualListbox = null;

    var _AirportcountryLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Airports/CountryLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Airports/_AirportCountryLookupTableModal.js',
      modalClass: 'CountryLookupTableModal',
    });
    var _AirportcityLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Airports/CityLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Airports/_AirportCityLookupTableModal.js',
      modalClass: 'CityLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
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

      _$airportInformationForm = _modalManager.getModal().find('form[name=AirportInformationsForm]');
      _$airportInformationForm.validate();

      _regionService.getAllRegionForTableDropdown().then(x=>{

        const selectedRegionsInput= modal.find("#Airport_Regions");
        let selectedRegions= [];
        if(selectedRegionsInput?.val()!=undefined){
          selectedRegions= [...selectedRegionsInput?.val()?.split(',').map(x=>parseInt(x))]
        }

        _dualListbox= new DualListbox("#Airport_Regions_Select",{
          availableTitle: app.localize('Options'),
          selectedTitle: app.localize('Selected'),
          searchPlaceholder: app.localize('Ara'),
          addButtonText: ">",
          removeButtonText: "<",
          addAllButtonText: ">>",
          removeAllButtonText: "<<",

          sortable: true,
          upButtonText: "ᐱ",
          downButtonText: "ᐯ",

          draggable: true,

          options: x.map(y=>({ text: y.displayName, value: y.id.toString(), selected: selectedRegions.includes(y.id) }))
        })


      })
    };

    $('#OpenCountryLookupTableButton').click(function () {
      var airport = _$airportInformationForm.serializeFormToObject();

      _AirportcountryLookupTableModal.open(
        { id: airport.countryId, displayName: airport.countryDisplayProperty },
        function (data) {
          _$airportInformationForm.find('input[name=countryDisplayProperty]').val(data.displayName);
          _$airportInformationForm.find('input[name=countryId]').val(data.id);
        }
      );
    });

    $('#ClearCountryDisplayPropertyButton').click(function () {
      _$airportInformationForm.find('input[name=countryDisplayProperty]').val('');
      _$airportInformationForm.find('input[name=countryId]').val('');
    });

    $('#OpenCityLookupTableButton').click(function () {
      var airport = _$airportInformationForm.serializeFormToObject();

      _AirportcityLookupTableModal.open(
        { id: airport.cityId, displayName: airport.cityDisplayProperty },
        function (data) {
          _$airportInformationForm.find('input[name=cityDisplayProperty]').val(data.displayName);
          _$airportInformationForm.find('input[name=cityId]').val(data.id);
        }
      );
    });

    $('#ClearCityDisplayPropertyButton').click(function () {
      _$airportInformationForm.find('input[name=cityDisplayProperty]').val('');
      _$airportInformationForm.find('input[name=cityId]').val('');
    });

    $("#countryId").change(async function () {
      var res = await _citiesService.getAllCityForTableDropdown($("#countryId").val());

      $("#cityId").html(`<option value='' disabled selected> ${app.localize('SelectACity')} </option>`);

      res.forEach(city => {
        $("#cityId").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    this.save = function () {
      if (!_$airportInformationForm.valid()) {
        return;
      }
      if ($('#Airport_CountryId').prop('required') && $('#Airport_CountryId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
        return;
      }
      if ($('#Airport_CityId').prop('required') && $('#Airport_CityId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('City')));
        return;
      }

      var airport = _$airportInformationForm.serializeFormToObject();
      airport.regions= _dualListbox.options?.filter(x=>x.selected==true).map(x=>({RegionId: x.value}));

      console.log(airport);
      debugger;

      _modalManager.setBusy(true);
      _airportsService
        .createOrEdit(airport)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditAirportModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
