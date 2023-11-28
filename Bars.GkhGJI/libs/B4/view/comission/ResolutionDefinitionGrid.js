Ext.define('B4.view.comission.ResolutionDefinitionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.GridStateColumn',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.form.ComboBox',
        'B4.enums.ControlType',
        'B4.enums.ComissionDocumentDecision',
        'B4.view.Control.GkhButtonPrint',
        'B4.ux.grid.column.Enum',
        'B4.enums.KindKNDGJI',
        'B4.ux.grid.selection.CheckboxModel',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.enums.TypeBase',
        'B4.enums.TypeDefinitionResolution'
    ],

    title: 'Определения постановления',
    store: 'comission.ListResolutionDefinitions',
    itemId: 'comissionResolutionDefinitionGrid',
    alias: 'widget.comissionresolutiondefinitiongrid',
    closable: false,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columnLines: true,
            columns: [
                //{
                //    xtype: 'actioncolumn',
                //    width: 25,
                //    items: [{
                //        tooltip: 'Переход к постановлению',
                //        icon: B4.Url.content('content/img/icons/arrow_right.png'),
                //        handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                //            debugger;
                //            me.fireEvent('gotoresolution', rec);
                //        },
                //    }]
                //},
                //{
                //    xtype: 'b4editcolumn',
                //    scope: me
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    flex: 1,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ExecutionDate',
                    text: 'Дата исполнения',
                    format: 'd.m.Y',
                    width: 100,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 100,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IssuedDefinition',
                    flex: 1,
                    text: 'ДЛ, вынесшее определение',
                    filter: { xtype: 'textfield' },
                    renderer: function (val) { return val.Fio; }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeDefinition',
                    flex: 1,
                    text: 'Тип определения',
                    enumName: 'B4.enums.TypeDefinitionResolution',
                    filter: true
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    action: 'ResolutionDefinitionPrint',
                                    name: 'ResolutionDefinitionPrint',
                                    itemId: 'btnResolutionDefinitionPrint'
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