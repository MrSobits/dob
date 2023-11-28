Ext.define('B4.view.asfk.ASFKEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.asfkeditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    height: 800,
    width: 1000,
    itemId: 'asfkEditWindow',
    title: 'Форма обмена с АСФК',
    closeAction: 'hide',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.asfk.ASFK',
        'B4.store.asfk.BDOPER',
        'B4.view.asfk.BDOPERGrid',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.enums.ASFKBudgetLevel',
        'B4.enums.ASFKADBDocCode',
        'B4.enums.ASFKConfirmingDocCode',
        'B4.enums.ASFKKBKType',
        'B4.enums.ASFKReportType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'column',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Данные обмена',
                                    margin: '10 10 0 10',
                                    columnWidth: 0.34,
                                    layout: 'anchor',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'NumVer',
                                            fieldLabel: 'Версионный номер'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Former',
                                            fieldLabel: 'Сформировано'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FormVer',
                                            fieldLabel: 'Версия формирующей программы'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NormDoc',
                                            fieldLabel: 'Нормативный документ'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Данные отправителя',
                                    margin: '10 10 0 10',
                                    columnWidth: 0.33,
                                    layout: 'anchor',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'KodTofkFrom',
                                            fieldLabel: 'Код ТОФК',
                                            maxLength: 4
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NameTofkFrom',
                                            fieldLabel: 'Наименование ТОФК',
                                            maxLength: 2000
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Данные получателя',
                                    margin: '10 10 0 10',
                                    columnWidth: 0.33,
                                    layout: 'anchor',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Уровень бюджета',
                                            enumName: 'B4.enums.ASFKBudgetLevel',
                                            name: 'BudgetLevel'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'KodUbp',
                                            fieldLabel: 'Код УБП',
                                            maxLength: 8
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NameUbp',
                                            fieldLabel: 'Наименование УБП',
                                            maxLength: 2000
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    margin: '10 0 10 0',
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Общие данные о выписке и итоговых суммах',
                            margin: '0 10 0 10',
                            layout: 'column',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    margin: '0 10 0 0',
                                    columnWidth: 0.5,
                                    layout: 'anchor',
                                    defaults: {
                                        labelWidth: 200,
                                        anchor: '100%',
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'GuidVT',
                                            fieldLabel: 'ГУИД',
                                            maxLength: 300
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateOtch',
                                            fieldLabel: 'Дата выписки',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Признак промежуточного отчёта',
                                            enumName: 'B4.enums.ASFKReportType',
                                            name: 'VidOtch'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NameTofkVT',
                                            fieldLabel: 'Наименование ТОФК',
                                            maxLength: 2000
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NameUbpAdb',
                                            fieldLabel: 'Администратор доходов бюджета',
                                            maxLength: 2000
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NameGadb',
                                            fieldLabel: 'Главный администратор доходов бюджета',
                                            maxLength: 2000
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Oktmo',
                                            fieldLabel: 'Код по ОКТМО',
                                            maxLength: 8
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NameFo',
                                            fieldLabel: 'Наименование финансового органа',
                                            maxLength: 2000
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NameIsp',
                                            fieldLabel: 'ФИО ответственного исполнителя',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DatePod',
                                            fieldLabel: 'Дата формирования',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'SumOutItogV',
                                            hideTrigger: true,
                                            fieldLabel: 'Итоговая сумма возвратов'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'SumNOutItogV',
                                            hideTrigger: true,
                                            fieldLabel: 'Итоговая сумма неисполненных возвратов'
                                        }
                                    ]
                                },
                                {        
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    layout: 'anchor',
                                    defaults: {
                                        labelWidth: 200,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'LsAdb',
                                            fieldLabel: 'Номер лицевого счета АДБ',
                                            maxLength: 11
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateOld',
                                            fieldLabel: 'Дата предыдущей выписки',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'KodTofkVT',
                                            fieldLabel: 'Код ТОФК',
                                            maxLength: 4
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'KodUbpAdb',
                                            fieldLabel: 'Код АДБ',
                                            maxLength: 8
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'KodGadb',
                                            fieldLabel: 'Код ГАДБ',
                                            maxLength: 3
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NameBud',
                                            fieldLabel: 'Наименование бюджета',
                                            maxLength: 512
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'OkpoFo',
                                            fieldLabel: 'Код по ОКПО',
                                            maxLength: 8
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DolIsp',
                                            fieldLabel: 'Должность ответственного исполнителя',
                                            maxLength: 100
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TelIsp',
                                            fieldLabel: 'Телефон ответственного исполнителя',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'SumInItogV',
                                            hideTrigger: true,
                                            fieldLabel: 'Итоговая сумма поступлений'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'SumZachItogV',
                                            hideTrigger: true,
                                            fieldLabel: 'Итоговая сумма зачетов'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'SumNZachItogV',
                                            hideTrigger: true,
                                            fieldLabel: 'Итоговая сумма неисполненных зачетов'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    margin: '10 0 10 0',
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Суммы поступлений, возвратов и зачетов',
                            margin: '0 10 0 10',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'column',
                                    items: [
                                        {
                                            xtype: 'container',
                                            margin: '0 10 0 0',
                                            columnWidth: 0.5,
                                            layout: 'anchor',
                                            defaults: {
                                                labelWidth: 200,
                                                anchor: '100%',
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumBeginIn',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма поступлений на начало дня'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumBeginOut',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма возвратов на начало дня'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumBeginZach',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма зачетов на начало дня'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumBeginNOut',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма неисполненных возвратов на начало дня'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumBeginNZach',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма неисполненных зачетов на начало дня'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            columnWidth: 0.5,
                                            layout: 'anchor',
                                            defaults: {
                                                labelWidth: 200,
                                                anchor: '100%',
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumEndIn',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма поступлений на конец дня'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumEndOut',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма возвратов на конец дня'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumEndZach',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма зачетов на конец дня'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumEndNOut',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма неисполненных возвратов на конец дня'
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'SumEndNZach',
                                                    hideTrigger: true,
                                                    fieldLabel: 'Сумма неисполненных зачетов на конец дня'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Данные по операциям',
                            margin: '0 10 0 10',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'bdopergrid'
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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