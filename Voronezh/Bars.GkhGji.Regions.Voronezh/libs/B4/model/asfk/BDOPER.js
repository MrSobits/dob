Ext.define('B4.model.asfk.BDOPER', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BDOPER'
    },
    fields: [
        { name: 'Id' },
        { name: 'ASFK' },
        { name: 'IsPayFineAdded' },
        { name: 'GUID' },
        { name: 'Sum' },
        { name: 'InnPay' },
        { name: 'KppPay' },
        { name: 'NamePay' },
        { name: 'Kbk' },
        { name: 'KodDocAdb' },
        { name: 'Purpose' }
    ]
});