Ext.define('B4.store.dict.ViolationGjiForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ViolationGji'],
    autoLoad: false,
    storeId: 'violationGjiForSelectStore',
    model: 'B4.model.dict.ViolationGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalViol',
        listAction: 'ListViolationforMunicipality',
        timeout: 300000
    }
});