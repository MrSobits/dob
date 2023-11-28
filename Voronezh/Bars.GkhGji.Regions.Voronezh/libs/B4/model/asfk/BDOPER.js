Ext.define('B4.model.asfk.BDOPER', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BDOPER'
    },
    fields: [
        { name: 'Id' },
        { name: 'IsPayFineAdded' },
        { name: 'GUID'},
        { name: 'Sum'},
        { name: 'InnPay'},
        { name: 'KppPay' },
        { name: 'NamePay' },
        { name: 'Purpose' }
    ]
});