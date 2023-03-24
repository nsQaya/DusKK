(function ($) {
  app.modals.CreateOrEditFuneralFlightModal = function () {
    var _funeralFlightsService = abp.services.app.funeralFlights;

    var _modalManager;
    var _$funeralFlightInformationForm = null;

    var _FuneralFlightfuneralLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralFlights/FuneralLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/Portal/Views/FuneralFlights/_FuneralFlightFuneralLookupTableModal.js',
      modalClass: 'FuneralLookupTableModal',
    });
    var _FuneralFlightairlineCompanyLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralFlights/AirlineCompanyLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/FuneralFlights/_FuneralFlightAirlineCompanyLookupTableModal.js',
      modalClass: 'AirlineCompanyLookupTableModal',
    });
    var _FuneralFlightairportLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/FuneralFlights/AirportLookupTableModal',
      scriptUrl:
        abp.appPath + 'view-resources/Areas/Portal/Views/FuneralFlights/_FuneralFlightAirportLookupTableModal.js',
      modalClass: 'AirportLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$funeralFlightInformationForm = _modalManager.getModal().find('form[name=FuneralFlightInformationsForm]');
      _$funeralFlightInformationForm.validate();
    };

    $('#OpenFuneralLookupTableButton').click(function () {
      var funeralFlight = _$funeralFlightInformationForm.serializeFormToObject();

      _FuneralFlightfuneralLookupTableModal.open(
        { id: funeralFlight.funeralId, displayName: funeralFlight.funeralName },
        function (data) {
          _$funeralFlightInformationForm.find('input[name=funeralName]').val(data.displayName);
          _$funeralFlightInformationForm.find('input[name=funeralId]').val(data.id);
        }
      );
    });

    $('#ClearFuneralNameButton').click(function () {
      _$funeralFlightInformationForm.find('input[name=funeralName]').val('');
      _$funeralFlightInformationForm.find('input[name=funeralId]').val('');
    });

    $('#OpenAirlineCompanyLookupTableButton').click(function () {
      var funeralFlight = _$funeralFlightInformationForm.serializeFormToObject();

      _FuneralFlightairlineCompanyLookupTableModal.open(
        { id: funeralFlight.airlineCompanyId, displayName: funeralFlight.airlineCompanyCode },
        function (data) {
          _$funeralFlightInformationForm.find('input[name=airlineCompanyCode]').val(data.displayName);
          _$funeralFlightInformationForm.find('input[name=airlineCompanyId]').val(data.id);
        }
      );
    });

    $('#ClearAirlineCompanyCodeButton').click(function () {
      _$funeralFlightInformationForm.find('input[name=airlineCompanyCode]').val('');
      _$funeralFlightInformationForm.find('input[name=airlineCompanyId]').val('');
    });

    $('#OpenAirportLookupTableButton').click(function () {
      var funeralFlight = _$funeralFlightInformationForm.serializeFormToObject();

      _FuneralFlightairportLookupTableModal.open(
        { id: funeralFlight.liftOffAirportId, displayName: funeralFlight.airportName },
        function (data) {
          _$funeralFlightInformationForm.find('input[name=airportName]').val(data.displayName);
          _$funeralFlightInformationForm.find('input[name=liftOffAirportId]').val(data.id);
        }
      );
    });

    $('#ClearAirportNameButton').click(function () {
      _$funeralFlightInformationForm.find('input[name=airportName]').val('');
      _$funeralFlightInformationForm.find('input[name=liftOffAirportId]').val('');
    });

    $('#OpenAirport2LookupTableButton').click(function () {
      var funeralFlight = _$funeralFlightInformationForm.serializeFormToObject();

      _FuneralFlightairportLookupTableModal.open(
        { id: funeralFlight.langingAirportId, displayName: funeralFlight.airportName2 },
        function (data) {
          _$funeralFlightInformationForm.find('input[name=airportName2]').val(data.displayName);
          _$funeralFlightInformationForm.find('input[name=langingAirportId]').val(data.id);
        }
      );
    });

    $('#ClearAirportName2Button').click(function () {
      _$funeralFlightInformationForm.find('input[name=airportName2]').val('');
      _$funeralFlightInformationForm.find('input[name=langingAirportId]').val('');
    });

    this.save = function () {
      if (!_$funeralFlightInformationForm.valid()) {
        return;
      }
      if ($('#FuneralFlight_FuneralId').prop('required') && $('#FuneralFlight_FuneralId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Funeral')));
        return;
      }
      if ($('#FuneralFlight_AirlineCompanyId').prop('required') && $('#FuneralFlight_AirlineCompanyId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('AirlineCompany')));
        return;
      }
      if ($('#FuneralFlight_LiftOffAirportId').prop('required') && $('#FuneralFlight_LiftOffAirportId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Airport')));
        return;
      }
      if ($('#FuneralFlight_LangingAirportId').prop('required') && $('#FuneralFlight_LangingAirportId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Airport')));
        return;
      }

      var funeralFlight = _$funeralFlightInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _funeralFlightsService
        .createOrEdit(funeralFlight)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditFuneralFlightModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
