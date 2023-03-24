(function() {
    $(function() {
        var _$vehiclesTable = $('#VehiclesTable');
        var _vehiclesService = abp.services.app.vehicles;

        var _vehiclesPermissions = {
            create: abp.auth.hasPermission('Pages.Vehicles.Create'),
            edit: abp.auth.hasPermission('Pages.Vehicles.Edit'),
            delete: abp.auth.hasPermission('Pages.Vehicles.Delete'),
        };

        var _vehiclesCreateOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Portal/Vehicles/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/Vehicles/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditVehicleModal',
        });

        var vehiclesDataTable = _$vehiclesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _vehiclesService.getAll,
                inputFilter: function() {
                    return {
                        organizationUnitId: $('[name=organizationUnitId]').val(),
                    };
                },
            },
            columnDefs: [{
                    className: 'control responsive',
                    orderable: false,
                    render: function() {
                        return '';
                    },
                    targets: 0,
                },
                {
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [{
                                text: app.localize('Edit'),
                                visible: function() {
                                    return _vehiclesPermissions.edit;
                                },
                                action: function(data) {
                                    _vehiclesCreateOrEditModal.open({
                                        id: data.record.vehicle.id
                                    });
                                },
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function() {
                                    return _vehiclesPermissions.delete;
                                },
                                action: function(data) {
                                    abp.message.confirm('', app.localize('AreYouSure'), function(isConfirmed) {
                                        if (isConfirmed) {
                                            _vehiclesService
                                                .delete({
                                                    id: data.record.vehicle.id,
                                                })
                                                .done(function() {
                                                    vehiclesDataTable.ajax.reload();
                                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                                });
                                        }
                                    });
                                },
                            },
                        ],
                    },
                },
                {
                    targets: 2,
                    data: 'vehicle.plate',
                    name: 'plate',
                },
                {
                    targets: 3,
                    data: 'vehicle.description',
                    name: 'description',
                },
                {
                    targets: 4,
                    data: 'vehicle.endExaminationDate',
                    name: 'endExaminationDate',
                    render: function(endExaminationDate) {
                        if (endExaminationDate) {
                            return moment(endExaminationDate).format('L');
                        }
                        return '';
                    },
                },
                {
                    targets: 5,
                    data: 'vehicle.endInsuranceDate',
                    name: 'endInsuranceDate',
                    render: function(endInsuranceDate) {
                        if (endInsuranceDate) {
                            return moment(endInsuranceDate).format('L');
                        }
                        return '';
                    },
                },
                {
                    targets: 6,
                    data: 'vehicle.endGuarantyDate',
                    name: 'endGuarantyDate',
                    render: function(endGuarantyDate) {
                        if (endGuarantyDate) {
                            return moment(endGuarantyDate).format('L');
                        }
                        return '';
                    },
                },
                {
                    targets: 7,
                    data: 'vehicle.capactiy',
                    name: 'capactiy',
                },
                {
                    targets: 8,
                    data: 'vehicle.year',
                    name: 'year',
                },
                {
                    targets: 9,
                    data: 'vehicle.brand',
                    name: 'brand',
                }
            ],
        });


        $("#addNewVehicle").click(function() {
            _vehiclesCreateOrEditModal.open({
                companyId: $("[name=companyId]").val()
            });
        });
        abp.event.on('app.createOrEditVehicleModalSaved', function() {
            vehiclesDataTable.ajax.reload();
        });


        /* Contacts*/
        var _$companyContactsTable = $('#CompanyContactsTable');
        var _companyContactsService = abp.services.app.companyContacts;

        var _contactCreateOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Portal/CompanyContacts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Portal/Views/CompanyContacts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCompanyContactModal',
        });

        var _contactPermissions = {
            create: abp.auth.hasPermission('Pages.CompanyContacts.Create'),
            edit: abp.auth.hasPermission('Pages.CompanyContacts.Edit'),
            delete: abp.auth.hasPermission('Pages.CompanyContacts.Delete'),
        };

        var contactsDataTable = _$companyContactsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _companyContactsService.getAll,
                inputFilter: function() {
                    return {
                        organizationUnitId: $('[name=organizationUnitId]').val(),
                    };
                },
            },
            columnDefs: [{
                    className: 'control responsive hidden',
                    orderable: false,
                    render: function() {
                        return '';
                    },
                    targets: 0,
                },
                {
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [{
                                text: app.localize('Edit'),
                                visible: function() {
                                    return _contactPermissions.edit;
                                },
                                action: function(data) {
                                    _contactCreateOrEditModal.open({
                                        id: data.record.companyContact.id
                                    });
                                },
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function() {
                                    return _contactPermissions.delete;
                                },
                                action: function(data) {
                                    abp.message.confirm('', app.localize('AreYouSure'), function(isConfirmed) {
                                        if (isConfirmed) {
                                            _companyContactsService
                                                .delete({
                                                    id: data.record.companyContact.id,
                                                })
                                                .done(function() {
                                                    contactsDataTable.ajax.reload();
                                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                                });
                                        }
                                    });
                                },
                            },
                        ],
                    },
                },
                {
                    targets: 2,
                    data: 'companyContact.title',
                    name: 'title',
                },
                {
                    targets: 3,
                    data: 'contactName',
                    name: 'contactFk.name',
                },
            ],
        });

        $("#addNewContact").click(function() {
            _contactCreateOrEditModal.open({
                companyId: $("[name=companyId]").val()
            });
        });
        abp.event.on('app.createOrEditCompanyContactModalSaved', function() {
            contactsDataTable.ajax.reload();
        });


        var _organizationUnitService = abp.services.app.organizationUnit;
        var _$companyUsersTable = $('#CompanyUsers');

        var _usersPermissions = {
            manageOrganizationTree: abp.auth.hasPermission('Pages.Administration.OrganizationUnits.ManageOrganizationTree'),
            manageMembers: abp.auth.hasPermission('Pages.Administration.OrganizationUnits.ManageMembers'),
            manageRoles: abp.auth.hasPermission('Pages.Administration.OrganizationUnits.ManageRoles'),
          };

        var usersDataTable= _$companyUsersTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            //deferLoading: 1, //prevents table for ajax request on initialize
            listAction: {
              ajaxFunction: _organizationUnitService.getOrganizationUnitUsers,
              inputFilter: function () {
                return { id: $("[name=organizationUnitId]").val() };
              },
            },
            columnDefs: [
                {
                    className: 'control responsive hidden',
                    orderable: false,
                    render: function() {
                        return '';
                    },
                    targets: 0,
                },
              {
                targets: 1,
                data: null,
                orderable: false,
                defaultContent: '',
                className: 'text-center',
                rowAction: {
                  targets: 1,
                  data: null,
                  orderable: false,
                  defaultContent: '',
                  element: $('<button/>')
                    .addClass('btn btn-icon btn-bg-light btn-active-color-danger btn-sm')
                    .attr('title', app.localize('Delete'))
                    .append($('<i/>').addClass('fa fa-times'))
                    .click(function () {
                      var record = $(this).data();
                      members.remove(record);
                    }),
                  visible: function () {
                    return _usersPermissions.manageMembers;
                  },
                },
              },
              {
                width: 120,
                targets: 2,
                data: 'userName',
              },
              {
                targets: 3,
                data: 'addedTime',
                render: function (addedTime) {
                  return moment(addedTime).format('L');
                },
              },
            ],
          });
    });
})()