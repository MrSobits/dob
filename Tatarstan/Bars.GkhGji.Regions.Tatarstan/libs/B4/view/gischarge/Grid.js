Ext.define('B4.view.gischarge.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.store.GisCharge',
        'B4.ux.grid.filter.YesNo'
    ],

    alias: 'widget.gischargegrid',
    title: 'Отправка начислений в ГИС ГМП',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.GisCharge');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    name: 'showJson',
                    width: 20,
                    icon: 'content/img/icons/cog_go.png',
                    tooltip: 'Отобразить передаваемый json'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Resolution',
                    text: 'Постановление',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateSend',
                    format: 'd.m.Y H:i',
                    width: 120,
                    text: 'Дата отправки'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsSent',
                    width: 100,
                    text: 'Отправлено',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me,
                    hidden: true
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
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            store.load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отправить сейчас',
                                    action: 'SendNow'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Загрузить сейчас',
                                    action: 'UploadNow'
                                }/*,
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            fieldLabel: 'С',
                                            name: 'DateStart',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'datefield',
                                            fieldLabel: 'По',
                                            name: 'DateEnd',
                                            format: 'd.m.Y'
                                        }
                                    ]
                                }*/
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