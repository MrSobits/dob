Ext.define('B4.model.objectcr.PerformedWorkAct', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PerformedWorkAct'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr', defaultValue: null },
        { name: 'TypeWorkCr', defaultValue: null },
        { name: 'WorkName' },
        { name: 'Municipality' },
        { name: 'WorkFinanceSource' },
        { name: 'DocumentNum' },
        { name: 'Volume', defaultValue: null },
        { name: 'SumTransfer', defaultValue: null },
        { name: 'DateFromTransfer', defaultValue: null },
        { name: 'Sum', defaultValue: null },
        { name: 'DateFrom', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'Address' },
        { name: 'ObjectCrId', defaultValue: null },
        { name: 'DateFrom' },
        { name: 'DocumentNum' },
        { name: 'CostFile' },
        { name: 'DocumentFile' },
        { name: 'AdditionFile' },
        { name: 'IsWork' },
        { name: 'OverLimits', defaultValue: false },
        { name: 'UsedInExport', defaultValue: 20 }
    ]
});