Ext.define('B4.store.asfk.ASFK', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.asfk.ASFK'],
    storeId: 'asfkStore',
    model: 'B4.model.asfk.ASFK'
});