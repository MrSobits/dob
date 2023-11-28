Ext.define('B4.view.dict.erknm.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.municipality.Grid',
        'B4.store.CreditOrg',
        //   'B4.form.FiasSelectAddress',
        'B4.form.SelectField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'dictionaryERKNMEditWindow',
    title: 'Записи ЕРКНМ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DictionaryERKNMGuid',
                    flex: 1,
                    text: 'Гуид справочника ',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Название справочника ',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Type',
                    flex: 1,
                    text: 'Тип',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Order',
                    flex: 1,
                    text: 'Order',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Required',
                    flex: 1,
                    text: 'Required',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateLastUpdate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 80,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                }
                //{
                //    xtype: 'container',
                //    layout: 'hbox',
                //    defaults: {
                //        xtype: 'textfield',
                //        labelWidth: 170,
                //        labelAlign: 'right',
                //        flex: 1
                //    },
                //    items: [
                //        {
                //            xtype: 'textfield',
                //            name: 'Town',
                //            fieldLabel: 'Населенный пункт',
                //            maxLength: 100
                //        },
                //        {
                //            xtype: 'textfield',
                //            name: 'Street',
                //            fieldLabel: 'Улица',
                //            maxLength: 100
                //        }
                //    ]
                //},
                //{
                //    xtype: 'b4selectfield',
                //    store: 'B4.store.CreditOrg',
                //    textProperty: 'Name',
                //    name: 'CreditOrg',
                //    fieldLabel: 'Банк',
                //    editable: false,
                //    columns: [
                //        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                //        { header: 'ОКТМО', xtype: 'gridcolumn', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } }
                //    ],
                //    allowBlank: false
                //},
                //{
                //    xtype: 'container',
                //    layout: 'hbox',
                //    defaults: {
                //        xtype: 'textfield',
                //        labelWidth: 170,
                //        labelAlign: 'right',
                //        flex: 1
                //    },
                //    items: [
                //        {
                //            xtype: 'textfield',
                //            name: 'BankAccount',
                //            fieldLabel: 'Расчетный счет',
                //            maxLength: 100
                //        },
                //        {
                //            xtype: 'textfield',
                //            name: 'KBK',
                //            fieldLabel: 'КБК',
                //            maxLength: 100
                //        }
                //    ]
                //}
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