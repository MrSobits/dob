﻿/*
    Проверочный расчет Model
*/
Ext.define('B4.model.delta.DeltaOfChargesOverride', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        timeout: 900000,
        type: 'b4proxy',
        controllerName: 'Delta'
    },
    fields: [
        { name: 'DataBank' },
        { name: 'PersonalAccountId' },
        { name: 'ServiceId' },
        { name: 'ServiceName' },
        { name: 'Measure' },
        { name: 'SupplierId' },
        { name: 'SupplierName' },
        { name: 'FormulaId' },
        { name: 'FormulaName' },
        { name: 'MeasureId' },
        { name: 'MeasureName' },
        { name: 'ParentId' },
        { name: 'ChargeDate' },
        { name: 'Date' },
        { name: 'RecalculationDate' },
        { name: 'Tariff' },
        { name: 'TariffPrev' },
        { name: 'Norm' },
        { name: 'NormConsumption' },
        { name: 'Consumption' },
        { name: 'ConsumptionPrev' },
        { name: 'ConsumptionFull' },
        { name: 'ConsumptionFullPrev' },
        { name: 'ConsumptionODN' },
        { name: 'Recalculation' },
        { name: 'RecalculationPositive' },
        { name: 'RecalculationNegative' },
        { name: 'FullCalculation' },
        { name: 'FullCalculationPrev' },
        { name: 'Credited' },
        { name: 'CalculationTariff' },
        { name: 'CalculationTariffPrev' },
        { name: 'ShortDelivery' },
        { name: 'ShortDeliveryPrev' },
        { name: 'Benefit' },
        { name: 'BenefitPrev' },
        { name: 'CalculationDaily' },
        { name: 'CalculationDailyPrev' },
        { name: 'Change' },
        { name: 'ChangePositive' },
        { name: 'ChangeNegative' },
        { name: 'Paid' },
        { name: 'IncomingSaldo' },
        { name: 'OutcomingSaldo' },
        { name: 'Payable' },
        { name: 'PayableEnd' },
        { name: 'BenefitAll' },
        { name: 'IncomingSaldoBegin' },
        { name: 'OutcomingSaldoEnd' },
        { name: 'HasPreviousRecalculation' },
        { name: 'HasNextRecalculation' },
        { name: 'CalculationSign' },

        { name: 'CurYear' },
        { name: 'CurMonth' },
        { name: 'IsGis' },
        { name: 'Delta', type: 'object' },
        { name: 'Topic', type: 'string' },
        { name: 'TopicName', type: 'string' },

        { name: 'IsComunal' }
    ]
});
