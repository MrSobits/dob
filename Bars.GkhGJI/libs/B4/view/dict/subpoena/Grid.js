Ext.define('B4.view.dict.subpoena.Grid', {
    extend: 'B4.ux.grid.Panel', 
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Edit',
        'Ext.ux.grid.FilterBar'
    ],

    title: 'Повестки на Комиссию',
    store: 'dict.Subpoena',
    alias: 'widget.subpoenaGrid',

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
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ComissionName',
                    flex: 1,
                    text: 'Имя комиссии',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            //plugins: [
            //    Ext.create('B4.ux.grid.plugin.HeaderFilters'),
            //    Ext.create('Ext.grid.plugin.CellEditing', {
            //        clicksToEdit: 1,
            //        pluginId: 'cellEditing'
            //    })
            //],
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
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