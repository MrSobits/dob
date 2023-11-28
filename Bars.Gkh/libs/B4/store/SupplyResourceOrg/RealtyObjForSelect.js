﻿Ext.define('B4.store.supplyresourceorg.RealtyObjForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    storeId: 'realtyObjForSelectStore',
    model: 'B4.model.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListRoBySupplyResorg'
    }
});