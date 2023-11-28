Ext.define('B4.view.dict.individualperson.ResolutionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    modal: true,
    alias: 'widget.individualpersonresolutiongrid',
    title: 'Статистика постановлений',
    store: 'dict.IndividualPersonResolution',
    itemId: 'individualPersonResolutionGrid',
    closable: false,

    initComponent: function() {
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
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    flex: 1,
                    text: 'Номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'InLawDate',
                    flex: 1,
                    text: 'Дата вступления в силу',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    text: 'Дата',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Violation',
                    flex: 1,
                    text: 'Нарушение',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PenaltyAmount',
                    flex: 1,
                    text: 'Сумма штрафа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'Paided',
                    flex: 1,
                    text: 'Оплаченный штраф',
                    trueText: 'Оплачен',
                    falseText: 'Не оплачен',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    text: 'Дата оплаты',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield' }
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