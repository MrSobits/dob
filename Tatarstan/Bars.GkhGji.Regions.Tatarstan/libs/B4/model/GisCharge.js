﻿Ext.define('B4.model.GisCharge', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisCharge'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution'},
        { name: 'DateSend' },
        { name: 'IsSent' },
        { name: 'JsonObject' }
    ]
});