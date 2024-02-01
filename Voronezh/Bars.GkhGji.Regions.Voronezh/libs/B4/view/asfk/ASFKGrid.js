Ext.define('B4.view.asfk.ASFKGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ASFKBudgetLevel',
        'B4.enums.ASFKADBDocCode',
        'B4.enums.ASFKConfirmingDocCode',
        'B4.enums.ASFKKBKType',
        'B4.enums.ASFKReportType'
    ],

    alias: 'widget.asfkgrid',
    store: 'asfk.ASFK',
    itemId: 'asfkGrid',
    title: 'АСФК',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        var renderer = function (val, meta, rec) {
            var distributedSum = rec.get('DistributedSum');
            var sumInItogV = rec.get('SumInItogV');

            if (sumInItogV == distributedSum) {
                meta.style = 'background: lightgreen;';
            }

            return val;
        };

        var renderBudgetLevel = function (val, meta, rec) {
            if (val == 1) {
                return 'Федеральный';
            }
            else if (val == 2) {
                return 'Субъекта РФ';
            }
            else if (val == 3) {
                return 'Местный';
            }
            else if (val == 4) {
                return 'ГВФ РФ';
            }
            else if (val == 5) {
                return 'ТГВФ';
            }
        }

        var renderVidOtch = function (val, meta, rec) {
            if (val == 0) {
                return 'Итоговый и/или нулевой';
            }
            else if (val == 1) {
                return 'Промежуточный';
            }
        }

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumVer',
                    text: 'Версионный номер',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Former',
                    text: 'Сформировано',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FormVer',
                    text: 'Версия формирующей программы',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NormDoc',
                    text: 'Нормативный документ',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KodTofkFrom',
                    text: 'Код ТОФК (отправитель)',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameTofkFrom',
                    text: 'Наименование ТОФК (отправитель)',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'BudgetLevel',
                    text: 'Уровень бюджета',
                    enumName: 'B4.enums.ASFKBudgetLevel',
                    filter: true,
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        return renderBudgetLevel(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KodUbp',
                    text: 'Код УБП',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameUbp',
                    text: 'Наименование УБП',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GuidVT',
                    text: 'ГУИД',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LsAdb',
                    text: 'Номер лицевого счета',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateOtch',
                    text: 'Дата выписки',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        return val ? Ext.Date.format(new Date(val), 'd.m.Y') : "";
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateOld',
                    text: 'Дата предыдущей выписки',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        return val ? Ext.Date.format(new Date(val), 'd.m.Y') : "";
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'VidOtch',
                    text: 'Признак промежуточного отчёта',
                    enumName: 'B4.enums.ASFKReportType',
                    filter: true,
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        return renderVidOtch(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KodTofkVT',
                    text: 'Код ТОФК (VT)',
                    hidden: true,
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameTofkVT',
                    text: 'Наименование ТОФК (VT)',
                    hidden: true,
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KodUbpAdb',
                    text: 'Код АДБ',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameUbpAdb',
                    text: 'Администратор доходов бюджета',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KodGadb',
                    text: 'Код ГАДБ',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameGadb',
                    text: 'Главный администратор доходов бюджета',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameBud',
                    text: 'Наименование бюджета',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Oktmo',
                    text: 'Код по ОКТМО',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OkpoFo',
                    text: 'Код по ОКПО',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameFo',
                    text: 'Наименование финансового органа',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DolIsp',
                    text: 'Должность ответственного исполнителя',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NameIsp',
                    text: 'ФИО ответственного исполнителя',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TelIsp',
                    text: 'Телефон ответственного исполнителя',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePod',
                    text: 'Дата формирования',
                    format: 'd.m.Y',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        return val ? Ext.Date.format(new Date(val), 'd.m.Y') : "";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumInItogV',
                    text: 'Итоговая сумма поступлений',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DistributedSum',
                    text: 'Распределенная сумма',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumOutItogV',
                    text: 'Итоговая сумма возвратов',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumZachItogV',
                    text: 'Итоговая сумма зачетов',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumNOutItogV',
                    text: 'Итоговая сумма неисполненных возвратов',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumNZachItogV',
                    text: 'Итоговая сумма неисполненных зачетов',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumBeginIn',
                    text: 'Сумма поступлений на начало дня',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumBeginOut',
                    text: 'Сумма возвратов на начало дня',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumBeginZach',
                    text: 'Сумма зачетов на начало дня',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumBeginNOut',
                    text: 'Сумма неисполненных возвратов на начало дня',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumBeginNZach',
                    text: 'Сумма неисполненных зачетов на начало дня',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumEndIn',
                    text: 'Сумма поступлений на конец дня',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumEndOut',
                    text: 'Сумма возвратов на конец дня',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumEndZach',
                    text: 'Сумма зачетов на конец дня',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumEndNOut',
                    text: 'Сумма неисполненных возвратов на конец дня',
                    hidden: true,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumEndNZach',
                    text: 'Сумма неисполненных зачетов на конец дня',
                    hidden: true,
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