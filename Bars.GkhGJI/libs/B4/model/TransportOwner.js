Ext.define('B4.model.TransportOwner', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'Owner'
    },
    fields: [
        { name: 'NameTransport' },
        { name: 'NamberTransport' },
        { name: 'RegistrationNamberTransport' },
        { name: 'SeriesTransport' },
        { name: 'RegNamberTransport' },
        { name: 'ContragentName' },
        { name: 'TypeViolator' },
        { name: 'DataOwnerStart' },
        { name: 'DataOwnerEdit'},
        { name: 'IndividualPerson' },
        { name: 'Contragent' },
        { name: 'ContragentContact' }       
    ]
});