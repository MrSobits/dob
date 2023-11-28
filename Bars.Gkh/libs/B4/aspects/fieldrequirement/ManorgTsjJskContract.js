Ext.define('B4.aspects.fieldrequirement.ManorgTsjJskContract', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.manorgtsjjskcontractfieldrequirement',
    
    init: function() {
        this.requirements = [
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.StartDatePaymentPeriod_Rqrd',
                applyTo: '[name=StartDatePaymentPeriod]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.EndDatePaymentPeriod_Rqrd',
                applyTo: '[name=EndDatePaymentPeriod]',
                selector: '#jskTsjContractEditWindow'
            },
             {
                 name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.CompanyReqiredPaymentAmount_Rqrd',
                 applyTo: '[name=CompanyReqiredPaymentAmount]',
                 selector: '#jskTsjContractEditWindow'
             },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.ReqiredPaymentAmount_Rqrd',
                applyTo: '[name=ReqiredPaymentAmount]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.CompanyPaymentProtocolFile_Rqrd',
                applyTo: '[name=CompanyPaymentProtocolFile]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.CompanyPaymentProtocolDescription_Rqrd',
                applyTo: '[name=CompanyPaymentProtocolDescription]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentAmount_Rqrd',
                applyTo: '[name=PaymentAmount]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentProtocolFile_Rqrd',
                applyTo: '[name=PaymentProtocolFile]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentProtocolDescription_Rqrd',
                applyTo: '[name=PaymentProtocolDescription]',
                selector: '#jskTsjContractEditWindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.WorkServiceName_Rqrd',
                applyTo: '[name=WorkService]',
                selector: 'jsktsjworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.WorkServicPaymentAmount_Rqrd',
                applyTo: '#Type',
                selector: 'jsktsjworkserviceeditwindow'
            },
            {
                name: 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.WorkServiceType_Rqrd',
                applyTo: '[name=PaymentAmount]',
                selector: 'jsktsjworkserviceeditwindow'
            },
        ];

        this.callParent(arguments);
    }
});