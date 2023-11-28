Ext.define('B4.view.dict.individualperson.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 750,
    height: 570,
    bodyPadding: 5,
    itemId: 'individualpersonEditWindow',
    title: 'Физические лица',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.form.FiasSelectAddress',
        'B4.TextValuesOverride',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.SocialStatus',
        'B4.enums.FamilyStatus',
        'B4.view.dict.individualperson.ResolutionGrid',
        'B4.view.dict.individualperson.TransportGrid',
        'B4.store.dict.PhysicalPersonDocType'
    ],

    initComponent: function () {
        var me = this;
     
        Ext.applyIf(me, {
            defaults: {
                flex: 1
            },
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            layout: 'anchor',
                            title: 'Основная информация', 
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                anchor: '100%',
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Fio',
                                    fieldLabel: 'ФИО',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Job',
                                    fieldLabel: 'Место работы',
                                    maxLength: 1000,
                                    allowBlank: true,
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhoneNumber',
                                    fieldLabel: 'Контактный телефон',
                                    maxLength: 20,
                                    allowBlank: true
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DateBirth',
                                            fieldLabel: 'Дата рождения',
                                            flex:1.5,
                                            allowBlank: false,
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'BirthPlace',
                                            fieldLabel: 'Место рождения',
                                            labelWidth: 110,
                                            maxLength: 500,
                                            flex: 2,
                                            allowBlank: false,
                                        }
                                    ]
                                },
                                {
                                    //Адрес места правонаружения
                                    xtype: 'b4fiasselectaddress',
                                    flex: 1.5,
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
                                },
                                {
                                    //Адрес места правонаружения
                                    xtype: 'b4fiasselectaddress',
                                    flex: 1.5,
                                    labelAlign: 'right',
                                    name: 'FiasFactAddress',
                                    itemId: 'protocolFiaFactAddressField',
                                    fieldLabel: 'Адрес фактического места жительства',
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
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox', 
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            name: 'IsPlaceResidenceOutState',
                                            itemId: 'cbIsPlaceResidenceOutState',
                                            fieldLabel: 'Регистрация за пределами субъекта'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'PlaceResidenceOutState',
                                            itemId: 'tfPlaceResidenceOutState',
                                            fieldLabel: 'Адрес регистрации',
                                            labelWidth: 100,
                                            hidden: true,
                                            maxLength: 500,
                                            flex: 2,
                                            allowBlank: true,
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            name: 'IsActuallyResidenceOutState',
                                            itemId: 'cbIsActuallyResidenceOutState',
                                            fieldLabel: 'Место фактического пребывания за пределами субъекта'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ActuallyResidenceOutState',
                                            itemId: 'tfActuallyResidenceOutState',
                                            fieldLabel: 'Адрес места жительства',
                                            labelWidth: 100,
                                            maxLength: 500,
                                            hidden: true,
                                            flex: 2,
                                            allowBlank: true,
                                        }
                                    ]
                                },
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
                                    xtype: 'b4selectfield',
                                    itemId: 'sfSocialStatus',
                                    name: 'SocialStatus',
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
                                    name: 'DependentsNumber',
                                    fieldLabel: 'Количество иждивенцев'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'INN',
                                    fieldLabel: 'ИНН',
                                    allowBlank: true,
                                    maxLength: 300,
                                    allowBlank: false,
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PassportIssued',
                                    fieldLabel: 'Паспорт выдан',
                                    allowBlank: false,
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'PassportSeries',
                                            fieldLabel: 'Серия паспорта',
                                            maxLength: 6,
                                            flex: 1,
                                            allowBlank: false
                                        }, 
                                        {
                                            xtype: 'textfield',
                                            name: 'PassportNumber',
                                            labelWidth: 100,
                                            fieldLabel: 'Номер паспорта',
                                            maxLength: 8,
                                            flex: 1,
                                            allowBlank: false,
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateIssue',
                                            flex: 1,
                                            labelWidth: 90,
                                            fieldLabel: 'Дата выдачи',
                                            allowBlank: false,
                                        }
                                    ]
                                },   
                                {
                                    xtype: 'textfield',
                                    margin: '5 0 0 0',
                                    name: 'DepartmentCode',
                                    fieldLabel: 'Код',
                                    allowBlank: false,
                                }
                            ]
                        },
                        {
                            xtype: 'individualpersonresolutiongrid',
                            flex: 1
                        },
                        {
                            xtype: 'individualpersontransportgrid',
                            flex: 1
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