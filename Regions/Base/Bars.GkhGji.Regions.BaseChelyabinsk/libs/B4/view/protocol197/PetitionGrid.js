Ext.define('B4.view.protocol197.PetitionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.YesNoNotSetPartially',
        'B4.ux.grid.column.Enum',
        'B4.store.dict.Inspector'
    ],

    alias: 'widget.protocol197petitiongrid',
    title: 'Ходатайства',
    store: 'protocol197.Petition',
    itemId: 'protocol197PetitionGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PetitionDate',
                    flex: 0.5,
                    text: 'Дата ходатайства',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PetitionAuthorFIO',
                    flex: 1,
                    text: 'ФИО'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PetitionAuthorDuty',
                    text: 'Должность',
                    flex: 1,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Workplace',
                    text: 'Место работы',
                    flex: 1,
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSetPartially',
                    dataIndex: 'Aprooved',
                    text: 'Решение',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                            columns: 2,
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