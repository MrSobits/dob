Ext.define('B4.model.dict.InspectorZonalInspSubscription', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectorZonalInspSubscription'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Inspector' },
        { name: 'ZonalInspName' },
        { name: 'ZonalInspAddress' }
    ]
});