Ext.define('B4.view.dict.articlelawgji.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.OmsRegionBelonging',
        'B4.ux.grid.column.Enum',
        'B4.form.ComboBox'
    ],

    title: 'Типы нарушения',
    store: 'dict.ArticleLawGji',
    alias: 'widget.articleLawGjiGrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Article',
                    flex: 1,
                    text: 'Статья',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Part',
                    flex: 1,
                    text: 'Пункт',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KBK',
                    flex: 1,
                    text: 'КБК Региона',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'OMS',
                //    flex: 1,
                //    text: 'Принадлежность к ОМС/Регион',
                //    editor: {
                //        xtype: 'textfield',
                //        maxLength: 300
                //    },
                //    filter: {
                //        xtype: 'textfield'
                //    }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OmsRegion',
                    flex: 0.5,
                    text: 'Принадлежность к ОМС/Регион',
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.OmsRegionBelonging.getItems(),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    renderer: function (val) {
                        return B4.enums.OmsRegionBelonging.displayRenderer(val);
                    },
                    editor: {
                        xtype: 'b4combobox',
                        valueField: 'Value',
                        displayField: 'Display',
                        items: B4.enums.OmsRegionBelonging.getItems(),
                        editable: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameOMS',
                    flex: 1,
                    text: 'Наименование ОМС для ОМС/Регион',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 2000
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Bank',
                    flex: 1,
                    text: 'Банк получателя для  ОМС/РЕГИОН',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 2000
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 3,
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});