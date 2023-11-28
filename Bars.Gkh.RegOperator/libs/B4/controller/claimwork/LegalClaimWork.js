Ext.define('B4.controller.claimwork.LegalClaimWork', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.view.claimwork.LegalGrid',
        'B4.aspects.ButtonDataExport',
        'B4.controller.claimwork.Navi',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkButtonPrintAspect',
        'B4.aspects.DebtorClaimWorkAspect',
        'B4.enums.DocumentFormationKind',
        'B4.enums.DebtorType',
        'B4.enums.ClaimWorkTypeBase'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'claimwork.LegalClaimWork'
    ],

    stores: [
         'claimwork.LegalClaimWork'
    ],

    views: [
        'claimwork.LegalGrid'
    ],

    refs: [
       {
           ref: 'mainView',
           selector: 'legalclaimworkgrid'
       }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.Legal.Update', applyTo: 'button[actionName=updateState]', selector: 'legalclaimworkgrid'
                },
                {
                    name: 'Clw.ClaimWork.Legal.Columns.BaseTariffDebtSum', applyTo: '[dataIndex=CurrChargeBaseTariffDebt]', selector: 'legalclaimworkgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.Columns.DecisionTariffDebtSum', applyTo: '[dataIndex=CurrChargeDecisionTariffDebt]', selector: 'legalclaimworkgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.Columns.DebtSum', applyTo: '[dataIndex=CurrChargeDebt]', selector: 'legalclaimworkgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'legalClaimWorkButtonExportAspect',
            gridSelector: 'legalclaimworkgrid',
            buttonSelector: 'legalclaimworkgrid #btnExport',
            controllerName: 'LegalClaimWork',
            actionName: 'Export',
            usePost: true
        },
        {
            xtype: 'debtorclaimworkaspect',
            name: 'legalClaimworkAspect',
            gridSelector: 'legalclaimworkgrid',
            storeName: 'claimwork.LegalClaimWork',
            modelName: 'claimwork.LegalClaimWork',
            controllerEditName: 'B4.controller.claimwork.Navi',
            entityName: 'LegalClaimWork',
            getDebtorType: function () {
                return B4.enums.DebtorType.Legal;
            }
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'legalPrintAspect',
            buttonSelector: 'legalclaimworkgrid gkhbuttonprint',
            codeForm: 'NotificationClw,Pretension,LawSuit,RestructDebt,RestructDebtAmicAgr,CourtClaim',
            massPrint: true,
            getClaimWorkType: function () {
                return B4.enums.ClaimWorkTypeBase.Debtor;
            },
            onBeforeLoadReportStore: function (store, operation) {
                operation.params = {};
                operation.params.codeForm = this.codeForm;
                operation.params.type = B4.enums.DebtorType.Legal;
            }
        }
    ],

    init: function () {
        var me = this;

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('legalclaimworkgrid');
        me.bindContext(view);
        me.application.deployView(view);

        me.getAspect('legalPrintAspect').loadReportStore();
        me.getAspect('legalClaimworkAspect').getDocsTypeToCreate();

        view.getStore().load();
    }
});