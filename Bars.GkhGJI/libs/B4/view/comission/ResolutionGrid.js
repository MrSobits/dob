Ext.define('B4.view.comission.ResolutionGrid', {
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
        'B4.enums.TypeBase'
    ],

    title: 'Постановления',
    store: 'comission.ListResolutions',
    itemId: 'comissionResolutionGrid',
    alias: 'widget.comissionresolutiongrid',
    closable: false,
    enableColumnHide: true,

    initComponent: function () {
        //var me = this;

        var me = this;

        var renderer = function (val, meta, rec) {

            var deliveryDate = rec.get('DeliveryDate');
            var paided = rec.get('Paided');

            var deltaDate = ((new Date()).getTime() - (new Date(deliveryDate)).getTime()) / (1000 * 60 * 60 * 24);
            if (deliveryDate && deltaDate > 30 && paided == 20) {
                meta.style = 'background: red;';
                meta.tdAttr = 'data-qtip="Истек срок оплаты штрафа"';

            }
            else if (deliveryDate && deltaDate > 10 && paided == 20) {
                meta.style = 'background: yellow;';
                meta.tdAttr = 'data-qtip="Истекает срок оплаты штрафа"';
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
                    xtype: 'actioncolumn',
                    width: 25,
                    items: [{
                        tooltip: 'Переход к постановлению',
                        icon: B4.Url.content('content/img/icons/arrow_right.png'),
                        handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                            debugger;
                            me.fireEvent('gotoresolution', rec);
                        },
                    }]
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeBase',
                    width: 150,
                    //Основание проверки
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
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeInitiativeOrgGji',
                    dataIndex: 'TypeInitiativeOrg',
                    text: 'Кем вынесено',
                    flex: 0.5,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Official',
                    flex: 1,
                    text: 'ДЛ, вынесшее постановление',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityId',
                    flex: 1,
                    text: 'Муниципальное образование',
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        return rec.get('MunicipalityNames');
                    },
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
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
                                    action: 'ResolutionPrint',
                                    name: 'ResolutionPrint',
                                    itemId: 'btnResolutionPrint'
                                },
                                {
                                    xtype: 'button',
                                    name: 'resolutionoperation',
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