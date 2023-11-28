Ext.define('B4.store.comission.ListResolutions', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Resolution'],
    autoLoad: false,
    model: 'B4.model.Resolution',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListResolutionsInCommission'
    }
});