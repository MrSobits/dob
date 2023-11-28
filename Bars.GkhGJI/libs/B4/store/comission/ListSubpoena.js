Ext.define('B4.store.comission.ListSubpoena', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Subpoena'],
    autoLoad: false,
    model: 'B4.model.dict.Subpoena',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Subpoena',
        listAction: 'ComissionListSubpoena'
    }
});