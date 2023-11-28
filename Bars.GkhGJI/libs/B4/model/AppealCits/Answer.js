﻿Ext.define('B4.model.appealcits.Answer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsAnswer'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'Executor', defaultValue: null },
        { name: 'Signer', defaultValue: null },
        { name: 'Addressee', defaultValue: null },
        { name: 'AnswerContent', defaultValue: null },
        { name: 'DocumentName' },
        { name: 'SerialNumber' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'IsMoved', defaultValue: false },
        { name: 'State', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'FileDoc', defaultValue: null },      
        { name: 'AdditionalInfo', defaultValue: null },
        { name: 'IsUploaded', defaultValue: null },
        { name: 'ExecDate', defaultValue: null },
        { name: 'ExtendDate', defaultValue: null },
        { name: 'ConcederationResult', defaultValue: null },
        { name: 'FactCheckingType', defaultValue: null },
        { name: 'RedirectContragent' },
        { name: 'Address' }
    ]
});