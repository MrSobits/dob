﻿Ext.define('B4.view.realityobj.ConstructiveElementGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realityobjconstructiveelementgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Конструктивные элементы',
    store: 'realityobj.ConstructiveElement',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ConstructiveElementGroup',
                    flex: 1,
                    text: 'Группа'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ConstructiveElementName',
                    flex: 3,
                    text: 'Конструктивный элемент'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LastYearOverhaul',
                    flex: 1,
                    text: 'Год последнего кап. ремонта',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ConstructiveElementRepairPlanDate',
                    flex: 1,
                    text: 'Плановая дата ремонта'
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'b4savebutton',
                                    itemId: 'btnSaveConstructiveElement'
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