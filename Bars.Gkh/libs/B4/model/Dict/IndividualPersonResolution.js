Ext.define('B4.model.dict.IndividualPersonResolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Resolution'
    },
    fields: [
        { name: 'Id' },
        { name: 'DocumentDate' },
        { name: 'InLawDate' },
        { name: 'DocumentNum' },
        { name: 'DocumentNumber' },
        { name: 'Violation' },
        { name: 'PenaltyAmount' },
        { name: 'Paided' },
        { name: 'PayDay' }
    ]
});