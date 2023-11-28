Ext.define('B4.view.comission.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.comissioneditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 1200,
    minWidth: 800,
    height: 650,
    resizable: true,
    bodyPadding: 3,
    itemId: 'comissionEditWindow',
    title: 'Заседание комиссии',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [      
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',    
        'B4.view.comission.ProtocolGrid',
        'B4.view.comission.ResolutionGrid',
        'B4.view.comission.ResolutionDefinitionGrid',
        'B4.store.dict.ZonalInspection',
        'B4.view.comission.InspectorGrid',
        'B4.form.ComboBox',
        'B4.view.comission.SubpoenaGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelAlign: 'right'
                    },
                    title: 'Данные по комиссии',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '5',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ZonalInspection',
                                    itemId: 'sflZonalInspection',
                                    fieldLabel: 'Комиссия',
                                    store: 'B4.store.dict.ZonalInspection',
                                    allowBlank: false,
                                    editable: false,
                                    labelWidth: 110,
                                    textProperty: 'ZoneName',
                                    flex: 1
                                },                               
                                {
                                    xtype: 'textfield',
                                    name: 'ComissionName',
                                    allowBlank: true,
                                    hidden: true,
                                    flex: 1,
                                    labelWidth: 110,
                                    maxLength: 500,
                                    fieldLabel: 'Наименование'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'CommissionNumber',
                                    allowBlank: false,
                                  //  labelWidth: 56,
                                    labelWidth: 110,
                                    fieldLabel: 'Номер заседания',
                                    flex: 0.5,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'CommissionDate',
                                    allowBlank: false,
                                    labelWidth: 110,
                                    fieldLabel: 'Дата заседания',
                                    format: 'd.m.Y',
                                    flex: 0.5
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TimeStart',
                                    allowBlank: true,
                                    labelWidth: 50,
                                    fieldLabel: 'Начало',
                                    flex: 0.5,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TimeEnd',
                                    allowBlank: true,
                                    labelWidth: 70,
                                    fieldLabel: 'Окончание',
                                    flex: 0.5,
                                    maxLength: 50
                                },
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 110,
                                labelAlign: 'right'
                            },
                            items: [
                         
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Описание',
                                    maxLength: 1500,
                                    flex: 1,
                                    allowBlank: true
                                }
                            ]
                        }
                        
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'mkdlicrequestTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'comissioninspectorgrid',
                            disabled: true,
                            flex: 1
                        },
                        {
                            xtype: 'comissionprotocolgrid',
                            disabled: false,
                            flex: 1
                        },
                        {
                            xtype: 'comissionresolutiongrid',
                            disabled: false,
                            flex: 1
                        },
                        {
                            xtype: 'comissionresolutiondefinitiongrid',
                            disabled: false,
                            flex: 1
                        },
                        {
                            xtype: 'subpoenaGrid',
                            disabled: false,
                            flex: 1
                        }, 
                        //{
                        //    xtype: 'mkdLicrequestquerygrid',
                        //    flex: 1
                        //}    
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