Ext.define('B4.store.comission.ListResolutionDefinitions', {
    extend: 'B4.base.Store',
    requires: ['B4.model.resolution.Definition'],
    autoLoad: false,
    model: 'B4.model.resolution.Definition',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListResolutionDefinitionsInCommission'
    }
});