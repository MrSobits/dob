Ext.define('B4.view.GisGmpParamsPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.gisgmpparamspanel',
    closable: true,
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    layout: { type: 'vbox', align: 'stretch' },
    title: 'Настройки интеграции ГИС ГМП',
    trackResetOnLoad: true,
    autoScroll: true,
    closeAction: 'hide',

    requires: [
        'B4.ux.button.Save',
        'B4.view.GisGmpPatternGrid'
    ],

    initComponent: function () {
        var me = this,
            vBox = { type: 'vbox', align: 'stretch' };

        Ext.applyIf(me, {
            defaults: {
                width: 600,
                maxWidth: 600,
                layout: vBox,
                defaults: {
                    labelAlign: 'right',
                    labelWidth: 150
                }
            },
            items: [
                {
                    xtype: 'checkbox',
                    name: 'GisGmpEnable',
                    fieldLabel: 'Интеграция включена',
                    labelAlign: 'right',
                    labelWidth: 160
                },
                {
                    xtype: 'fieldset',
                    title: 'Настройки прокси-сервера',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'GisGmpProxy',
                            fieldLabel: 'Адрес'
                        },
                        {
                            xtype: 'textfield',
                            name: 'GisGmpProxyUser',
                            fieldLabel: 'Логин'
                        },
                        {
                            xtype: 'textfield',
                            name: 'GisGmpProxyPassword',
                            fieldLabel: 'Пароль'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Общие параметры',
                    items: [
                        {
                            xtype: 'gisgmppatterngrid',
                            height: 200,
                            width: 250,
                            title: 'Коды шаблонов',
                        },
                        {
                            xtype: 'textfield',
                            name: 'GisGmpPatternCode',
                            fieldLabel: 'Стандартный код шаблона',
                            minLength: 0,
                            maxLength: 26,
                            allowBlank: false,
                            margins: '5 0 0 0'
                        },
                        {
                            xtype: 'textfield',
                            name: 'GisGmpSystemCode',
                            fieldLabel: 'Код системы'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Отправка начислений',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'GisGmpUriUpload',
                            fieldLabel: 'Адрес'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Загрузка оплат',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'GisGmpUriLoad',
                            fieldLabel: 'Адрес'
                        },
                        {
                            xtype: 'textfield',
                            name: 'GisGmpLoadTime',
                            fieldLabel: 'Время загрузки'
                        },
                        {
                            xtype: 'textfield',
                            name: 'GisGmpPayeeInn',
                            fieldLabel: 'ИНН получателя'
                        },
                        {
                            xtype: 'textfield',
                            name: 'GisGmpPayeeKpp',
                            fieldLabel: 'КПП получателя'
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
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});