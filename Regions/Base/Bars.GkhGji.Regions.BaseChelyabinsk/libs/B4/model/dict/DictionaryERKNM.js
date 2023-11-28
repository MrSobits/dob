Ext.define('B4.model.dict.DictionaryERKNM', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DictionaryERKNM'
    },
    fields: [
        { name: 'DictionaryERKNMGuid', },
        { name: 'Name' },
        { name: 'Type' },
        { name: 'Description' },
        { name: 'Order' },
        { name: 'Required' },
        { name: 'DateLastUpdate' },
        { name: 'EntityName' },
        { name: 'EntityId' },
    ]
});