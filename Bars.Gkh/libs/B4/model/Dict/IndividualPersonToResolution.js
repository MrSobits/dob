Ext.define('B4.model.dict.IndividualPersonToResolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Resolution'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Fio' },
        { name: 'INN' },
        { name: 'Job' },
        { name: 'PassportIssued' },
        { name: 'PassportNumber' },
        { name: 'PassportSeries' },
        { name: 'PlaceResidence' },
        { name: 'BirthPlace' },
        { name: 'DateBirth' },
        { name: 'DateIssue' },
        { name: 'ActuallyResidence' },
        { name: 'DepartmentCode' },
        { name: 'FamilyStatus' }
    ]
});