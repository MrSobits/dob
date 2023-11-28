﻿Ext.define('B4.view.disposal.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.disposaleditpanel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'disposalEditPanel',
    title: '',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.Inspector',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.Control.GkhTriggerField',
        'B4.view.disposal.TypeSurveyGrid',
        'B4.view.disposal.AnnexGrid',
        'B4.view.disposal.ExpertGrid',
        'B4.view.disposal.ProvidedDocGrid',
        'B4.enums.TypeAgreementResult',
        'B4.enums.TypeAgreementProsecutor',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton'
    ],

    initComponent: function () {
        var me = this;

        me.title = B4.DisposalTextValues.getSubjectiveCase();

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    defaults: {
                        xtype: 'container',
                        padding: 5,
                        border: false
                    },
                    items: [
                        {
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 50,
                                    width: 200,
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 140,
                                    width: 295,
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300
                                }
                            ]
                        },
                        {
                            layout: 'hbox',
                            defaults: {
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    width: 200,
                                    name: 'DocumentYear',
                                    itemId: 'nfDocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 50,
                                    hideTrigger: true
                                },
                                {
                                    width: 295,
                                    name: 'DocumentNum',
                                    itemId: 'nfDocumentNum',
                                    fieldLabel: 'Номер',
                                    labelWidth: 140,
                                    hideTrigger: true
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
                    itemId: 'disposalTabPanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Реквизиты',
                            bodyStyle: Gkh.bodyStyle,
                            bodyPadding: 8,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            flex: .7,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right',
                                                readOnly: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'disposalBaseName',
                                                    itemId: 'tfBaseName',
                                                    fieldLabel: 'Основание обследования'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'disposalPlanName',
                                                    itemId: 'tfPlanName',
                                                    fieldLabel: 'Документ основания'
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'KindCheck',
                                                    fieldLabel: 'Вид проверки',
                                                    displayField: 'Name',
                                                    url: '/KindCheckGji/List',
                                                    valueField: 'Id',
                                                    itemId: 'cbTypeCheck',
                                                    readOnly: false,
                                                    editable: false
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        allowBlank: false,
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            name: 'DateStart',
                                                            itemId: 'dfDateStart',
                                                            fieldLabel: 'Период обследования с',
                                                            labelWidth: 170,
                                                            flex: 0.65
                                                        },
                                                        {
                                                            name: 'DateEnd',
                                                            itemId: 'dfDateEnd',
                                                            fieldLabel: 'по',
                                                            labelWidth: 50,
                                                            flex: 0.35
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            name: 'ObjectVisitStart',
                                                            itemId: 'dfObjectVisitStart',
                                                            fieldLabel: 'Выезд на объект с',
                                                            labelWidth: 170,
                                                            flex: 0.65
                                                        },
                                                        {
                                                            name: 'ObjectVisitEnd',
                                                            itemId: 'dfObjectVisitEnd',
                                                            fieldLabel: 'по',
                                                            labelWidth: 50,
                                                            flex: 0.35
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    boxLabel: 'Выезд инспектора в командировку',
                                                    name: 'OutInspector',
                                                    itemId: 'cbOutInspector',
                                                    padding: '0 0 0 175'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'Информация о согласовании с прокуратурой',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            items: [
                                                {  
                                                    xtype: 'container',
                                                    flex: 0.70,
                                                    layout: {
                                                         type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'combobox',
                                                            editable: false,
                                                            displayField: 'Display',
                                                            valueField: 'Value',
                                                            readOnly: false,
                                                            labelAlign: 'right',
                                                            name: 'TypeAgreementProsecutor',
                                                            itemId: 'cbTypeAgreementProsecutor',
                                                            fieldLabel: 'Согласование с прокуратурой',
                                                            store: B4.enums.TypeAgreementProsecutor.getStore(),
                                                            labelWidth: 170
                                                        }, {
                                                            xtype: 'combobox',
                                                            editable: false,
                                                            displayField: 'Display',
                                                            valueField: 'Value',
                                                            readOnly: false,
                                                            labelAlign: 'right',
                                                            name: 'TypeAgreementResult',
                                                            itemId: 'cbTypeAgreementResult',
                                                            fieldLabel: 'Результат согласования',
                                                            store: B4.enums.TypeAgreementResult.getStore(),
                                                            labelWidth: 170
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    flex: 1,
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch',
                                                        anchor: '100%'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'DocumentNumberWithResultAgreement',
                                                            itemId: 'cbDocumentNumberWithResultAgreement',
                                                            fieldLabel: 'Номер документа с результатом согласования',
                                                            labelAlign: 'right',
                                                            labelWidth: 300,
                                                            allowBlank: true
                                                        },
                                                        {
                                                            name: 'DocumentDateWithResultAgreement',
                                                            itemId: 'cbDocumentDateWithResultAgreement',
                                                            fieldLabel: 'Дата документа с результатом согласования',
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            labelAlign: 'right',
                                                            labelWidth: 300,
                                                            allowBlank: true,
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'Должностные лица',
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            textProperty: 'Fio',
                                            name: 'IssuedDisposal',
                                            fieldLabel: 'ДЛ, вынесшее ' + B4.DisposalTextValues.getSubjectiveCase(),
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
                                            ],
                                            itemId: 'sfIssuredDisposal',
                                            allowBlank: false,
                                            editable: false
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            name: 'ResponsibleExecution',
                                            itemId: 'sflResponsibleExecution',
                                            fieldLabel: 'Ответственный за исполнение',
                                            textProperty: 'Fio',
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
                                            ],
                                            editable: false
                                        },
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'disposalInspectors',
                                            itemId: 'trigFInspectors',
                                            fieldLabel: 'Инспекторы',
                                            allowBlank: false,
                                            editable: false
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'disposaltypesurveygrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalprovideddocgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalexpertgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalannexgrid',
                            flex: 1
                        }
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
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
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