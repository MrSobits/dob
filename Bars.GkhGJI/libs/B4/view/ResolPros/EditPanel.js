Ext.define('B4.view.resolpros.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'resolProsEditPanel',
    title: 'Постановление прокуратуры',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.DocumentGji',
        'B4.store.dict.ProsecutorOffice',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.resolpros.AnnexGrid',
        'B4.view.resolpros.ArticleLawGrid',
        'B4.view.resolpros.RealityObjectGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.enums.TypeDocumentGji',
        'B4.form.FiasSelectAddress',
        'B4.store.dict.SocialStatus',
        'B4.enums.FamilyStatus',
        'B4.ux.form.field.TabularTextArea'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    defaults: {
                        border: false,
                        labelWidth: 170,
                        xtype: 'panel',
                        layout: 'hbox',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            bodyStyle: Gkh.bodyStyle,
                            padding: '10px 15px 5px 15px',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false,
                                    labelWidth: 80,
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    readOnly: true,
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxLength: 300,
                                    width: 295
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'UIN',
                                    fieldLabel: 'УИН',
                                    labelWidth: 50,
                                    width: 350,
                                }
                            ]
                        },
                        {
                            bodyStyle: Gkh.bodyStyle,
                            padding: '0 15px 20px 15px',
                            defaults: {
                                xtype: 'gkhintfield',
                                hideTrigger: true
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    fieldLabel: 'Год',
                                    itemId: 'nfDocumentYear',
                                    labelWidth: 80,
                                    width: 200
                                },
                                {
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    itemId: 'nfDocumentNum',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                },
                                {
                                    name: 'LiteralNum',
                                    itemId: 'nfLiteralNum',
                                    fieldLabel: 'Буквенный подномер',
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    name: 'DocumentSubNum',
                                    itemId: 'nfDocumentSubNum',
                                    fieldLabel: 'Подномер',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'resolprosTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Реквизиты',
                            bodyPadding: 5,
                            border: false,
                            frame: true,
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right'
                            },
                            items: [
                                //{
                                //    xtype: 'b4selectfield',
                                //    store: 'B4.store.dict.Municipality',
                                //    name: 'Municipality',
                                //    labelWidth: 280,
                                //    fieldLabel: 'Орган прокуратуры, вынесший постановление',
                                //    editable: false,
                                //    allowBlank: false,
                                //    columns: [
                                //        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                //    ],
                                //    itemId: 'sfMunicipalityResolPros'
                                //},
                                {
                                    xtype: 'b4selectfield',
                                    store:   'B4.store.dict.ProsecutorOffice',
                                    name: 'ProsecutorOffice',
                                    labelWidth: 280,
                                    fieldLabel: 'Орган прокуратуры, вынесший постановление',
                                    editable: false,
                                    allowBlank: false,
                                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } } ],
                                    itemId: 'sfMunicipalityResolPros'
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    border: false,
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            labelWidth: 280,
                                            name: 'DateSupply',
                                            fieldLabel: 'Дата поступления',
                                            format: 'd.m.Y',
                                            itemId: 'dfDateSupplyResolPros',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'ActCheck',
                                            itemId: 'actCheckSelectField',
                                            fieldLabel: 'Акт проверки',
                                            labelWidth: 150,
                                            flex: 1.5,
                                            isGetOnlyIdProperty: false,
                                            editable: false,
                                            textProperty: 'DocumentNumber',
                                            store: 'B4.store.DocumentGji',
                                            columns: [
                                                { xtype: 'datecolumn', dataIndex: 'DocumentDate', text: 'Дата', format: 'd.m.Y', width: 100 },
                                                { text: 'Номер', dataIndex: 'DocumentNumber', flex: 1 },
                                                {
                                                    text: 'Тип документа',
                                                    dataIndex: 'TypeDocumentGji',
                                                    flex: 1,
                                                    renderer: function(val) { return B4.enums.TypeDocumentGji.displayRenderer(val); }
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'tabtextarea',
                                    labelWidth: 280,
                                    fieldLabel: 'Обстоятельства дела',
                                    name: 'AdditionalInfo',
                                    itemId: 'taAdditionalInfo',
                                    maxLength: 2000
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 120,
                                        labelAlign: 'right'
                                    },
                                    title: 'Постановление вынесено в отношении',
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
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.Contragent',
                                            textProperty: 'ShortName',
                                            name: 'Contragent',
                                            fieldLabel: 'Контрагент',
                                            itemId: 'sfContragent',
                                            disabled: true,
                                            editable: false,
                                            columns: [
                                                {
                                                    header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
                                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'КПП', xtype: 'gridcolumn', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                disabled: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPerson',
                                                    fieldLabel: 'Физическое лицо',
                                                    itemId: 'tfPhysPerson',
                                                    maxLength: 300,
                                                    labelWidth: 120
                                                },
                                                {
                                                    xtype: 'textarea',
                                                    name: 'PhysicalPersonInfo',
                                                    fieldLabel: 'Реквизиты физ. лица',
                                                    itemId: 'taPhysPersonInfo',
                                                    maxLength: 500,
                                                    labelWidth: 150
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                disabled: false,
                                                flex: 0.5
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPersonPosition',
                                                    fieldLabel: 'Должность',
                                                    itemId: 'tfdPhysicalPersonPosition',
                                                    maxLength: 300,
                                                    labelWidth: 120
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'PhysicalPersonDocType',
                                                    labelWidth: 150,
                                                    fieldLabel: 'Вид документа ФЛ',
                                                    store: 'B4.store.dict.PhysicalPersonDocType',
                                                    editable: false,
                                                    itemId: 'dfPhysicalPersonDocType',
                                                    allowBlank: true,
                                                    columns: [
                                                        { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 120,
                                                labelAlign: 'right',
                                                disabled: false,
                                                flex: 0.5
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPersonDocumentSerial',
                                                    itemId: 'dfPhysicalPersonDocumentSerial',
                                                    fieldLabel: 'Серия документа ФЛ',
                                                    allowBlank: true,
                                                    maxLength: 20
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPersonDocumentNumber',
                                                    itemId: 'dfPhysicalPersonDocumentNumber',
                                                    fieldLabel: 'Номер документа ФЛ',
                                                    allowBlank: true,
                                                    maxLength: 20
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    itemId: 'dfPhysicalPersonIsNotRF',
                                                    name: 'PhysicalPersonIsNotRF',
                                                    fieldLabel: 'Не является гражданином РФ',
                                                    labelWidth: 180,
                                                    allowBlank: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 120,
                                                flex: 0.5
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PassportIssued',
                                                    itemId: 'dfPassportIssued',
                                                    fieldLabel: 'Паспорт выдан',
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateIssue',
                                                    itemId: 'dfDateIssue',
                                                    fieldLabel: 'Дата выдачи',
                                                    allowBlank: true,
                                                    maxLength: 20
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DepartmentCode',
                                                    itemId: 'dfDepartmentCode',
                                                    fieldLabel: 'Код подразделения',
                                                    maxLength: 20
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 10 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    fieldLabel: 'Дата рождения',
                                                    labelWidth: 120,
                                                    name: 'DateBirth',
                                                    itemId: 'tfDateBirth',
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    fieldLabel: 'Место рождения',
                                                    labelWidth: 150,
                                                    name: 'BirthPlace',
                                                    itemId: 'tfBirthPlace',
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'IsPlaceResidenceOutState',
                                                    itemId: 'cbIsPlaceResidenceOutState',
                                                    flex: 0.5,
                                                    fieldLabel: 'Регистрация за пределами субъекта'
                                                },
                                                {
                                                    xtype: 'b4fiasselectaddress',
                                                    flex: 3,
                                                    labelWidth: 220,
                                                    labelAlign: 'right',
                                                    name: 'FiasRegistrationAddress',
                                                    itemId: 'protocolFiasRegistrationAddressField',
                                                    fieldLabel: 'Адрес регистрации нарушителя',
                                                    fieldsRegex: {
                                                        tfHousing: {
                                                            regex: /^\d+$/,
                                                            regexText: 'В это поле можно вводить только цифры'
                                                        },
                                                        tfBuilding: {
                                                            regex: /^\d+$/,
                                                            regexText: 'В это поле можно вводить только цифры'
                                                        }
                                                    }
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'IsActuallyResidenceOutState',
                                                    itemId: 'cbIsActuallyResidenceOutState',
                                                    flex: 0.5,
                                                    fieldLabel: 'Место фактического пребывания за пределами субъекта'
                                                },
                                                {
                                                    xtype: 'b4fiasselectaddress',
                                                    flex: 3,
                                                    labelAlign: 'right',
                                                    name: 'FiasFactAddress',
                                                    labelWidth: 220,
                                                    itemId: 'protocolFiaFactAddressField',
                                                    fieldLabel: 'Адрес факт. места жительства',
                                                    fieldsRegex: {
                                                        tfHousing: {
                                                            regex: /^\d+$/,
                                                            regexText: 'В это поле можно вводить только цифры'
                                                        },
                                                        tfBuilding: {
                                                            regex: /^\d+$/,
                                                            regexText: 'В это поле можно вводить только цифры'
                                                        }
                                                    }
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 120,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4enumcombo',
                                                    name: 'FamilyStatus',
                                                    fieldLabel: 'Семейное положение',
                                                    itemId: 'ecFamilyStatus',
                                                    width: 450,
                                                    minWidth: 450,
                                                    enumName: B4.enums.FamilyStatus
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Job',
                                                    itemId: 'tfJob',
                                                    labelWidth: 80,
                                                    flex: 1,
                                                    fieldLabel: 'Место работы',
                                                    maxLength: 1000,
                                                    allowBlank: true,
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhoneNumber',
                                                    itemId: 'tfPhoneNumber',
                                                    maxLength: 100,
                                                    flex: 1,
                                                    fieldLabel: 'Контактный телефон',
                                                    allowBlank: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 10 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 120,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    itemId: 'sfSocialStatus',
                                                    name: 'SocialStatus',
                                                    flex: 1,
                                                    fieldLabel: 'Социальный статус',
                                                    store: 'B4.store.dict.SocialStatus',
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    hideTrigger: true,
                                                    keyNavEnabled: false,
                                                    mouseWheelEnabled: false,
                                                    maxValue: 20,
                                                    flex: 1,
                                                    labelWidth: 150,
                                                    itemId: 'nfDependentsNumber',
                                                    name: 'DependentsNumber',
                                                    fieldLabel: 'Количество иждевенцев'
                                                },
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        { xtype: 'resolprosArticleLawGrid', flex: 1 },
                        { xtype: 'resolprosRealityObjectGrid', flex: 1 },
                        { xtype: 'resolprosAnnexGrid', flex: 1 }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGJIToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                //ToDo ГЖИ после перехода на правила необходимо удалить
                                /*
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Сформировать',
                                    itemId: 'btnCreateDocument',
                                    menu: [
                                        {
                                            text: 'Постановление',
                                            textAlign: 'left',
                                            itemId: 'btnCreateResolProsToResolution',
                                            actionName: 'createResolProsToResolution'
                                        }
                                    ]
                                }*/
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                }
                                /*, В постановлении прокуратуры неможет быть кнопки Удалить потмоу что оудаление произходит из реестра
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                }*/
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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