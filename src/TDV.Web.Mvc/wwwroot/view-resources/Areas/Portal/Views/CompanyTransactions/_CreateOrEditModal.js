(function ($) {
  app.modals.CreateOrEditCompanyTransactionModal = function () {
    var _companyTransactionsService = abp.services.app.companyTransactions;

    var _modalManager;
    var _$companyTransactionInformationForm = null;

    var _CompanyTransactioncompanyLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyTransactions/CompanyLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/CompanyTransactions/_CompanyTransactionCompanyLookupTableModal.js',
      modalClass: 'CompanyLookupTableModal',
    });
    var _CompanyTransactionfuneralLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyTransactions/FuneralLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/CompanyTransactions/_CompanyTransactionFuneralLookupTableModal.js',
      modalClass: 'FuneralLookupTableModal',
    });
    var _CompanyTransactiondataListLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyTransactions/DataListLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/CompanyTransactions/_CompanyTransactionDataListLookupTableModal.js',
      modalClass: 'DataListLookupTableModal',
    });
    var _CompanyTransactioncurrencyLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/CompanyTransactions/CurrencyLookupTableModal',
      scriptUrl:
        abp.appPath +
        'view-resources/Areas/Portal/Views/CompanyTransactions/_CompanyTransactionCurrencyLookupTableModal.js',
      modalClass: 'CurrencyLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$companyTransactionInformationForm = _modalManager
        .getModal()
        .find('form[name=CompanyTransactionInformationsForm]');
      _$companyTransactionInformationForm.validate();
    };

    $('#OpenCompanyLookupTableButton').click(function () {
      var companyTransaction = _$companyTransactionInformationForm.serializeFormToObject();

      _CompanyTransactioncompanyLookupTableModal.open(
        { id: companyTransaction.companyId, displayName: companyTransaction.companyTaxAdministration },
        function (data) {
          _$companyTransactionInformationForm.find('input[name=companyTaxAdministration]').val(data.displayName);
          _$companyTransactionInformationForm.find('input[name=companyId]').val(data.id);
        }
      );
    });

    $('#ClearCompanyTaxAdministrationButton').click(function () {
      _$companyTransactionInformationForm.find('input[name=companyTaxAdministration]').val('');
      _$companyTransactionInformationForm.find('input[name=companyId]').val('');
    });

    $('#OpenFuneralLookupTableButton').click(function () {
      var companyTransaction = _$companyTransactionInformationForm.serializeFormToObject();

      _CompanyTransactionfuneralLookupTableModal.open(
        { id: companyTransaction.funeralId, displayName: companyTransaction.funeralDisplayProperty },
        function (data) {
          _$companyTransactionInformationForm.find('input[name=funeralDisplayProperty]').val(data.displayName);
          _$companyTransactionInformationForm.find('input[name=funeralId]').val(data.id);
        }
      );
    });

    $('#ClearFuneralDisplayPropertyButton').click(function () {
      _$companyTransactionInformationForm.find('input[name=funeralDisplayProperty]').val('');
      _$companyTransactionInformationForm.find('input[name=funeralId]').val('');
    });

    $('#OpenDataListLookupTableButton').click(function () {
      var companyTransaction = _$companyTransactionInformationForm.serializeFormToObject();

      _CompanyTransactiondataListLookupTableModal.open(
        { id: companyTransaction.type, displayName: companyTransaction.dataListValue },
        function (data) {
          _$companyTransactionInformationForm.find('input[name=dataListValue]').val(data.displayName);
          _$companyTransactionInformationForm.find('input[name=type]').val(data.id);
        }
      );
    });

    $('#ClearDataListValueButton').click(function () {
      _$companyTransactionInformationForm.find('input[name=dataListValue]').val('');
      _$companyTransactionInformationForm.find('input[name=type]').val('');
    });

    $('#OpenCurrencyLookupTableButton').click(function () {
      var companyTransaction = _$companyTransactionInformationForm.serializeFormToObject();

      _CompanyTransactioncurrencyLookupTableModal.open(
        { id: companyTransaction.currencyId, displayName: companyTransaction.currencyCode },
        function (data) {
          _$companyTransactionInformationForm.find('input[name=currencyCode]').val(data.displayName);
          _$companyTransactionInformationForm.find('input[name=currencyId]').val(data.id);
        }
      );
    });

    $('#ClearCurrencyCodeButton').click(function () {
      _$companyTransactionInformationForm.find('input[name=currencyCode]').val('');
      _$companyTransactionInformationForm.find('input[name=currencyId]').val('');
    });

    $('#OpenDataList2LookupTableButton').click(function () {
      var companyTransaction = _$companyTransactionInformationForm.serializeFormToObject();

      _CompanyTransactiondataListLookupTableModal.open(
        { id: companyTransaction.unitType, displayName: companyTransaction.dataListValue2 },
        function (data) {
          _$companyTransactionInformationForm.find('input[name=dataListValue2]').val(data.displayName);
          _$companyTransactionInformationForm.find('input[name=unitType]').val(data.id);
        }
      );
    });

    $('#ClearDataListValue2Button').click(function () {
      _$companyTransactionInformationForm.find('input[name=dataListValue2]').val('');
      _$companyTransactionInformationForm.find('input[name=unitType]').val('');
    });

    this.save = function () {
      if (!_$companyTransactionInformationForm.valid()) {
        return;
      }
      if ($('#CompanyTransaction_CompanyId').prop('required') && $('#CompanyTransaction_CompanyId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Company')));
        return;
      }
      if ($('#CompanyTransaction_FuneralId').prop('required') && $('#CompanyTransaction_FuneralId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Funeral')));
        return;
      }
      if ($('#CompanyTransaction_Type').prop('required') && $('#CompanyTransaction_Type').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('DataList')));
        return;
      }
      if ($('#CompanyTransaction_CurrencyId').prop('required') && $('#CompanyTransaction_CurrencyId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Currency')));
        return;
      }
      if ($('#CompanyTransaction_UnitType').prop('required') && $('#CompanyTransaction_UnitType').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('DataList')));
        return;
      }

      var companyTransaction = _$companyTransactionInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _companyTransactionsService
        .createOrEdit(companyTransaction)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditCompanyTransactionModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
