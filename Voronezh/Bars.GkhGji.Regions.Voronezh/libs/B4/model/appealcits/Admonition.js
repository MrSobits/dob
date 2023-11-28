Ext.define('B4.model.appealcits.Admonition', {
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
        { name: 'DocumentName', defaultValue: 'Предостережение' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'PerfomanceDate' },
        { name: 'Violations' },
        { name: 'Number' },
        { name: 'KindKNDGJI', defaultValue: 0 },
        { name: 'PerfomanceFactDate' },
        { name: 'Inspector', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'Signature', defaultValue: null },
        { name: 'Executor', defaultValue: null },
        { name: 'AnswerFile', defaultValue: null },
        { name: 'SignedAnswerFile', defaultValue: null },
        { name: 'AnswerSignature', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'Address', defaultValue: null }
    ]
});