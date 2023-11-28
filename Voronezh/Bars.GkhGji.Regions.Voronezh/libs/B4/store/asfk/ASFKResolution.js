Ext.define('B4.store.asfk.ASFKResolution', {
    extend: 'B4.base.Store',
    requires: ['B4.model.asfk.ASFKResolution'],
    autoLoad: false,
    storeId: 'asfkResolutionStore',
    model: 'B4.model.asfk.ASFKResolution',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BDOPER',
        listAction: 'GetResolution'
    }
});