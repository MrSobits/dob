Ext.define('B4.model.dict.Subpoena', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Subpoena'
    },
    idProperty: 'Id',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'DateOfProceedings' },
        { name: 'HourOfProceedings' },
        { name: 'ProceedingCopyNum' },
        { name: 'ProceedingsPlace' },
        { name: 'Comission'},
        { name: 'ComissionName' }
    ]
});