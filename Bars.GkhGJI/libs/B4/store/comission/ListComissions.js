Ext.define('B4.store.comission.ListComissions', {
    extend: 'B4.base.Store',
    requires: ['B4.model.comission.ComissionMeetingDocument'],
    autoLoad: false,
    model: 'B4.model.comission.ComissionMeetingDocument',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListListComissionsInDocument'
    }
});