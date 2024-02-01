Ext.define('B4.view.asfk.BDOPEREditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.bdopereditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    height: 500,
    width: 800,
    itemId: 'bdoperEditWindow',
    title: 'Операция',
    closeAction: 'hide',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.asfk.ListResolutionsForSelected',
        'B4.store.asfk.BDOPER',
        'B4.view.asfk.BDOPERGrid',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.enums.ASFKADBDocCode',
        'B4.store.asfk.ASFK'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    margin: '10 0 10 0',
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Данные операции',
                            margin: '0 10 0 10',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'GUID',
                                    fieldLabel: 'ГУИД операции'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Sum',
                                    hideTrigger: true,
                                    fieldLabel: 'Сумма платежа'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Purpose',
                                    hideTrigger: true,
                                    fieldLabel: 'Данные о назначении платежа'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NamePay',
                                    hideTrigger: true,
                                    fieldLabel: 'Данные о плательщике'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'InnPay',
                                    hideTrigger: true,
                                    fieldLabel: 'ИНН плательщика'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'KppPay',
                                    hideTrigger: true,
                                    fieldLabel: 'КПП плательщика'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Тип операции',
                                    enumName: 'B4.enums.ASFKADBDocCode',
                                    name: 'KodDocAdb'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Kbk',
                                    enforceMaxLength: true,
                                    maxLength: 20,
                                    hideTrigger: true,
                                    fieldLabel: 'КБК'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ASFK',
                                    store: 'B4.store.asfk.ASFK',
                                    textProperty: 'GuidVT',
                                    fieldLabel: 'Связанная выписка',
                                    editable: false,
                                    columns: [
                                        {
                                            text: 'GUID',
                                            flex: 1,
                                            dataIndex: 'GuidVT',
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            text: 'Дата выписки',
                                            flex: 1,
                                            format: 'd.m.Y',
                                            dataIndex: 'DateOtch',
                                            filter: {
                                                xtype: 'datefield',
                                                format: 'd.m.Y'
                                            }
                                        },
                                        {
                                            text: 'Сумма',
                                            flex: 1,
                                            dataIndex: 'SumInItogV',
                                            filter: { xtype: 'textfield' }
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Связанное постановление',
                            margin: '0 10 0 10',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'asfkresolutiongrid'
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