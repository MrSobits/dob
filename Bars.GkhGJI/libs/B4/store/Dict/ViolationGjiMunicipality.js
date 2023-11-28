Ext.define('B4.store.dict.ViolationGjiMunicipality', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ViolationGjiMunicipality'],
    autoLoad: false,
    storeId: 'violationGjimunicipalityStore',
    model: 'B4.model.dict.ViolationGjiMunicipality'
    //proxy: {
    //    type: 'b4proxy',
    //    controllerName: 'ViolationGji',
    //    listAction: 'ListMunicipality',
    //    timeout: 300000
    //}
});