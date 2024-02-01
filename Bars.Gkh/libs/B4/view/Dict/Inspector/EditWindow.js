Ext.define('B4.view.dict.inspector.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    height: 430,
    bodyPadding: 5,
    itemId: 'inspectorEditWindow',
    title: 'Член комиссии',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.store.dict.Position',
        'B4.store.dict.ZonalInspection',
        'B4.view.dict.zonalinspection.Grid',
        'B4.view.Control.GkhTriggerField',
        'B4.TextValuesOverride',
        'B4.enums.TypeCommissionMember'
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
                                //{
                                //    xtype: 'textfield',
                                //    name: 'Id',
                                //    fieldLabel: 'Код',
                                //    maxLength: 300
                                //},
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'TypeCommissionMember',
                                    fieldLabel: 'Роль в комиссии',
                                    itemId: 'ecTypeCommissionМember',
                                    width: 450,
                                    minWidth: 450,
                                    enumName: B4.enums.TypeCommissionMember
                                },
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    name: 'NotMemberPosition',
                                    fieldLabel: 'Должность',
                                    store: 'B4.store.dict.Position',
                                    labelAlign: 'right',
                                    labelWidth: 150,
                                    flex: 1
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Fio',
                                    fieldLabel: 'ФИО',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ShortFio',
                                    fieldLabel: 'Фамилия И.О.',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsHead',
                                    fieldLabel: 'Начальник'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Phone',
                                    fieldLabel: 'Телефон',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Email',
                                    fieldLabel: 'Электронная почта',
                                    maxLength: 100,
                                    regex: /^([\w\-\'\-]+)(\.[\w\'\-]+)*@([\w\-]+\.){1,5}([A-Za-z]){2,4}$/
                                }, 
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'inspectorZoanalInsp',
                                    itemId: 'zonInspectorsTrigerField',
                                    fieldLabel: B4.TextValuesOverride.getText('Административная комиссия')
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Подразделение',
                                    maxLength: 500
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'Active',
                                    fieldLabel: 'Действует'
                                },
                              
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'inspectorZoanalInsp',
                                    itemId: 'zonInspectorsTrigerField',
                                    fieldLabel: B4.TextValuesOverride.getText('Комиссия')
                                },
                            ]
                        },
                        {
                            layout: 'anchor',
                            title: 'Падежи ФИО',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 100,
                                anchor: '100%',
                                maxLength: 300
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FioGenitive',
                                    fieldLabel: 'Родительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FioDative',
                                    fieldLabel: 'Дательный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FioAccusative',
                                    fieldLabel: 'Винительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FioAblative',
                                    fieldLabel: 'Творительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FioPrepositional',
                                    fieldLabel: 'Предложный'
                                }
                            ]
                        },
                        {
                            layout: 'anchor',
                            title: 'Падежи Должность',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 100,
                                anchor: '100%',
                                maxLength: 300
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PositionGenitive',
                                    fieldLabel: 'Родительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PositionDative',
                                    fieldLabel: 'Дательный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PositionAccusative',
                                    fieldLabel: 'Винительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PositionAblative',
                                    fieldLabel: 'Творительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PositionPrepositional',
                                    fieldLabel: 'Предложный'
                                }
                            ]
                        },
                        {
                            xtype: 'inspectorsubcriptiongrid',
                            flex: 1
                        },
                        {
                            xtype: 'inspectorzonalinspsubscriptiongrid',
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