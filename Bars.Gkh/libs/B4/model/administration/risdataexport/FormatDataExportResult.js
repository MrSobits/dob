Ext.define('B4.model.administration.risdataexport.FormatDataExportResult', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'Login' },
        { name: 'Status' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'Progress' },
        { name: 'LogFile' },
        { name: 'EntityCodeList' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'FormatDataExportResult'
    }
});