﻿Ext.define('B4.view.report.HeatSeasonReceivedDocumentsPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'heatSeasonReceivedDocumentsPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.view.dict.heatseasonperiodgji.Grid',
        'B4.store.dict.HeatSeasonPeriodGji',
        'B4.form.SelectField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'HeatSeasonPeriodGji',
                    itemId: 'sfHeatSeasonPeriodGji',
                    fieldLabel: 'Отопительный сезон',
                    store: 'B4.store.dict.HeatSeasonPeriodGji',
                   

                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'ReportDate',
                    itemId: 'dfReportDate',
                    fieldLabel: 'Дата отчета',
                    format: 'd.m.Y',
                    value: new Date(),
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'b4combobox',
                    name: 'HeatType',
                    itemId: 'cbHeatType',
                    fieldLabel: 'Тип отопления',
                    editable: false,
                    items: [[20, 'Централизованное'], [10, 'Индивидуальное'], [30, 'Централизованное и индивидуальное']],
                    value: 30
                },
                {
                    xtype: 'b4combobox',
                    name: 'RealtyObjectType',
                    itemId: 'cbRoType',
                    fieldLabel: 'Тип дома',
                    editable: false,
                    items: [[30, 'Многоквартирный'], [40, 'Общежитие'], [50, 'Многоквартирный и общежитие']],
                    value: 50
                }
            ]
        });

        me.callParent(arguments);
    }
});