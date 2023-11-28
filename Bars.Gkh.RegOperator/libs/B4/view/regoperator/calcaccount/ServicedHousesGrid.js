﻿Ext.define('B4.view.regoperator.calcaccount.ServicedHousesGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.regopservicedhousesgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.regoperator.RobjectToAddAccount'
    ],

    title: 'Обслуживаемые дома',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.calcaccount.RealityObject');

        Ext.applyIf(me, {
            store: store,

            columns: [
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    dataIndex: 'Address',
                    text: 'Адрес',
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