Ext.define('B4.view.asfk.ASFKPayFineWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.asfkpayfinewindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {type: 'vbox', align: 'stretch'},
    width: 500,
    itemId: 'asfkPayFineWindow',
    title: 'Выбор постановления',
    closeAction: 'hide',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.asfk.ListResolutionsForASFK',
        'B4.model.asfk.ASFKResolution',
        'B4.ux.button.Add',
        'B4.ux.button.Update'
    ],

    initComponent: function () {
        var me = this;
        listStore = Ext.create('B4.store.asfk.ListResolutionsForASFK', {
        });

        listStore.on('beforeload', me.onListStoreBeforeLoad, me);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    margin: '10 10 5 10',
                    layout: 'anchor',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right'         
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Resolution',
                            fieldLabel: 'Постановление',
                            store: listStore,
                            model: 'B4.model.asfk.ASFKResolution',
                            editable: false,
                            itemId: 'sfResolution',
                            allowBlank: false,
                            textProperty: 'DocumentNumber',
                            windowCfg: {
                                width: 1000
                            },
                            columns: [
                                {
                                    text: 'Номер документа',
                                    flex: 0.5,
                                    dataIndex: 'DocumentNumber',
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    type: 'datecolumn',
                                    text: 'Дата документа',
                                    flex: 0.5,
                                    dataIndex: 'DocumentDate',
                                    filter: { xtype: 'datefield', format: 'd.m.Y' },
                                    renderer: function (val) {
                                        if (val != null) {
                                            return Ext.Date.format(new Date(val), 'd.m.Y');
                                        }
                                        else {
                                            return '';
                                        }
                                    }
                                },
                                {
                                    type: 'datecolumn',
                                    text: 'Дата вступления в законную силу',
                                    flex: 0.5,
                                    dataIndex: 'InLawDate',
                                    filter: { xtype: 'datefield', format: 'd.m.Y' },
                                    renderer: function (val) {
                                        if (val != null) {
                                            return Ext.Date.format(new Date(val), 'd.m.Y');
                                        }
                                        else {
                                            return '';
                                        }
                                    }
                                },
                                {
                                    text: 'ФИО нарушителя',
                                    dataIndex: 'Fio',
                                    flex: 1.5,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Контрагент',
                                    dataIndex: 'ContragentName',
                                    flex: 1.5,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Сумма штрафа',
                                    dataIndex: 'PenaltyAmount',
                                    flex: 0.5,
                                    filter: { xtype: 'numberfield' }
                                },
                                {
                                    text: 'Комиссия',
                                    dataIndex: 'ComissionName',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        }
                    ],
                },
                {
                    xtype: 'checkbox',
                    margin: '0 10 10 10',
                    itemId: 'cbShowAllResolutions',
                    fieldLabel: 'Выводить все постановления',
                    labelWidth: 320,
                    labelAlign: 'right'
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
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'createPayFineBtn',
                                    text: 'Создать'
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
                                    xtype: 'button',
                                    iconCls: 'icon-decline',
                                    itemId: 'closeBtn',
                                    text: 'Закрыть'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    onListStoreBeforeLoad: function (store, operation) {
        var me = this,
            showAll = false;

        if (me.rendered) {
            showAll = me.down('#cbShowAllResolutions').getValue();
        }
        operation.params.showAll = showAll;
    }
});