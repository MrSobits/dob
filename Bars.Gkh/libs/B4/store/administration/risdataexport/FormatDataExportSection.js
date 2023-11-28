Ext.define('B4.store.administration.risdataexport.FormatDataExportSection', {
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.risdataexport.FormatDataExportSection'],
    autoLoad: false,
    model: 'B4.model.administration.risdataexport.FormatDataExportSection',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FormatDataExport',
        listAction: 'ListAvailableSection'
    },
    pageSize: 100,
    sorters: [
        {
            property: 'Description',
            direction: 'ASC'
        }
    ]
});