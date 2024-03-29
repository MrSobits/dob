﻿Ext.define('B4.view.actremoval.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.plugin.CellEditing'
    ],

    alias: 'widget.actRemovalViolationGrid',
    title: 'Устранение нарушений',
    store: 'actremoval.Violation',
    itemId: 'actRemovalViolationGrid',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CodesPin',
                    width: 80,
                    text: 'Пункты НПД'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGji',
                    flex: 1,
                    text: 'Текст нарушения'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CircumstancesDescription',
                    flex: 1,
                    text: 'Описание',
                    editor: { xtype: 'textfield' },
                    itemId: 'gcCircumstancesDescription'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePlanRemoval',
                    text: 'Срок устранения',
                    format: 'd.m.Y',
                    width: 100,
                    itemId: 'cdfDatePlanRemoval'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFactRemoval',
                    text: 'Дата факт. исполнения',
                    format: 'd.m.Y',
                    width: 140,
                    editor: { xtype: 'datefield', format: 'd.m.Y' },
                    itemId: 'cdfDateFactRemoval'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 110,
                    text: 'Мун. образование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес'
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
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
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