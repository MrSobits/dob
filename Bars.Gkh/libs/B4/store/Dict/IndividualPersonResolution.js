Ext.define('B4.store.dict.IndividualPersonResolution', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.IndividualPersonResolution'],
    autoLoad: false,
    storeId: 'individualpersonresolutionStore',
    model: 'B4.model.dict.IndividualPersonResolution',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Resolution',
        listAction: 'ListIndividualPerson',
        timeout: 300000
    }
});