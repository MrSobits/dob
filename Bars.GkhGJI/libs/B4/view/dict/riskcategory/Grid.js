Ext.define('B4.view.dict.riskcategory.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.riskcategorygrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',

        'B4.store.dict.RiskCategory'
    ],

    title: 'Категории риска УК',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.dict.RiskCategory');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    editor: {
                        xtype: 'textfield',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RiskFrom',
                    flex: 1,
                    text: 'Показатель риска от',
                    editor: {
                        xtype: 'numberfield',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RiskTo',
                    flex: 1,
                    text: 'Показатель риска до',
                    editor: {
                        xtype: 'numberfield',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'b4deletecolumn'
                }
            ],

            plugins: [
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
                            columns: 4,
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});