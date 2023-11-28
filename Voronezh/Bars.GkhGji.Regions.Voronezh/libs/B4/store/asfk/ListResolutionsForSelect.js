Ext.define('B4.store.asfk.ListResolutionsForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.asfk.ASFKResolution'],
    autoLoad: false,
    storeId: 'listResolutionsForSelectStore',
    model: 'B4.model.asfk.ASFKResolution',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BDOPER',
        listAction: 'GetListResolutionsForSelect'
    }
});