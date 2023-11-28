Ext.define('B4.view.documentsgjiregister.ResolutionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.GridStateColumn',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.view.Control.GkhButtonPrint',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.ux.grid.column.Enum',
        'B4.enums.ControlType',
        'B4.enums.TypeBase',
        'B4.ux.grid.selection.CheckboxModel'
    ],

    title: 'Постановления',
    store: 'view.Resolution',
    itemId: 'docsGjiRegisterResolutionGrid',
    alias: 'widget.registerresolutiongrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;
        store = Ext.create('B4.store.view.Resolution'),
            numberFilter = {
                xtype: 'numberfield',
                allowDecimals: false,
                hideTrigger: true,
                operand: CondExpr.operands.eq
            };

        var renderer = function (val, meta, rec) {

            var sanction = rec.get('Sanction');
            var penaltyAmount = rec.get('PenaltyAmount');
            if (penaltyAmount == null) {
                return val;
            }
            var sumPays = rec.get('SumPays');
            if (sumPays == null) {
                sumPays = 0;
            }
            var dueDate = rec.get('DueDate');
            var paymentDate = rec.get('PaymentDate');
            var hasProtocol = rec.get('HasProtocol');

            if (sanction == 'Административный штраф')
            {
                if ((penaltyAmount - sumPays) > 0 && new Date() > new Date(dueDate) && hasProtocol == false) {
                    meta.style = 'background: red;';
                    meta.tdAttr = 'data-qtip="Истек срок оплаты штрафа"';
                }
                else if (penaltyAmount - sumPays <= 0 && new Date(paymentDate) > new Date(dueDate)) {
                    meta.style = 'background: yellow;';
                    meta.tdAttr = 'data-qtip="Штраф оплачен с опозданием"';
                }
            }

            return val;
        };

        //поскольку требуется чтобы в енуме Тип Основания небыло постановления прокуратуры
        //то получаем енум и формируем из его item-ов новый енум но без постановления прокуратуры
        var currTypeBase = B4.enums.TypeBase.getItemsWithEmpty([null, '-']);
        var newTypeBase = [];

        Ext.iterate(currTypeBase, function (val, key) {
            if (key != 6)
                newTypeBase.push(val);
        });
        
        Ext.applyIf(me, {
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус',
                    width: 125,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_document_resol';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this,
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeBase',
                    width: 150,
                    // 'Основание проверки'
                    text: 'Основание',
                    renderer: function (val, meta, rec) {
                        val = renderer(val, meta, rec);
                        if (val != 60)
                            return B4.enums.TypeBase.displayRenderer(val);
                        return '';
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: newTypeBase,
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ConcederationResult',
                    width: 160,
                    text: 'Результат рассмотрения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'SentToOSP',
                    text: 'Отправлено в ССП',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'HasProtocol',
                    text: 'Создан протокол',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                //{
                //    xtype: 'b4enumcolumn',
                //    enumName: 'B4.enums.ControlType',
                //    dataIndex: 'ControlType',
                //    text: 'Вид контроля',
                //    filter: true,
                //    flex: 1
                //},
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeInitiativeOrgGji',
                    dataIndex: 'TypeInitiativeOrg',
                    text: 'Кем вынесено',
                    flex: 0.5,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OfficialName',
                    flex: 1,
                    text: 'ДЛ вынесшее постановление',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OfficialPosition',
                    text: 'Должность вынесшего постановление',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.icontains,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Position/List'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityNames',
                    width: 160,
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MoSettlement',
                    width: 160,
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlaceName',
                    width: 160,
                    text: 'Населенный пункт',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeExecutant',
                    flex: 1,
                    text: 'Тип нарушителя',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PhysicalPerson',
                    text: 'ФИО нарушителя',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 1,
                    text: 'Организация',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 80,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    width: 50,
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    width: 80,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        return Ext.Date.format(new Date(val), 'd.m.Y');
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'InLawDate',
                    text: 'Дата вступления в силу',
                    width: 80,
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sanction',
                    flex: 1,
                    text: 'Вид санкции',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PenaltyAmount',
                    width: 70,
                    text: 'Сумма штрафа',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNoNotSet',
                    dataIndex: 'Paided',
                    text: 'Штраф оплачен',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DueDate',
                    text: 'Оплатить до',
                    width: 80,
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumPays',
                    width: 70,
                    text: 'Оплачено штрафов',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PaymentDate',
                    text: 'Дата оплаты',
                    width: 80,
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ArticleLaw',
                    width: 150,
                    text: 'Статьи закона',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Protocol205Date',
                    text: 'Дата неоплаты',
                    width: 80,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    format: 'd.m.Y',
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
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Выгрузка в Excel',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    xtype: 'button',
                                    name: 'resolutionoperation',
                                    text: 'Другие операции',
                                    iconCls: 'icon-cog-go',
                                    menu: []
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    action: 'ResolutionPrint',
                                    name: 'ResolutionPrint',
                                    itemId: 'btnResolutionPrint'
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