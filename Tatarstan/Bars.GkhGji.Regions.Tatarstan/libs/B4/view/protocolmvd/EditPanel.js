﻿Ext.define('B4.view.protocolmvd.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'protocolMvdEditPanel',
    title: 'Протокол МВД',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.protocolmvd.AnnexGrid',
        'B4.view.protocolmvd.ArticleLawGrid',
        'B4.view.protocolmvd.RealityObjectGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.enums.TypeDocumentGji',
        'B4.store.dict.OrganMvd'
    ],

    initComponent: function () {
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
                                    width: 200,
                                    itemId: 'documentDate',
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxLength: 300,
                                    width: 295
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
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'protocolMvdTabPanel',
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
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    border: false,
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Municipality',
                                            name: 'Municipality',
                                            labelWidth: 250,
                                            fieldLabel: 'Муниципальное образование',
                                            editable: false,
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                            ],
                                            itemId: 'municipality',
                                            maxWidth: 700
                                        },
                                        {
                                            xtype: 'datefield',
                                            labelWidth: 280,
                                            name: 'DateOffense',
                                            fieldLabel: 'Дата правонарушения ',
                                            format: 'd.m.Y',
                                            itemId: 'dateOffense',
                                            maxWidth: 400
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
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.OrganMvd',
                                            name: 'OrganMvd',
                                            labelWidth: 250,
                                            fieldLabel: 'Орган МВД, оформивший протокол',
                                            editable: false,
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                            ],
                                            itemId: 'organMvd',
                                            maxWidth: 700
                                        },
                                        {
                                            fieldLabel: 'Время правонарушения',
                                            name: 'TimeOffense',
                                            xtype: 'textfield',
                                            regex: /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/,
                                            regexText: 'В это поле необходимо вводить время в формате 00:00',
                                            labelWidth: 280,
                                            maxWidth: 400,
                                            maskRe: /[0-9::]/i,
                                            itemId: 'timeOffense'
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
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            labelWidth: 250,
                                            name: 'DateSupply',
                                            fieldLabel: 'Дата поступления в ГЖИ',
                                            format: 'd.m.Y',
                                            itemId: 'dfDateSupplyProtocolMvd',
                                            maxWidth: 400
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 120,
                                        labelAlign: 'right'
                                    },
                                    title: 'Протокол составлен в отношении',
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
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            border: false,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                allowBlank: false,
                                                labelWidth: 200
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PhysicalPerson',
                                                    fieldLabel: 'ФИО нарушителя (полностью)',
                                                    itemId: 'tfPhysPerson'

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
                                                allowBlank: false,
                                                format: 'd.m.Y'

                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    labelWidth: 200,
                                                    name: 'BirthDate',
                                                    fieldLabel: 'Дата рождения',
                                                    width: 400,
                                                    itemId: 'birthDate'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    labelWidth: 610,
                                                    name: 'IssueDate',
                                                    fieldLabel: 'Дата выдачи',
                                                    width: 800,
                                                    itemId: 'issueDate'
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
                                                allowBlank: false,
                                                labelWidth: 200
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
                                                allowBlank: false,
                                                labelWidth: 200
                                            },
                                            items: [
                                                {
                                                    xtype: 'textarea',
                                                    name: 'PhysicalPersonInfo',
                                                    fieldLabel: 'Фактический адрес проживания',
                                                    itemId: 'taPhysPersonInfo'
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
                                }
                            ]
                        },
                        { xtype: 'protocolMvdArticleLawGrid', flex: 1 },
                        { xtype: 'protocolMvdRealityObjectGrid', flex: 1 },
                        { xtype: 'protocolMvdAnnexGrid', flex: 1 }
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
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                }
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