Ext.define('B4.view.asfk.ASFKResolutionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.form.ComboBox',
        'B4.store.asfk.BDOPER',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo'
    ],

    alias: 'widget.asfkresolutiongrid',
    store: 'asfk.ASFKResolution',
    itemId: 'asfkResolutionGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата документа',
                    flex: 1,
                    dataIndex: 'DocumentDate',
                    filter: { xtype: 'datefield', format: 'd.m.Y' },
                    renderer: function (val) {
                        if (val != null) {
                            return Ext.Date.format(new Date(val), 'd.m.Y');
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата вступления в законную силу',
                    dataIndex: 'InLawDate',
                    flex: 1,
                    filter: { xtype: 'datefield', format: 'd.m.Y' },
                    renderer: function (val) {
                        if (val != null) {
                            return Ext.Date.format(new Date(val), 'd.m.Y');
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PenaltyAmount',
                    text: 'Сумма штрафа',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    text: 'ФИО нарушителя',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    text: 'Контрагент',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ComissionName',
                    text: 'Комиссия',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
                //{
                //    xtype: 'b4deletecolumn',
                //    scope: me
                //}
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    text: 'Привязать постановление'
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