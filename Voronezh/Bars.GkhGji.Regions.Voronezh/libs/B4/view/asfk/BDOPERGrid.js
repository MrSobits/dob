Ext.define('B4.view.asfk.BDOPERGrid', {
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

    alias: 'widget.bdopergrid',
    store: 'asfk.BDOPER',
    itemId: 'bdoperGrid',

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
                    xtype: 'gridcolumn',
                    dataIndex: 'Kbk',
                    flex: 1,
                    text: 'КБК',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsPayFineAdded',
                    text: 'Найдено постановление',
                    flex: 0.5,
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno', operator: 'eq' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GUID',
                    flex: 1,
                    text: 'ГУИД операции',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 0.5,
                    text: 'Сумма платежа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NamePay',
                    flex: 2,
                    text: 'Данные о плательщике',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Purpose',
                    flex: 2,
                    text: 'Данные о назначении платежа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InnPay',
                    flex: 0.5,
                    text: 'ИНН плательщика',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KppPay',
                    flex: 0.5,
                    text: 'КПП плательщика',
                    filter: { xtype: 'textfield' }
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