(function () {
  $(function () {
    var _companiesService = abp.services.app.companies;

    var _districtService= abp.services.app.districts;
    var _quartersService = abp.services.app.quarters;
    var _citiesService= abp.services.app.cities;

    var _$companyInformationForm = $('form[name=CompanyInformationsForm]');
    _$companyInformationForm.validate();


    $('.date-picker').daterangepicker({
      singleDatePicker: true,
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });


    $(".select2").select2({
      theme: "bootstrap5",
      selectionCssClass: 'form-select',
      width: '100%',
    });

    $("#countryId").change(async function () {
      var res = await _citiesService.getAllCityForTableDropdown($("#countryId").val());

      $("#cityId").html(`<option value='' disabled selected> ${app.localize('SelectACity')} </option>`);
      $("#districtId").html(`<option value='' disabled selected> ${app.localize('SelectADistrict')} </option>`);
      $("#quarterId").html(`<option value='' disabled selected> ${app.localize('SelectAQuarter')} </option>`);

      res.forEach(city => {
        $("#cityId").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    $("#cityId").change(async function () {
      var res = await _districtService.getAllDistrictForTableDropdown($("#cityId").val());
      $("#districtId").html(`<option value='' disabled selected> ${app.localize('SelectADistrict')} </option>`);

      res.forEach(city => {
        $("#districtId").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    $("#districtId").change(async function () {
      var res = await _quartersService.getAllQuartersForTableDropdown($("#districtId").val());
      $("#quarterId").html(`<option value='' disabled selected> ${app.localize('SelectAQuarter')} </option>`);

      res.forEach(city => {
        $("#quarterId").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    $("#quarterId").change(async function(){
     _districtService.getRegionFromDistrict($("#districtId").val())
          .then(x=>{
            _$funeralInformationForm.find("input[name='regionId']").val(x.region.id);
            _$funeralInformationForm.find("[name='region']").val(x.region.name);
          })
    });

    function save(successCallback) {
      if (!_$companyInformationForm.valid()) {
        return;
      }

      var company = _$companyInformationForm.serializeFormToObject();

      abp.ui.setBusy();
      _companiesService
        .createOrEdit(company)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          abp.event.trigger('app.createOrEditCompanyModalSaved');

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$companyInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/Portal/Companies';
      });
    });

    $('#saveAndNewBtn').click(function () {
      save(function () {
        if (!$('input[name=id]').val()) {
          //if it is create page
          clearForm();
        }
      });
    });
  });
})();
