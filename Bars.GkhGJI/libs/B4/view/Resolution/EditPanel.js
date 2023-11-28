Ext.define('B4.view.resolution.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'resolutionEditPanel',
    title: 'Постановление',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.dict.Inspector',
        'B4.store.dict.SanctionGji',
        'B4.store.Contragent',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.resolution.RequisitePanel',
        'B4.view.resolution.DisputeGrid',
        'B4.view.resolution.DefinitionGrid',
        'B4.view.resolution.PayFineGrid',
        'B4.view.resolution.AnnexGrid',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.enums.YesNoNotSet',
        'B4.view.GjiDocumentCreateButton'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 170,
                        border: false,
                        xtype: 'panel',
                        layout: 'hbox',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                labelAlign: 'right',
                                allowBlank: false
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false,
                                    labelWidth: 50,
                                    width: 200,
                                    itemId: 'documentDate'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxlength: 300,
                                    width: 295,
                                    allowBlank: true
                                }
                            ]
                        },
                        {
                            bodyStyle: Gkh.bodyStyle,                       
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'InLawDate',
                                    fieldLabel: 'Дата вступления в силу',
                                    format: 'd.m.Y',
                                    labelAlign: 'right',
                                    allowBlank: true,
                                    labelWidth: 150,
                                    width: 300,
                                    itemId: 'dfInLawDate'
                                },
                                {
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    name: 'GisUin',
                                    width: 300,
                                    itemId: 'tfGisUin',
                                    fieldLabel: 'УИН',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            padding: '5px 0px 5px 0px',
                            bodyStyle: Gkh.bodyStyle,
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DeliveryDate',
                                    labelAlign: 'right',
                                    fieldLabel: 'Дата вручения',
                                    format: 'd.m.Y',
                                    allowBlank: true,
                                    labelWidth: 150,
                                    width: 300,
                                    itemId: 'dfDeliveryDate'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'SendDate',
                                    fieldLabel: 'Дата отправки',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    allowBlank: true,
                                    labelWidth: 100,
                                    width: 300,
                                    itemId: 'dfPostSendDate'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'PostDeliveryDate',
                                    fieldLabel: 'Дата доставки',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    allowBlank: true,
                                    labelWidth: 100,
                                    width: 300,
                                    itemId: 'dfPostDeliveryDate'
                                },
                                {
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    name: 'PostGUID',
                                    labelAlign: 'right',
                                    width: 300,
                                    labelWidth: 180,
                                    itemId: 'tfPostGUID',
                                    fieldLabel: 'Почтовый идентификатор',
                                    //  readOnly: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'resolutionRequisitePanel',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionDisputeGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionDefinitionGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionPayFineGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionAnnexGrid',
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
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Перейти к судебной практике',
                                    iconCls: 'icon-application-go',
                                    textAlign: 'left',
                                    itemId: 'btnCourtPractice'
                                },
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