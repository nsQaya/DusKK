(function ($) {
  app.modals.CreateOrEditUserDetailModal = function () {
    var _userDetailsService = abp.services.app.userDetails;

    var _modalManager;
    var _$userDetailInformationForm = null;

    var _UserDetailuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/UserDetails/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/UserDetails/_UserDetailUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });
    var _UserDetailcontactLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Portal/UserDetails/ContactLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/UserDetails/_UserDetailContactLookupTableModal.js',
      modalClass: 'ContactLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
      modal.find('.date-picker').daterangepicker({
        singleDatePicker: true,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$userDetailInformationForm = _modalManager.getModal().find('form[name=UserDetailInformationsForm]');
      _$userDetailInformationForm.validate();
    };

    $('#OpenUserLookupTableButton').click(function () {
      var userDetail = _$userDetailInformationForm.serializeFormToObject();

      _UserDetailuserLookupTableModal.open(
        { id: userDetail.userId, displayName: userDetail.userName },
        function (data) {
          _$userDetailInformationForm.find('input[name=userName]').val(data.displayName);
          _$userDetailInformationForm.find('input[name=userId]').val(data.id);
        }
      );
    });

    $('#ClearUserNameButton').click(function () {
      _$userDetailInformationForm.find('input[name=userName]').val('');
      _$userDetailInformationForm.find('input[name=userId]').val('');
    });

    $('#OpenContactLookupTableButton').click(function () {
      var userDetail = _$userDetailInformationForm.serializeFormToObject();

      _UserDetailcontactLookupTableModal.open(
        { id: userDetail.contactId, displayName: userDetail.contactDisplayProperty },
        function (data) {
          _$userDetailInformationForm.find('input[name=contactDisplayProperty]').val(data.displayName);
          _$userDetailInformationForm.find('input[name=contactId]').val(data.id);
        }
      );
    });

    $('#ClearContactDisplayPropertyButton').click(function () {
      _$userDetailInformationForm.find('input[name=contactDisplayProperty]').val('');
      _$userDetailInformationForm.find('input[name=contactId]').val('');
    });

    this.save = function () {
      if (!_$userDetailInformationForm.valid()) {
        return;
      }
      if ($('#UserDetail_UserId').prop('required') && $('#UserDetail_UserId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }
      if ($('#UserDetail_ContactId').prop('required') && $('#UserDetail_ContactId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Contact')));
        return;
      }

      var userDetail = _$userDetailInformationForm.serializeFormToObject();

      _modalManager.setBusy(true);
      _userDetailsService
        .createOrEdit(userDetail)
        .done(function () {
          abp.notify.info(app.localize('SavedSuccessfully'));
          _modalManager.close();
          abp.event.trigger('app.createOrEditUserDetailModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
