Ext.define('B4.store.dict.IndividualPerson', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.IndividualPerson'],
    autoLoad: false,
    storeId: 'individualpersonStore',
    model: 'B4.model.dict.IndividualPerson'
});