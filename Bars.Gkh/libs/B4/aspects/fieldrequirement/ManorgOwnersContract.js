Ext.define('B4.aspects.fieldrequirement.ManorgOwnersContract', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.manorgownerscontractfieldrequirement',
    
    init: function() {
        this.requirements = [
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.StartDatePaymentPeriod_Rqrd',
                applyTo: '[name=StartDatePaymentPeriod]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.EndDatePaymentPeriod_Rqrd',
                applyTo: '[name=EndDatePaymentPeriod]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.PaymentAmount_Rqrd',
                applyTo: '[name=PaymentAmount]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.SetPaymentsFoundation_Rqrd',
                applyTo: '[name=SetPaymentsFoundation]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.PaymentProtocolFile_Rqrd',
                applyTo: '[name=PaymentProtocolFile]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.PaymentProtocolDescription_Rqrd',
                applyTo: '[name=PaymentProtocolDescription]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.RevocationReason_Rqrd',
                applyTo: '[name=RevocationReason]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.DateLicenceRegister_Rqrd',
                applyTo: '[name=DateLicenceRegister]',
                selector: '#manorgContractOwnersEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServiceName_Rqrd',
                applyTo: '[name=WorkService]',
                selector: 'ownersworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServicPaymentAmount_Rqrd',
                applyTo: '#Type',
                selector: 'ownersworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServiceType_Rqrd',
                applyTo: '[name=PaymentAmount]',
                selector: 'ownersworkserviceeditwindow'
            }
        ];

        this.callParent(arguments);
    }
});