Ext.define('B4.view.comission.ProtocolGrid', {
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
        'B4.ux.grid.selection.CheckboxModel'
    ],

    title: 'Протоколы',
    store: 'comission.ListProtocols',
    itemId: 'comissionProtocolGrid',
    alias: 'widget.comissionprotocolgrid',
    closable: false,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columnLines: true,
            columns: [
                {
                    xtype: 'actioncolumn',
                    width: 50,
                    items: [{
                        tooltip: 'Переход к протоколу',
                        icon: B4.Url.content('content/img/icons/arrow_right.png'),
                        handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                            debugger;
                            me.fireEvent('gotoprotocol', rec);
                        }
                         },
                         {
                            tooltip: 'Вынести решение по протоколу',
                            icon: B4.Url.content('content/img/icons/pencil.png'),
                            handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                                me.fireEvent('editpotocol', rec);
                            }
                         }]
                   
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateViolation',
                    text: 'Дата нарушения',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolatorFio',
                    flex: 1,
                    text: 'ФИО',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 1,
                    text: 'Наименование ЮЛ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Penalty',
                    flex: 1,
                    text: 'Сумма штрафа',
                    filter: { xtype: 'numberfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ComissionDocumentDecision',
                    dataIndex: 'ComissionDocumentDecision',
                    flex: 1,
                    text: 'Решение',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CaseNumber',
                    flex: 1,
                    text: 'Номер дела',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 75,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 80,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'MunicipalityNames',
                //    width: 160,
                //    text: 'Муниципальный район',
                //    filter: {
                //        xtype: 'b4combobox',
                //        operand: CondExpr.operands.eq,
                //        storeAutoLoad: false,
                //        hideLabel: true,
                //        editable: false,
                //        valueField: 'Name',
                //        emptyItem: { Name: '-' },
                //        url: '/Municipality/ListMoAreaWithoutPaging'
                //    }
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'TypeExecutant',
                //    flex: 1,
                //    text: 'Тип исполнителя',
                //    filter: { xtype: 'textfield' }
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'ContragentName',
                //    flex: 1,
                //    text: 'Организация',
                //    filter: { xtype: 'textfield' }
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'CountViolation',
                //    width: 75,
                //    text: 'Количество нарушений',
                //    filter: {
                //        xtype: 'numberfield',
                //        hideTrigger: true,
                //        operand: CondExpr.operands.eq
                //    }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ArticleLaw',
                    width: 250,
                    text: 'Статьи закона',
                    filter: {
                        xtype: 'textfield'
                    }
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
                                    action: 'DocumentComissionPrint',
                                    name: 'DocumentComissionPrint',
                                    itemId: 'btnDocumentComissionPrint'
                                },
                                {
                                    xtype: 'button',
                                    name: 'comissionoperation',
                                    text: 'Другие операции',
                                    iconCls: 'icon-cog-go',
                                    menu: []
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