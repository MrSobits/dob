Ext.define('B4.model.dict.ArticleLawGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ArticleLawGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'OMS' },
        { name: 'OmsRegion' },
        { name: 'NameOMS' },
        { name: 'Part' },
        { name: 'KBK' },
        { name: 'Article' },
        { name: 'Bank' }
    ]
});