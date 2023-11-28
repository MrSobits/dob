Ext.define('B4.view.disposal.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.disposalViolationGrid',
    store: 'disposal.Violation',
    itemId: 'disposalViolationGrid',
    title: 'Нарушения',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGji',
                    flex: 1.3,
                    text: 'Нарушение',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Подробнее',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Action',
                    flex: 1,
                    text: 'Мероприятие',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'pagingtoolbar',
                    store: 'disposal.Violation',
                    dock: 'bottom',
                    displayInfo: true,
                    displayMsg: 'Всего записей {2}',
                    getPagingItems: function () {
                        return [];
                    },
                    onLoad: function () {
                        Ext.suspendLayouts();
                        this.updateInfo();
                        Ext.resumeLayouts(true);
                    }
                }
            ]
        });

        me.callParent(arguments);
    }
});