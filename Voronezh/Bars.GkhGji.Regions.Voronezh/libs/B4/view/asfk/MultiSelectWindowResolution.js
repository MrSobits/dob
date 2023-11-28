Ext.define('B4.view.asfk.MultiSelectWindowResolution', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.grid.Panel',
        'B4.form.FileField',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    itemId: 'multiSelectWindowResolution',
    closeAction: 'hide',
    height: 500,
    width: 900,
    modal: true,
    layout: 'fit',
    mixins: ['B4.mixins.window.ModalMask'],
    maximizable: true,
    trackResetOnLoad: true,
    title: 'Выбор элементов',
    titleGridSelect: 'Элементы для выбора',
    titleGridSelected: 'Выбранные элементы',
    storeSelect: null,
    storeSelected: null,
    columnsGridSelect: [],
    columnsGridSelected: [],
    selModelMode: null, //по умолчанию аспект передает 'MULTI'

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'panel',
                            flex: 2,
                            layout: 'fit',
                            border: false,
                            items: [
                                {
                                    xtype: 'b4grid',
                                    itemId: 'multiSelectGrid',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    title: this.titleGridSelect,
                                    border: false,
                                    store: this.storeSelect,
                                    selModel: Ext.create('Ext.selection.CheckboxModel', { mode: this.selModelMode }),
                                    columns: this.columnsGridSelect,
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
                                                            xtype: 'b4updatebutton'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4pagingtoolbar',
                                            displayInfo: true,
                                            store: this.storeSelect,
                                            dock: 'bottom'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            itemId: 'multiSelectedPanel',
                            split: false,
                            collapsible: false,
                            border: false,
                            height: 200,
                            flex: 1,
                            style: {
                                border: 'solid #99bce8',
                                borderWidth: '0 0 0 1px'
                            },
                            layout: 'fit',
                            items: [
                                {
                                    xtype: 'b4grid',
                                    bodyStyle: 'backrgound-color:transparent;',
                                    itemId: 'multiSelectedGrid',
                                    border: false,
                                    title: this.titleGridSelected,
                                    store: this.storeSelected,
                                    columns: this.columnsGridSelected,
                                    listeners: {
                                        afterrender: function () {
                                            var store = this.getStore();
                                            if (store)
                                                store.pageSize = store.getCount();
                                        }
                                    },
                                    dockedItems: [
                                        {
                                            xtype: 'toolbar',
                                            dock: 'bottom',
                                            items: [
                                                {
                                                    xtype: 'tbtext',
                                                    ref: 'status',
                                                    height: 16,
                                                    margin: 3,
                                                    text: '0 записи'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            itemId: 'bgwithfile',
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right'
                            },
                            columns: 2,
                            items: [
                                {
                                    xtype: 'container',
                                    border: false,
                                    layout: 'vbox',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            
                                            itemId: 'cbShowAllResolutions',
                                            fieldLabel: 'Выводить все постановления',
                                            labelAlign: 'right'
                                        }
                                    ]
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            dock: 'top',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'container',
                                    border: false,
                                    padding: '5 0 0 0',
                                    layout: 'vbox',
                                    defaults: {
                                        format: 'd.m.Y',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4savebutton',
                                            text: 'Применить'
                                        },
                                        {
                                            xtype: 'tbfill'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    border: false,
                                    padding: '5 0 0 0',
                                    layout: 'vbox',
                                    defaults: {
                                        format: 'd.m.Y',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4closebutton'
                                        },
                                        {
                                            xtype: 'tbfill'
                                        }
                                    ]
                                }

                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});