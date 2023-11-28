Ext.define('B4.model.comission.ComissionMeeting', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ComissionMeeting'
    },
    fields: [
        { name: 'Id' },
        { name: 'State'},
        { name: 'ZonalInspection'},
        { name: 'ComissionName'},
        { name: 'CommissionDate' },
        { name: 'CommissionNumber' },
        { name: 'Description' }, 
        { name: 'TimeEnd' }, 
        { name: 'TimeStart' }      
    ]
});