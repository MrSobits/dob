Ext.define('B4.model.administration.risdataexport.FormatDataExportTask', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'Login' },
        { name: 'TriggerName' },
        { name: 'CreateDate' },
        { name: 'StartNow', defaultValue: false },
        { name: 'StartDate', useNull: true },
        { name: 'EndDate', useNull: true },
        { name: 'EntityGroupCodeList', useNull: false, defaultValue: [] },

        { name: 'PeriodType' },
        { name: 'StartTimeHour', defaultValue: 0 },
        { name: 'StartTimeMinutes', defaultValue: 0 },
        { name: 'IsDelete', defaultValue: false },
        { name: 'StartDayOfWeekList', useNull: false, defaultValue: [] },
        { name: 'StartMonthList', useNull: false, defaultValue: [] },
        { name: 'StartDaysList', useNull: false, defaultValue: [] },
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'FormatDataExportTask'
    },
});