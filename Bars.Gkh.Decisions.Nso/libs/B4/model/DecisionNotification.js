﻿Ext.define('B4.model.DecisionNotification', {
    extend: 'B4.base.Model',
    //idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionNotification'
    },
    fields: [
        { name: 'Id' },
        { name: 'Protocol' },
        { name: 'Number' },
        { name: 'Date' },
        { name: 'Document' },
        { name: 'ProtocolFile' },
        { name: 'AccountNum' },
        { name: 'OpenDate' },
        { name: 'CloseDate' },
        { name: 'BankDoc' },
        { name: 'IncomeNum' },
        { name: 'RegistrationDate' },
        { name: 'OriginalIncome' },
        { name: 'CopyIncome' },
        { name: 'CopyProtocolIncome' },
        { name: 'State' },

        { name: 'MoSettlement' },
        { name: 'Mu' },
        { name: 'Address' },
        { name: 'Manage' },
        { name: 'FormFundType' },
        { name: 'OrgName' },
        { name: 'PostAddress' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Ogrn' },
        { name: 'Oktmo' },
        { name: 'CreditOrgName' },
        { name: 'CreditOrgAddress' },
        { name: 'CreditOrgBik' },
        { name: 'CreditOrgCorAcc' },
        { name: 'CreditOrgInn' },
        { name: 'CreditOrgKpp' },
        { name: 'CreditOrgOgrn' },
        { name: 'CreditOrgOktmo' },

        { name: 'ProtocolDateNum' },
        { name: 'ProtocolId' },
        { name: 'RealObjId' },
        { name: 'HasCertificate' },
        { name: 'AreaLivingNotLivingMkd' }
    ]
});