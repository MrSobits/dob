Ext.define('B4.model.comission.ComissionMeetingDocument', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ComissionMeetingDocument'
    },
    fields: [
        { name: 'Id' },
        { name: 'State'},
        { name: 'ZonalInspection'},
        { name: 'ComissionName' },
        { name: 'ZonalInspection' },
        { name: 'CommissionDate' },
        { name: 'CommissionNumber' },
        { name: 'ComissionDocumentDecision' },
        { name: 'Description' }, 
        { name: 'TimeEnd' }, 
        { name: 'TimeStart' }      
    ]
});