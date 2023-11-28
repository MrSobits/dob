Ext.define('B4.view.dict.individualperson.TransportGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.TypeCommissionMember'
    ],

    title: 'Транспортные средства',
    store: 'TransportOwner',
    alias: 'widget.individualpersontransportgrid',
    closable: true,

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
                    dataIndex: 'NameTransport',
                    width: 200,
                    flex: 1,
                    text: 'Наименование т/с',
                    filter: { xtype: 'textfield' }
                },               
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NamberTransport',
                    width: 100,
                    flex: 1,
                    text: 'Регистрационный номер',
                    filter: { xtype: 'textfield' }
                }, 
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DataOwnerStart',
                    text: 'Дата начала владения',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DataOwnerEdit',
                    text: 'Дата окончания владения',
                    format: 'd.m.Y',
                    width: 100
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