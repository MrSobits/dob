﻿Ext.define('B4.view.dict.notificationcause.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.NotificationCause'
    ],

    title: 'Причины уведомлений',
    alias: 'widget.notificationcausegrid',
    closable: true,
    store: 'dict.NotificationCause',

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
                        maxLength: 100
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});