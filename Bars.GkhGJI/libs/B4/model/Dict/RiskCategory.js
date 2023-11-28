Ext.define('B4.model.dict.RiskCategory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RiskCategory'
    },

    fields: [
        { name: 'Name' },
        { name: 'RiskFrom'},
        { name: 'RiskTo' },
        { name: 'Code' },
    ]
});