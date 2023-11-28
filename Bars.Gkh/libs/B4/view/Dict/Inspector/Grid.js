Ext.define('B4.view.dict.inspector.Grid', {
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

    title: 'Члены комиссии',
    store: 'dict.Inspector',
    alias: 'widget.inspectorGrid',
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
                    dataIndex: 'Fio',
                    width: 200,
                    flex: 1,
                    text: 'ФИО',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Phone',
                    width: 100,
                    text: 'Телефон',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Email',
                    flex: 0.5,
                    text: 'Электронная почта',
                    filter: { xtype: 'textfield' }
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeCommissionMember',
                    dataIndex: 'TypeCommissionMember',
                    text: 'Роль',
                    flex: 0.5,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NotMemberPosition',
                    flex: 1,
                    text: 'Должность',
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