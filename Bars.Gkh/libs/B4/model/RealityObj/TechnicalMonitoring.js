Ext.define('B4.model.realityobj.TechnicalMonitoring', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectTechnicalMonitoring'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'MonitoringTypeDict', defaultValue: null },
        { name: 'Name' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'File' },
        { name: 'UsedInExport', defaultValue: 10 }
    ]
});