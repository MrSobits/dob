Ext.define('B4.store.dict.IndividualPersonToResolution', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.IndividualPersonToResolution'],
    autoLoad: false,
    storeId: 'individualpersontoresolutionStore',
    model: 'B4.model.dict.IndividualPersonToResolution',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Resolution',
        listAction: 'ListResolutionIndividualPerson',
        timeout: 300000
    }
});