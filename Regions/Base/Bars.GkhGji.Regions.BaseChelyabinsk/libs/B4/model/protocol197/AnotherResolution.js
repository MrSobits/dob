Ext.define('B4.model.protocol197.AnotherResolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol197AnotherResolution'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol197', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null },
        { name: 'DocumentGji', defaultValue: null },
        { name: 'Description' },
        { name: 'DocumentNumber' },
        { name: 'Sanction' },        
        { name: 'DocumentDate' },
        { name: 'TypeDocumentGji' }
    ]
});