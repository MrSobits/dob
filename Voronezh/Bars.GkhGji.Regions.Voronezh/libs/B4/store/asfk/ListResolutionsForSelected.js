Ext.define('B4.store.asfk.ListResolutionsForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Resolution'],
    autoLoad: false,
    storeId: 'listResolutionsForSelectedStore',
    model: 'B4.model.Resolution'
});