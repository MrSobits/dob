﻿Ext.define('B4.model.transferrf.TransferCtr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransferCtr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProgramCr', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'ObjectCr', defaultValue: null },
        { name: 'Builder', defaultValue: null },
        { name: 'ContragentBank', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'Perfomer', defaultValue: null },
        { name: 'DocumentName', defaultValue: null },
        { name: 'DocumentNum', defaultValue: null },
        { name: 'DocumentNumPp', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'DateFrom', defaultValue: null },
        { name: 'DateFromPp', defaultValue: null },
        { name: 'PaymentType', defaultValue: null },
        { name: 'Contract', defaultValue: null },
        { name: 'TypeWorkCr', defaultValue: null },
        { name: 'PaymentPurposeDescription' },
        { name: 'RegOperator', defaultValue: null },
        { name: 'RegopCalcAccount', defaultValue: null },
        { name: 'ProgramCrType', defaultValue: null },
        { name: 'Comment', defaultValue: null },
        { name: 'KindPayment', defaultValue: 0 },
        { name: 'TypeProgramRequest', defaultValue: null },
        { name: 'PaymentDate' },
        { name: 'PaidSum' },
        { name: 'Sum' },
        { name: 'IsExport' },
        { name: 'BuilderInn' },
        { name: 'BuilderSettlAcc' },
        { name: 'CalcAccNumber' },
        { name: 'Perfomer' },
        { name: 'TransferGuid' },
        { name: 'PaymentPriority', defaultValue: '5' },
        { name: 'IsEditPurpose', defaultValue: false },
        { name: 'TypeCalculationNds', defaultValue: 0 },
        { name: 'Document' },
        { name: 'FinSource' }
    ]
});