Ext.define('B4.model.appealcits.AppealCitsAdmonition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsAdmonition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'DocumentName' },
        { name: 'KindKNDGJI', defaultValue: 0 },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'PerfomanceDate' },
        { name: 'PerfomanceFactDate' },
        { name: 'Inspector', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'Signature', defaultValue: null },
        { name: 'Executor', defaultValue: null },
        { name: 'AnswerFile', defaultValue: null },
        { name: 'SignedAnswerFile', defaultValue: null },
        { name: 'AnswerSignature', defaultValue: null }
    ]
});