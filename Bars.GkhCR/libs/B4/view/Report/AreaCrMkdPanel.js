﻿Ext.define('B4.view.report.AreaCrMkdPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'areaCrMkdPanel',
    layout: {
        type : 'vbox'
    },
    border: false,
    
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.view.dict.programcr.Grid',
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox'
    ],
    initComponent: function() {
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
                    name: 'ProgramCr',
                    itemId: 'sfProgramCr',
                    textProperty: 'Name',
                    fieldLabel: 'Программа кап.ремонта',
                    store: 'B4.store.dict.ProgramCr',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xype: 'datefield',
                    xtype: 'datefield',
                    name: 'ReportDate',
                    itemId: 'dfReportDate',
                    fieldLabel: 'Дата отчета',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'FinSources',
                    itemId: 'tfFinSources',
                    fieldLabel: 'Разрезы финансирования',
                    emptyText: 'Все разрезы'
                },
                {
                    xtype: 'b4combobox',
                    name: 'Graph',
                    itemId: 'graph',
                    fieldLabel: 'График выпонения',
                    editable: false,
                    items: [[1, 'Учитывать'], [0, 'Не учитывать']],
                    value: 0
                }
            ]
            
        });
        
        me.callParent(arguments);
    }           
});