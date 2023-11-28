Ext.define('B4.model.BaseStatement', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseStatement'
    },
    fields: [
        { name: 'ContragentName', defaultValue: null },
        { name: 'TypeBase', defaultValue: 20 },
        { name: 'Municipality' },
        { name: 'RealityObjectCount' },
        { name: 'FormCheck', defaultValue: 10 },
        { name: 'InspectionNumber' },
        { name: 'IsDisposal' },
        { name: 'RealObjAddresses' },
        { name: 'State', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'ControlType', defaultValue: 10 }
    ]
});