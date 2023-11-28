Ext.define('B4.store.comission.ListProtocols', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ProtocolGji'],
    autoLoad: false,
    model: 'B4.model.ProtocolGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListProtocolsInCommission'
    }
});