Ext.define('B4.view.comission.InspectorGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.comissioninspectorgrid',

    requires: [
        'B4.Url',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeCommissionMember',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Должностные лица',
    store: 'comission.ComissionMeetingInspector',

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
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'ФИО'
                },
                //{
                //    xtype: 'b4enumcolumn',
                //    enumName: 'B4.enums.TypeCommissionMember',
                //    dataIndex: 'TypeCommissionMember',
                //    flex: 1,
                //    text: 'Должность',
                //    filter: true
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeCommissionMember',
                    flex: 0.5,
                    text: 'Исполняет должность',
                    renderer: function (val) {
                        return B4.enums.TypeCommissionMember.displayRenderer(val);
                    },
                    editor: {
                        xtype: 'b4combobox',
                        valueField: 'Value',
                        displayField: 'Display',
                        items: B4.enums.TypeCommissionMember.getItems(),
                        editable: false
                    }
                },               
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                }),
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept',
                                    itemId: 'inspectorSaveButton'
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