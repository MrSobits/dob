﻿Ext.define('B4.view.dict.additwork.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Доп. виды работ',
    store: 'dict.AdditWork',
    alias: 'widget.additWorkGrid',
    //closable: true,

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
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 50
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Percentage',
                    flex: 1,
                    text: 'Процент',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 3
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Queue',
                    flex: 1,
                    text: 'Очередность',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        maxLength: 2,
                        mouseWheelEnabled: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000
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