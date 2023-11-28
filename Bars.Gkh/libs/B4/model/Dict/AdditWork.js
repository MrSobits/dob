Ext.define('B4.model.dict.AdditWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdditWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Percentage' },
        { name: 'Queue' },        
        { name: 'Description' },
        { name: 'Work' }
    ]
});