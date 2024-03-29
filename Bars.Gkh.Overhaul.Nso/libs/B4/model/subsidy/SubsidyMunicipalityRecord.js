Ext.define('B4.model.subsidy.SubsidyMunicipalityRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SubsidyMunicipalityRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'SubsidyMunicipality', defaultValue: null },
        { name: 'SubsidyYear' },
        { name: 'BudgetFund' },
        { name: 'BudgetRegion' },
        { name: 'BudgetMunicipality' },
        { name: 'OtherSource' },
        { name: 'CalculatedCollection' },
        { name: 'PlanCollection' },
        { name: 'ShareBudgetFund' },
        { name: 'ShareBudgetRegion' },
        { name: 'ShareBudgetMunicipality' },
        { name: 'ShareOtherSource' },
        { name: 'ShareOwnerFounds' },
        { name: 'FinanceNeedBefore' },
        { name: 'FinanceNeedAfter' },
        { name: 'FinanceNeedFromCorrect' },        
        { name: 'CalculatedTarif' },
        { name: 'RecommendedTarif' },
        { name: 'DeficitBefore' },
        { name: 'DeficitAfter' },
        { name: 'DeficitFromCorrect' },
        { name: 'StartRecommendedTarif' },
        { name: 'RecommendedTarifCollection' },
        { name: 'EstablishedTarif' },
        { name: 'OwnersLimit' },
        { name: 'OwnersMoneyBalance' },
        { name: 'BudgetFundBalance' },
        { name: 'BudgetRegionBalance' },
        { name: 'BudgetMunicipalityBalance' },
        { name: 'OtherSourceBalance' }
    ]
});