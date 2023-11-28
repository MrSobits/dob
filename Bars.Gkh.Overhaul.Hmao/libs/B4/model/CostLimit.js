Ext.define('B4.model.CostLimit', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CostLimit'
    },
    fields: [
        { name: 'Id' },
        { name: 'Work' },
        { name: 'Cost' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'FloorStart' },
        { name: 'FloorEnd' },
        { name: 'Municipality' },
    ]
});