﻿Ext.define('B4.view.dict.prodcalendar.Grid', {
    extend: 'B4.ux.grid.Panel', 
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.YesNo',
        'B4.form.ComboBox',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Производственный календарь',
    store: 'dict.ProdCalendar',
    alias: 'widget.prodCalendarGrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ProdDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100,
                    flex: 1,
                    editor: {
                        xtype: 'datefield',
                        maxLength: 300,
                        format: 'd.m.Y'
                    },
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkDay',
                    flex: 0.5,
                    text: 'Рабочий',
                    renderer: function (val) {
                        return B4.enums.YesNo.displayRenderer(val);
                    },
                    editor: {
                        xtype: 'b4combobox',
                        valueField: 'Value',
                        displayField: 'Display',
                        items: B4.enums.YesNo.getItems(),
                        editable: false
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});