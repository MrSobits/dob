Ext.define('B4.model.Resolution', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'Resolution'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'Executant', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'Sanction', defaultValue: null },
        { name: 'Official', defaultValue: null },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonInfo' },
        { name: 'DeliveryDate' },
        { name: 'TypeInitiativeOrg', defaultValue: 10 },
        { name: 'SectorNumber' },
        { name: 'PenaltyAmount' },
        { name: 'ParentDocumentsList' },
        { name: 'TypeDocumentGji', defaultValue: 70 },
        { name: 'Paided', defaultValue: 30 },
        { name: 'DateTransferSsp' },
        { name: 'DocumentNumSsp' },
        { name: 'ContragentName' },
        { name: 'TypeExecutant' },
        { name: 'MunicipalityNames' },
        { name: 'DocumentDate' },
        { name: 'OfficialName' },
        { name: 'SumPays' },
        { name: 'Description' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'TypeBase', defaultValue: null },
        { name: 'PhysPersonAddress' },
        { name: 'PhysPersonJob' },
        { name: 'PhysPersonPosition' },
        { name: 'PhysPersonBirthdayAndPlace' },
        { name: 'PhysPersonSalary' },
        { name: 'PhysPersonDocument' },
        { name: 'PhysPersonMaritalStatus' },
        { name: 'Description' },
        { name: 'BecameLegal', type: 'boolean', defaultValue: false }
    ]
});