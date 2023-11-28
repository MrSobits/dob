Ext.define('B4.view.resolution.RequisitePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.resolutionRequisitePanel',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.dict.Inspector',
        'B4.store.dict.SanctionGji',
        'B4.store.Contragent',
        'B4.store.dict.MunicipalitySelectTree',
        'B4.store.dict.MunicipalityTree',
        'B4.store.dict.municipality.ListAllWithParent',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeTerminationBasement',
        'B4.form.TreeSelectField',
        'B4.store.dict.JurInstitution',
        'B4.store.dict.ConcederationResult',
        'B4.enums.OSPDecisionType'
    ],
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            title: 'Реквизиты',
            bodyPadding: 5,
            autoScroll: true,
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 140,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'resolutionBaseName',
                            itemId: 'tfBaseName',
                            fieldLabel: 'Документ-основание',
                            readOnly: true
                        }
         
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 0 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 140,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            labelAlign: 'right',
                            name: 'ConcederationResult',
                            itemId: 'sfConcederationResult',
                            fieldLabel: 'Результат рассмотрения',
                            store: 'B4.store.dict.ConcederationResult',
                            readOnly: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            flex: 1
                        },
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 0 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 140,
                        labelAlign: 'right'

                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'OffenderWas',
                            fieldLabel: 'Нарушитель явился на рассмотрение',
                            displayField: 'Display',
                            store: B4.enums.YesNoNotSet.getStore(),
                            valueField: 'Value',
                            itemId: 'offenderWas',
                            editable: false,
                            value: 30,
                            width: 230

                        }
                    ]
                },
                {
                    xtype: 'tabtextarea',
                    labelWidth: 140,
                    fieldLabel: 'Обстоятельства дела',
                    name: 'AdditionalInfo',
                    itemId: 'taAdditionalInfo',
                    maxLength: 2000
                },
                {
                    padding: '5 0 5 0',
                    xtype: 'textarea',
                    labelAlign: 'right',
                    name: 'Comment',
                    fieldLabel: 'Основание прекращения',
                    labelWidth: 140,
                    itemId: 'taComment',
                    maxLength: 3000
                },
                {
                    xtype: 'fieldset',
                    hidden: true,
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        anchor: '100%'
                    },
                    title: 'Кем вынесено',
                    items: [
                        {
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    name: 'TypeInitiativeOrg',
                                    fieldLabel: 'Кем вынесено',
                                    displayField: 'Display',
                                    store: B4.enums.TypeInitiativeOrgGji.getStore(),
                                    valueField: 'Value',
                                    itemId: 'cbTypeInitOrg',
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'SectorNumber',
                                    fieldLabel: 'Номер участка',
                                    itemId: 'tfSectorNumber',
                                    maxLength: 250
                                }
                            ]
                        },
                        {
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                //{
                                //    xtype: 'treeselectfield',
                                //    name: 'FineMunicipality',
                                //    itemId: 'tsfFineMunicipality',
                                //    fieldLabel: 'МО получателя штрафа',
                                //    titleWindow: 'Выбор муниципального образования',
                                //    allowBlank: true,
                                //    store: 'B4.store.dict.MunicipalitySelectTree',
                                //    editable: false
                                //},
                                //{
                                //    xtype: 'component'
                                //}

                            ]
                        },
                        {
                            defaults: {
                                xtype: 'b4selectfield',
                                editable: false,
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    store: 'B4.store.dict.Inspector',
                                    textProperty: 'Fio',
                                    name: 'Official',
                                    fieldLabel: 'Должностное лицо',
                                    allowBlank: true,
                                    itemId: 'sfOfficial',
                                    columns: [
                                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    dockedItems: [
                                        {
                                            xtype: 'b4pagingtoolbar',
                                            displayInfo: true,
                                            store: 'B4.store.dict.Inspector',
                                            dock: 'bottom'
                                        }
                                    ]
                                },
                                {
                                    store: 'B4.store.dict.Municipality',
                                    textProperty: 'Name',
                                    name: 'Municipality',
                                    fieldLabel: 'Местонахождение',
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                        { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'Okato', flex: 1 },
                                        { header: 'ОКТМО', xtype: 'gridcolumn', dataIndex: 'Oktmo', flex: 1 }
                                    ],
                                    itemId: 'sfMunicipality'
                                }
                            ]
                        },
                        {
                            padding: '5 0 5 0',
                            xtype: 'textarea',
                            labelAlign: 'right',
                            name: 'Description',
                            fieldLabel: 'Состав АП',
                            labelWidth: 130,
                            flex: 1,
                            itemId: 'taDescription',
                            maxLength: 30000
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    itemId: 'fsJudicalOffice',
                    hidden:true,
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        anchor: '100%'
                    },
                    title: 'Судебное решение',
                    items: [
                        {
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                 {
                                    xtype: 'b4selectfield',
                                    name: 'IndividualPerson',
                                    fieldLabel: 'Нарушитель',
                                    store: 'B4.store.dict.IndividualPersonToResolution',
                                    textProperty: 'Fio',
                                    editable: false,
                                    flex: 1,
                                    itemId: 'dfIndividualPerson',
                                    allowBlank: true,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Дата рождения', dataIndex: 'DateBirth', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Место рождения', dataIndex: 'BirthPlace', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Актуальое место жительства', dataIndex: 'ActuallyResidence', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Номер паспорта', dataIndex: 'PassportNumber', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Серия паспорта', dataIndex: 'PassportSeries', flex: 1, filter: { xtype: 'textfield' } },
                                    ]
                                 },
                                 {
                                     xtype: 'textfield',
                                     name: 'DecisionNumber',
                                     itemId: 'tfDecisionNumber',
                                     fieldLabel: 'Номер решения судебного участка',
                                     allowBlank: true,
                                     flex: 1
                                 },
                                {
                                    xtype: 'datefield',
                                    name: 'DecisionDate',
                                    labelWidth: 200,
                                    itemId: 'dfDecisionDate',
                                    width: 300,
                                    allowBlank: true,
                                    fieldLabel: 'Дата решения',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    labelAlign: 'right',
                                    name: 'JudicalOffice',
                                    itemId: 'sfJudicalOffice',
                                    fieldLabel: 'Судебный участок',
                                    store: 'B4.store.dict.JurInstitution',
                                    readOnly: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } } 
                                    ],
                                    flex: 1
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DecisionEntryDate',
                                    labelWidth: 200,
                                    width: 300,
                                    allowBlank: true,
                                    fieldLabel: 'Дата вступления в законную силу',
                                    format: 'd.m.Y'
                                }

                            ]
                        },
                          {
                              xtype: 'textfield',
                              name: 'Violation',
                              fieldLabel: 'Нарушение',
                              allowBlank: true,
                              flex: 1
                          },
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        anchor: '100%',
                        flex: 1
                    },
                    title: 'Санкция',
                    items: [
                        {
                            defaults: {
                                allowBlank: true,
                                editable: false,
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'Sanction',
                                    fieldLabel: 'Вид санкции',
                                    fields: ['Id', 'Name', 'Code'],
                                    url: '/SanctionGji/List',
                                    itemId: 'cbSanction'
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'Paided',
                                    fieldLabel: 'Штраф оплачен',
                                    displayField: 'Display',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    valueField: 'Value',
                                    itemId: 'cbPaided'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'Protocol205Date',
                                    itemId: 'dfProtocol205Date',
                                    flex: 0.5,
                                    allowBlank: true,
                                    hidden:true,
                                    fieldLabel: 'Дата неоплаты',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'PaymentDate',
                                    itemId: 'dfPaymentDate',
                                    flex: 0.5,
                                    allowBlank: true,
                                    hidden: true,
                                    fieldLabel: 'Дата оплаты',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            padding: '10 0 5 0',
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'treeselectfield',
                                    name: 'FineMunicipality',
                                    itemId: 'tsfFineMunicipality',
                                    fieldLabel: 'МО получателя штрафа',
                                    titleWindow: 'Выбор муниципального образования',
                                    allowBlank: true,
                                    store: 'B4.store.dict.MunicipalitySelectTree',
                                    editable: false
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'PenaltyAmount',
                                    fieldLabel: 'Сумма штрафа',
                                    itemId: 'nfPenaltyAmount'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'PenaltyAmountByCourt',
                                    fieldLabel: 'Сумма по суду',
                                    itemId: 'nfPenaltyAmountByCourt'
                                }
                            ]
                        },
                        {
                            padding: '5 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    itemId: 'dfSentToOSP',
                                    name: 'SentToOSP',
                                    flex: 0.25,
                                    fieldLabel: 'Направлено приставам',
                                    allowBlank: true
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateTransferSsp',
                                    flex: 0.75,
                                    fieldLabel: 'Дата передачи в ССП',
                                    itemId: 'dfDateTransferSsp'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumSsp',
                                    fieldLabel: 'Номер документа переданного в ССП',
                                    itemId: 'tfDocumentNumSsp',
                                    flex: 1,
                                    maxLength: 300,
                                    labelAlign: 'right',
                                    labelWidth: 130
                                }
                            ]
                        },
{
                            defaults: {
                                padding: '5 0 5 0',
                                allowBlank: true,
                                editable: false,
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    name: 'OSPDecisionType',
                                    fieldLabel: 'Решение ССП',
                                    labelAlign: 'right',
                                    itemId: 'cbOSPDecisionType',
                                    store: B4.enums.OSPDecisionType.getStore(),
                                    valueField: 'Value',
                                    labelWidth: 130,
                                    flex: 1,
                                    displayField: 'Display',
                                    editable: false
                                },
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    store: 'B4.store.dict.JurInstitution',
                                    textProperty: 'ShortName',
                                    name: 'OSP',
                                    flex: 1,
                                    fieldLabel: 'Отдел ССП',
                                    labelWidth: 130,
                                    itemId: 'sfSSP',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальное образование',
                                            dataIndex: 'Municipality',
                                            flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        }
                                    ],
                                    listeners: {
                                        'beforeload': function (store, operation) {
                                            operation.params['type'] = 20;
                                        }
                                    }
                                }
                            ]
                        },
                        {
                            defaults: {
                                padding: '5 0 5 0',
                                allowBlank: true,
                                editable: false,
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ExecuteSSPNumber',
                                    fieldLabel: 'Номер ИП',
                                    labelWidth: 130,
                                    flex: 1,
                                    itemId: 'tfExecuteSSPNumber',
                                    maxLength: 500
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateEndExecuteSSP',
                                    fieldLabel: 'Дата окончания ИП',
                                    itemId: 'dfDateEndExecuteSSP'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Документ выдан',
                    name: 'fsReciever',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                disbled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    itemId: 'cbExecutant',
                                    name: 'Executant',
                                    allowBlank: false,
                                    disabled: true,
                                    editable: false,
                                    fieldLabel: 'Тип исполнителя',
                                    fields: ['Id', 'Name', 'Code'],
                                    url: '/ExecutantDocGji/List',
                                    queryMode: 'local',
                                    triggerAction: 'all'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateWriteOut',
                                    fieldLabel: 'Дата выписки из ЕГРЮЛ',
                                    itemId: 'dfDateWriteOut'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                disbled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    store: 'B4.store.Contragent',
                                    disabled: true,
                                    textProperty: 'ShortName',
                                    name: 'Contragent',
                                    fieldLabel: 'Контрагент',
                                    itemId: 'sfContragent',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальное образование',
                                            dataIndex: 'Municipality',
                                            flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPerson',
                                    fieldLabel: 'Физическое лицо',
                                    disabled: true,
                                    itemId: 'tfPhysPerson',
                                    maxLength: 500
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                disabled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Surname',
                                    fieldLabel: 'Фамилия',
                                    itemId: 'tfSurname',
                                    disabled: true,
                                    maxLength: 255
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FirstName',
                                    fieldLabel: 'Имя',
                                    itemId: 'tfFirstName',
                                    disabled: true,
                                    maxLength: 255
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Patronymic',
                                    fieldLabel: 'Отчество',
                                    itemId: 'tfPatronymic',
                                    disabled: true,
                                    maxLength: 255
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Position',
                                    fieldLabel: 'Должность',
                                    itemId: 'tfPosition',
                                    maxLength: 255
                                }
                            ]
                        },                        
                        {
                            xtype: 'textarea',
                            name: 'PhysicalPersonInfo',
                            disabled: true,
                            fieldLabel: 'Реквизиты физ. лица',
                            itemId: 'taPhysPersonInfo',
                            maxLength: 500
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});