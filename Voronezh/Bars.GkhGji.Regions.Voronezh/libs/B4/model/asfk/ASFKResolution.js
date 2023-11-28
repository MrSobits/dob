Ext.define('B4.model.asfk.ASFKResolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ASFKResolution'
    },
    fields: [
        { name: 'Id' },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber'},
        { name: 'ComissionName'},
        { name: 'InLawDate'},
        { name: 'Fio' },
        { name: 'ContragentName' },
        { name: 'PenaltyAmount' }
    ]
});