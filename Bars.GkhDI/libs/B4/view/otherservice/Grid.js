Ext.define('B4.view.otherservice.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhIntField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Прочие услуги',
    store: 'OtherService',
    itemId: 'otherServiceGrid',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 5,
                    text: 'Наименование',
                    filter:
                    {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    filter:
                    {
                        xtype: 'gkhintfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 1,
                    text: 'Ед. измерения',
                    filter:
                    {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Tariff',
                    flex: 1,
                    text: 'Тариф',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val)) {
                            val = '' + val;
                            if (val.indexOf('.') != -1) {
                                val = val.replace('.', ',');
                            }
                            return val;
                        }
                        return '';
                    },
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Provider',
                    flex: 2,
                    text: 'Поставщик',
                    filter:
                    {
                        xtype: 'textfield'
                    }
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
                            columns: 1,
                            items: [
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