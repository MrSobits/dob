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
        'B4.enums.TypeInitiativeOrgGji',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeTerminationBasement',
        'B4.form.TreeSelectField'
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
                        },
                        {
                            xtype: 'datefield',
                            name: 'DeliveryDate',
                            fieldLabel: 'Дата вручения',
                            format: 'd.m.Y',
                            itemId: 'dfDeliveryDate',
                            //labelWidth: 130,
                            allowBlank: false
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
                            xtype: 'textfield',
                            name: 'GisUin',
                            itemId: 'tfGisUin',
                            fieldLabel: 'УИН',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'AbandonReason',
                            itemId: 'tfAbandonReason',
                            fieldLabel: 'Причина аннулирования'
                        }
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
                    xtype: 'fieldset',
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
                                {
                                    xtype: 'treeselectfield',
                                    name: 'FineMunicipality',
                                    itemId: 'tsfFineMunicipality',
                                    fieldLabel: 'МО получателя штрафа',
                                    titleWindow: 'Выбор муниципального образования',
                                    store: 'B4.store.dict.MunicipalitySelectTree',
                                    editable: false
                                },
                                {
                                    xtype: 'component'
                                }

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
                        }
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
                                allowBlank: false,
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
                                }
                            ]
                        },
                        {
                            padding: '5 0 5 0',
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'PenaltyAmount',
                                    fieldLabel: 'Сумма штрафа',
                                    itemId: 'nfPenaltyAmount'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateTransferSsp',
                                    fieldLabel: 'Дата передачи в ССП',
                                    itemId: 'dfDateTransferSsp'
                                }
                            ]
                        },
                        {
                            defaults: {
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 130
                            },
                            items: [
                                {
                                    padding: '5 0 5 0',
                                    xtype: 'combobox',
                                    name: 'TypeTerminationBasement',
                                    fieldLabel: 'Основание прекращения',
                                    itemId: 'cbTermination',
                                    store: B4.enums.TypeTerminationBasement.getStore(),
                                    valueField: 'Value',
                                    displayField: 'Display',
                                    editable: false
                                },
                                {
                                    xtype: 'container'
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumSsp',
                                    fieldLabel: 'Номер документа',
                                    itemId: 'tfDocumentNumSsp',
                                    maxLength: 300,
                                    flex: 0.5,
                                    labelAlign: 'right',
                                    labelWidth: 130
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
                                    itemId: 'tfPhysPerson',
                                    maxLength: 500
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'PhysicalPersonInfo',
                            fieldLabel: 'Реквизиты физ. лица',
                            itemId: 'taPhysPersonInfo',
                            maxLength: 500
                        }
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
                    title: 'Постановление вынесено в отношении',
                    name: 'fsPerson',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right',
                                disbled: true,
                                flex: 1,
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'combobox',
                                    itemId: 'cbExecutant',
                                    name: 'TypeExecutant',
                                    editable: false,
                                    fieldLabel: 'Тип исполнителя',
                                    displayField: 'Display',
                                    store: B4.enums.TypeExecutantProtocolMvd.getStore(),
                                    valueField: 'Value'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            border: false,
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                flex: 1,
                                labelWidth: 130,
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ProtocolMvdPhysicalPerson',
                                    fieldLabel: 'ФИО нарушителя (полностью)',
                                    itemId: 'protocolMvdPhysicalPerson',

                                },
                                {
                                    xtype: 'textfield',
                                    name: 'SerialAndNumber',
                                    fieldLabel: 'Серия и номер паспорта',
                                    itemId: 'serialAndNumber',
                                    maskRe: /[0-9 ]/i
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            border: false,
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                format: 'd.m.Y',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 130,
                                    name: 'BirthDate',
                                    fieldLabel: 'Дата рождения',
                                    width: 400,
                                    flex: 0.5,
                                    itemId: 'birthDate'
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 130,
                                    name: 'IssueDate',
                                    fieldLabel: 'Дата выдачи',
                                    width: 800,
                                    itemId: 'issueDate',
                                    flex: 0.5,
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            border: false,
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                flex: 1,
                                labelWidth: 130,
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'BirthPlace',
                                    fieldLabel: 'Место рождения',
                                    itemId: 'birthPlace'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'IssuingAuthority',
                                    fieldLabel: 'Кем выдан',
                                    itemId: 'issuingAuthority'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            border: false,
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                flex: 1,
                                labelWidth: 130,
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    name: 'ProtocolMvdPhysicalPersonInfo',
                                    fieldLabel: 'Фактический адрес проживания',
                                    itemId: 'protocolMvdPhysicalPersonInfo'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Company',
                                    fieldLabel: 'Место работы, должность',
                                    itemId: 'company'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 130,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Направление копии постановления',
                    name: 'fsCopy',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                disbled: true,
                                flex: 1,
                                labelWidth: 130,
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'RulinFio',
                                    fieldLabel: 'ФИО',
                                    itemId: 'rulinFio',
                                    maxLength: 150
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'RulingDate',
                                    fieldLabel: 'Дата',
                                    width: 400,
                                    itemId: 'rulingDate'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                flex: 0.5,
                                labelWidth: 130,
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'RulingNumber',
                                    fieldLabel: 'Номер',
                                    itemId: 'rulingNumber',
                                    maxLength: 50,
                                    maskRe: /[0-9]/i,
                                    width: 200
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