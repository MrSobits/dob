Ext.define('B4.view.dict.subpoena.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    height: 300,
    maxHeight: 300,
    width: 800,
    itemId: 'subpoenaeditwindow',
    title: 'Повестка на комиссию',
    closeAction: 'hide',

    border: false,
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.comission.ComissionMeeting'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 300
            },
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'panel',
                            layout: { type: 'vbox', align: 'stretch' },
                            border: false,
                            frame: true,
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 180
                            },
                            title: 'Повестка',
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateOfProceedings',
                                    fieldLabel: 'Дата',
                                    allowBlank: false,
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Name',
                                    fieldLabel: 'Наименование',
                                    allowBlank: false,
                                    maxLength: 2000
                                },
                                {
                                    xtype: 'b4selectfield',
                                    labelAlign: 'right',
                                    name: 'ComissionName',
                                    textProperty: 'ComissionName',
                                    itemId: 'sfComission',
                                    fieldLabel: 'Комиссия',
                                    store: 'B4.store.comission.ComissionMeeting',
                                    readOnly: true,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ComissionName', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                }
                            ]
                        },
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