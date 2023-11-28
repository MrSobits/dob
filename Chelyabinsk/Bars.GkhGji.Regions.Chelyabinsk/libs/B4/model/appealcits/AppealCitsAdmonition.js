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
        { name: 'DocumentName', defaultValue: 'Предостережение' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'PerfomanceDate' },
        { name: 'PerfomanceFactDate' },
        { name: 'Inspector', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'Executor', defaultValue: null },
        { name: 'AnswerFile', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'Address', defaultValue: null }
    ]
});