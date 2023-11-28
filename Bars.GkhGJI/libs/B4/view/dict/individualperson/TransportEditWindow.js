Ext.define('B4.view.dict.individualperson.TransportEditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    height: 200,
    bodyPadding: 5,
    itemId: 'individualpersonTransportEditWindow',
    title: 'Транспортное средство',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'

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
                                labelWidth: 130,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'NameTransport',
                                    fieldLabel: 'Наименование т/с',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NamberTransport',
                                    fieldLabel: 'Гос. номер',
                                    maxLength: 1000,
                                    allowBlank: false,
                                },
                                
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 130,
                                        labelAlign: 'right',
                                        allowBlank: true,
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DataOwnerStart',
                                            fieldLabel: 'Дата с',
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DataOwnerEdit',
                                            labelWidth: 80,
                                            fieldLabel: 'Дата по',
                                            format: 'd.m.Y'
                                        }
                                    ]
                                }

                            ]
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