Ext.define('B4.view.resolution.action.DateProtocolWindow', {
    extend: 'B4.view.resolution.action.BaseResolution2Window',

    alias: 'widget.resolutiondateprotocolwindow',

    requires: [
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.grid.Panel',
    ],

    modal: true,
    closable: false,
    maximized: false,
    width: 500,
    minWidth: 300,
    height: 150,
    minHeight: 150,
    title: 'Создание протоколов 20.25',
    closeAction: 'destroy',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    bodyPadding: 0,
    border: null,
    accountOperationCode: 'Create2025Operation',
    resolutionIds: null,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    flex: 0.7,
                    title: 'Дата рассмотрения',
                    bodyStyle: Gkh.bodyStyle,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items:[
                        {
                            xtype: 'form',
                            border: null,
                            bodyPadding: '10px 10px 0 10px',
                            bodyStyle: Gkh.bodyStyle,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                flex: 1,
                                labelWidth: 130,
                                labelAlign: 'right',
                                readOnly: false
                            },
                            items:[
                                {
                                    xtype: 'datefield',
                                    name: 'NextCommissionDate',
                                    itemId: 'dfNextCommissionDate',
                                    allowBlank: true,
                                    labelWidth: 80,
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    flex: 0.5
                                }
                            ]
                        },
                    ]
                },
            ],

            getForm: function() {
                return me.down('form');
            }
        });

        me.callParent(arguments);
    },

    getParams: function() {
        var me = this,
            params = {
                operationCode: me.accountOperationCode,
                accIds: Ext.JSON.encode(me.resolutionIds),
                amount: me.down('[name=NextCommissionDate]').getValue(),
            };

        return params;
    },
    //listeners: {
    //    beforeclose: function (win) {
    //        var closebutton = win.down('b4closebutton');
    //        click: function (closebutton) {

    //            closebutton.up('window').close();
    //        }
    //        debugger;
    //        Ext.Msg.confirm('Внимание',
    //            'Закрыть форму без сохранения изменений?',
    //            function(result) {
    //                if (result === 'yes') {
    //                    win.clearListeners(); // чистим все слушателей, чтобы опять сюда не попасть
    //                    win.close();
    //                }
    //            });

    //        return false;
    //    },
    //}       
});         
            