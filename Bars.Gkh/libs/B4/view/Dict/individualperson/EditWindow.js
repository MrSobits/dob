Ext.define('B4.view.dict.individualperson.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    height: 600,
    bodyPadding: 5,
    itemId: 'individualpersonEditWindow',
    title: 'Нарушители',
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
        'B4.view.dict.individualperson.ResolutionGrid'
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
                                    xtype: 'container',
                                    padding: '5',
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
                                            allowBlank: false,
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'BirthPlace',
                                            fieldLabel: 'Место рождения',
                                            maxLength: 500,
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
                                    xtype: 'textfield',
                                    name: 'PassportNumber',
                                    fieldLabel: 'Номер паспорта',
                                    maxLength: 300,
                                    allowBlank: false,
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PassportSeries',
                                    fieldLabel: 'Серия паспорта',
                                    maxLength: 100,
                                    allowBlank: false
                                   // regex: /^([\w\-\'\-]+)(\.[\w\'\-]+)*@([\w\-]+\.){1,5}([A-Za-z]){2,4}$/
                                }, 
                                {
                                    xtype: 'textfield',
                                    margin: '5 0 0 0',
                                    name: 'DepartmentCode',
                                    fieldLabel: 'Код',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateIssue',
                                    fieldLabel: 'Дата выдачи паспорта',
                                    allowBlank: false
                                }
                            ]
                        },
                        {
                            xtype: 'individualpersonresolutiongrid',
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