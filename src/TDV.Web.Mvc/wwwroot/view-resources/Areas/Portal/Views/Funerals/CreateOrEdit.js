(function () {
  $(function () {
    var _funeralsService = abp.services.app.funerals;
    var _funeralAddressService= abp.services.app.funeralAddreses;
    var _funeralDocumentsService= abp.services.app.funeralDocuments;
    var _funeralFlightService= abp.services.app.funeralFlights;


    var _districtService= abp.services.app.districts;
    var _quartersService = abp.services.app.quarters;
    var _citiesService= abp.services.app.cities;

    var _$funeralInformationForm = $('#funeralStepperForm');
    _$funeralInformationForm.validate();

    var _FuneralcontactLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/Funerals/ContactLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Funerals/_FuneralContactLookupTableModal.js',
      modalClass: 'ContactLookupTableModal',
    });

    var _stepperItems=[
      {
        form: $("#funeralMainInfos"),
        get idHolder(){
          return this.form.find("[name=id]");
        },
        get isEdit(){
          return this.idHolder.val()!=""
        },
        saveMethod: saveFuneralMains
      },
      {
        form: $("#funeralAddressInfos"),
        get idHolder(){
          return this.form.find("[name=id]");
        },
        get isEdit(){
          return this.idHolder.val()!=""
        },
        fetchMethod: getFuneralAddress,
        saveMethod: saveFuneralAddress
      },
      {
        form: $("#funeralDocumentsInfos"),
        fetchMethod: getFuneralDocuments,
        saveMethod: saveFuneralDocuments
      },
      {
        form: $("#funeralFlightInfos"),
        get idHolder(){
          return this.form.find("[name=id]");
        },
        get isEdit(){
          return this.idHolder.val()!=""
        },
        fetchMethod: getFuneralFlight,
        saveMethod: saveFuneralFlight
      }
    ];

    var _stepper= new KTStepper(document.querySelector("#funeralStepper"));

    _stepper.on("kt.stepper.next", async function (stepper) {
      let currentStepIndex= stepper.getCurrentStepIndex();
      let currentStep= _stepperItems[currentStepIndex-1];
      let nextStep= _stepperItems[currentStepIndex];

      if($("[name=id]").val()!="" && !currentStep.form.valid()){
        return;
      }

      abp.ui.block("#funeralStepper")
      var isSuccessCurrent=false;


      if(currentStep.saveMethod!=null){
        isSuccessCurrent= await currentStep.saveMethod()
      }

      if(isSuccessCurrent==true && nextStep!=null && nextStep.fetchMethod!=null){
       await nextStep.fetchMethod();
      }

      if(isSuccessCurrent){
        stepper.goNext();
      }

      abp.ui.unblock("#funeralStepper");
    });

    // Handle previous step
    _stepper.on("kt.stepper.previous", function (stepper) {
      stepper.goPrevious();
    });

    $(".select2").select2({
      theme: "bootstrap5",
      selectionCssClass: 'form-select',
      width: '100%',
    });

    $("#debug-stepper").click(()=>{
      _stepper.goNext();
    });


    $("[name=tcNo]").keyup(function(){
      var value= $(this).val();
      console.log(value);
      if(value.replaceAll('_','').length <= 0){
        $("[name=tcNo]").inputmask("")
      }else{
        $("[name=tcNo]").inputmask("99999999999")
      }
    });


    $("[name='contact[details][0][value]']").inputmask("+9999")

    const countryListForPhone={
      '+49': "+99 9999 9999999",
      '+90': "+99 (999) 999 99 99"
    }

    $("[name='contact[details][0][value]']").keyup(()=>{
      const countryCode= $("[name='contact[details][0][value]']").val();
      const clearVal= countryCode.replaceAll('+','').replaceAll('_','');

      if(clearVal.length > 4 ){
        return;
      }

      let currentPhoneMasking= null;

      const keys= Object.keys(countryListForPhone);
      const currentPhoneCode= keys.find(x=>`+${clearVal}`.startsWith(x));

      if(currentPhoneCode){
        currentPhoneMasking= countryListForPhone[currentPhoneCode];
      }


      switch(true){
        case countryCode.startsWith("+49"):
          currentPhoneMasking= "+99 9999 9999999";
          break;
      }

      if(!currentPhoneMasking){
        currentPhoneMasking= "+99 9999999999999"
      }
      $("[name='contact[details][0][value]']").inputmask(currentPhoneMasking);
    })


    $('.date-picker').daterangepicker({
      singleDatePicker: true,
      locale: abp.localization.currentLanguage.name,
      format: 'L',
    });


    $('#OpenContactLookupTableButton').click(function () {
      var funeral = _$funeralInformationForm.serializeFormToObject();

      _FuneralcontactLookupTableModal.open(
        { id: funeral.contactId, displayName: funeral.contactDisplayProperty },
        function (data) {
          _$funeralInformationForm.find('input[name=contactDisplayProperty]').val(data.displayName);
          _$funeralInformationForm.find('input[name="contact[id]"]').val(data.id);


          $("#contactInfos").hide();
        }
      );
    });

    $('#ClearContactDisplayPropertyButton').click(function () {
      _$funeralInformationForm.find('input[name=contactDisplayProperty]').val('');
      _$funeralInformationForm.find('input[name="contact[id]"]').val('');
      $("#contactInfos").show();
    });

    $("#address_countryId_").change(async function () {
      var res = await _citiesService.getAllCityForTableDropdown($("#address_countryId_").val());

      $("#address_cityId_").html(`<option value='' disabled selected> ${app.localize('SelectACity')} </option>`);
      $("#address_districtId_").html(`<option value='' disabled selected> ${app.localize('SelectADistrict')} </option>`);
      $("#address_quarterId_").html(`<option value='' disabled selected> ${app.localize('SelectAQuarter')} </option>`);

      res.forEach(city => {
        $("#address_cityId_").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    $("#address_cityId_").change(async function () {
      var res = await _districtService.getAllDistrictForTableDropdown($("#address_cityId_").val());
      $("#address_districtId_").html(`<option value='' disabled selected> ${app.localize('SelectADistrict')} </option>`);

      res.forEach(city => {
        $("#address_districtId_").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    $("#address_districtId_").change(async function () {
      var res = await _quartersService.getAllQuartersForTableDropdown($("#address_districtId_").val());
      $("#address_quarterId_").html(`<option value='' disabled selected> ${app.localize('SelectAQuarter')} </option>`);

      res.forEach(city => {
        $("#address_quarterId_").append(`<option value="${city.id}">${city.displayName}</option>`);
      });
    });

    $("#address_quarterId_").change(async function(){
     _districtService.getRegionFromDistrict($("#address_districtId_").val())
          .then(x=>{
            _$funeralInformationForm.find("input[name='regionId']").val(x.region.id);
            _$funeralInformationForm.find("[name='region']").val(x.region.name);
          })
    });

    function createDocumentElement(id, name, path, type, files){
      let documentCount=$("[id*='document-']").length;
      var newDocument= $("#funeralDocumentTemplate").clone();
      newDocument.show();
      newDocument.attr("id", `document-${documentCount}`)

      newDocument.find(".documentId").val(id|| "");
      newDocument.find(".documentName").val(name|| "");

      newDocument.find(".documentPath").attr("name", `funeralDocuments[${documentCount}][path]`);
      newDocument.find(".documentPath").val(path || "");

      newDocument.find(".documentIndex").attr("name", `funeralDocuments[${documentCount}][index]`)
      newDocument.find(".documentIndex").val(documentCount);

      newDocument.find("select").attr("name", `funeralDocuments[${documentCount}][type]`)
      newDocument.find("select").val(app.localize("Enum_FuneralDocumentType_"+type) || "");

      if(files){
        var newFiles= new DataTransfer()
        newFiles.items.add(files);
        newDocument.find("input[type=file]").prop({
          files: newFiles.files
        })
      }

      newDocument.find("input[type=file]").attr("name", `funeralDocuments[${documentCount}][file]`);
      newDocument.find(".showDocument").attr("data-index", documentCount);
      newDocument.find(".deleteDocument").attr("data-index", documentCount);
      newDocument.find(".confirmDocument").attr("data-index", documentCount);

      if(!id){
        newDocument.find(".confirmDocument").show();
      }else{
        newDocument.find(".confirmDocument").hide();
        newDocument.find("select").attr("disabled",true);
      }

      $("#allDocuments").append(newDocument);
    }

    $("#funeralDocuments").change(function(){
      const files= [...this.files];

      files.forEach(selectedFile => {
        createDocumentElement(null, selectedFile.name,null,0,selectedFile);
      });

      $("#funeralDocuments").val(null);

    })

    $(document).on("click", ".showDocument",function(){
      var documentIndex= $(this).data("index");
      var path= $(`[name='funeralDocuments[${documentIndex}][path]']`).val();
      if(path!=undefined && path!=null && path!="" ){
        window.open(`https://ymticenaze.blob.core.windows.net${path}`,'_blank');
      }else{
        var file= $(`#document-${documentIndex} input[type=file]`)[0].files[0];
        window.open(URL.createObjectURL(file), '_blank');
      }


    })

    $(document).on("click", ".deleteDocument",async function(){
      var documentIndex= $(this).data("index");
      var documentNode= $(`#document-${documentIndex}`);

      abp.ui.block(documentNode);

      var hasId= documentNode.find(".documentId").val()!="";
      if(hasId){
        await _funeralDocumentsService.delete({ id: documentNode.find(".documentId").val()});
      }

      abp.ui.unblock(documentNode);

      $(`#document-${documentIndex}`).remove();
    })

    $(document).on("click", ".confirmDocument",async function(){
      var documentIndex= $(this).data("index");
      var documentNode= $(`#document-${documentIndex}`);
      abp.ui.block(documentNode);

      var hasId= documentNode.find(".documentId").val()!="";

      var file= documentNode.find(`input[type=file]`)[0].files[0];
      var name= documentNode.find(`select`).val();

      if(!hasId){
        var uploaded= await uploadDocument(file, name);
        var uploadedID= await _funeralDocumentsService.createAndGetId({
          funeralId: _stepperItems[0].idHolder.val(),
          guid: uploaded.guid,
          path: uploaded.path,
          type: name
        });
        documentNode.find(".documentId").val(uploadedID);
      }else{
        alert("update");
      }

      documentNode.find(".documentPath").val(uploaded);

      documentNode.find(".deleteDocument").show();
      documentNode.find(".confirmDocument").hide();
      abp.ui.unblock(documentNode);
    })


    async function uploadDocument(file, name){

      var formData = new FormData();
      formData.append('file', new File([file], name+"."+file.name.split('.')[1]));

      return new Promise((resolve, reject)=>{
        $.ajax({
          url: '/Portal/FuneralDocuments/UploadPathFile',
          type: 'POST',
          data: formData,
          processData: false,
          contentType: false,
        }).done((resp)=>{
          if (resp.success && resp.result.guid) {
            resolve(resp.result);
          } else {
            abp.message.error(resp.result.message);
          }
        });
      });
    }

    async function saveFuneralMains(){
      var stepEl= _stepperItems[0];

      if(!stepEl.form.valid()){
        return false;
      }

      var funeral = stepEl.form.serializeFormToObject();

      if(funeral.contact.id==null || funeral.contact.id==""){
        var contactDetail= funeral.contact.details[0];
        funeral.contact.details= [
          contactDetail
        ];

        funeral.contact.details[0].value= funeral.contact.details[0].value.replaceAll('_','').replaceAll('+','').replaceAll('(','').replaceAll(')','').replaceAll(' ','');
      }else{
        delete funeral.contact.details;
      }

      if(stepEl.isEdit==false){
        var createdFuneralID= await _funeralsService.createAndGetId(funeral);
        $("[name=id]").val(createdFuneralID);
      }else{
        await _funeralsService.createOrEdit(funeral);
      }

      return true;
    }

    async function getFuneralAddress(){
      var addressStepper= _stepperItems[1];

      var funeralAddress= await _funeralAddressService.getFuneralAddresForStep(_stepperItems[0].idHolder.val());


      addressStepper.idHolder.val(funeralAddress?.funeralAddres?.id);
      addressStepper.form.find("[name=region]").val(funeralAddress?.quarterName);

      $("#address_countryId_").empty();
      funeralAddress.countryList?.forEach(x=>{
        $('#address_countryId_').append(new Option(x.displayName,x.id,false,x.id==funeralAddress?.countryId))
      })

      $("#address_cityId_").empty();
      funeralAddress.cityList?.forEach(x=>{
        $('#address_cityId_').append(new Option(x.displayName,x.id,false,x.id==funeralAddress?.cityId))
      })

      $("#address_districtId_").empty();
      funeralAddress.districtList?.forEach(x=>{
        $('#address_districtId_').append(new Option(x.displayName,x.id,false,x.id==funeralAddress?.districtId))
      })

      $("#address_quarterId_").empty();
      funeralAddress.quarterList?.forEach(x=>{
        $('#address_quarterId_').append(new Option(x.displayName,x.id,false,x.id==funeralAddress?.funeralAddres?.quarterId))
      })

      $("#Funeral_Address_Address").val(funeralAddress?.funeralAddres?.address);
      $("#Funeral_Address_Description").val(funeralAddress?.funeralAddres?.description);

      return true;
    }

    async function saveFuneralAddress(){
      var stepEl= _stepperItems[1];

      if(!stepEl.form.valid()){
        return false;
      }

      var address= stepEl.form.serializeFormToObject();

      address.funeralId = _stepperItems[0].idHolder.val();

      if(stepEl.isEdit==false){
        var createdFuneralAddressID= await _funeralAddressService.createAndGetId(address);
        $("[name='address[id]']").val(createdFuneralAddressID);
      }else{
        await _funeralAddressService.createOrEdit(address);
      }

      return true;
    }

    async function getFuneralDocuments(){
      $("#allDocuments").html("");
      var documents= await _funeralDocumentsService.getFuneralDocumentsForStep(_stepperItems[0].idHolder.val());
      documents.forEach(document=>{
        createDocumentElement(document.id, document.path, document.path, document.type, null);
      });
      return true;
    }

    async function saveFuneralDocuments(){
      var stepEl= _stepperItems[2];
      var funeralDocuments= stepEl.form.serializeFormToObject();

      var formattedDocuments=[];

      if(funeralDocuments?.funeralDocuments!=null){
        Object.keys(funeralDocuments?.funeralDocuments).forEach((item,index)=>{
          formattedDocuments[index]= funeralDocuments?.funeralDocuments[item];
        });
      }

      if(formattedDocuments.find(x=>x.path!="" && x.type=="Death")==null){
        abp.message.error(app.localize("DeathDocumentRequired"));
        return false;
      }

      return true;
    }

    async function getFuneralFlight(){
      var funeralFlight= await _funeralFlightService.getFuneralFlightForStep(_stepperItems[0].idHolder.val());
      if(funeralFlight==null){
        return;
      }

      $("#flight_airlineCompanyId_").empty();
      funeralFlight.airlineCompanyList?.forEach(x=>{
        $('#flight_airlineCompanyId_').append(new Option(x.displayName,x.id,false,x.id==funeralFlight.funeralFlight?.airlineCompanyId))
      })

      $("#flight_liftOffAirportId_").empty();
      $("#flight_langingAirportId_").empty();
      funeralFlight.airportList?.forEach(x=>{
        $('#flight_liftOffAirportId_').append(new Option(x.displayName,x.id,false,x.id==funeralFlight.funeralFlight?.liftOffAirportId))
        $('#flight_langingAirportId_').append(new Option(x.displayName,x.id,false,x.id==funeralFlight.funeralFlight?.langingAirportId))
      })



      $("#FuneralFlight_LangingAirportId").val(funeralFlight?.funeralFlight?.id);
      $("#FuneralFlight_Code").val(funeralFlight?.funeralFlight?.code);
      $("#flight_LandingDate").val(funeralFlight?.funeralFlight?.landingDate)
      $("#flight_LiftOffDate").val(funeralFlight?.funeralFlight?.liftOffDate)


      return true;
    }

    async function saveFuneralFlight(){
      var stepEl= _stepperItems[3];

      if(!stepEl.form.valid()){
        return false;
      }

      var flight= stepEl.form.serializeFormToObject();
      flight.funeralId = _stepperItems[0].idHolder.val();

      if(stepEl.isEdit==false){
        var createdFuneralFlightId= await _funeralFlightService.createAndGetId(flight);
        stepEl.idHolder.val(createdFuneralFlightId);
      }else{
        await _funeralFlightService.createOrEdit(flight);
      }

      return true;
    }

    async function save(successCallback) {

      abp.ui.setBusy();
      _funeralsService
        .checkForOperation(_stepperItems[0].idHolder.val())
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));

          if (typeof successCallback === 'function') {
            successCallback();
          }
        })
        .always(function () {
          abp.ui.clearBusy();
        });
    }

    function clearForm() {
      _$funeralInformationForm[0].reset();
    }

    $('#saveBtn').click(function () {
      save(function () {
        window.location = '/Portal/Funerals';
      });
    });

    $('#saveAsTemplate').click(function(){
      window.location = '/Portal/Funerals';
    })

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
