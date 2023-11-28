Ext.define('B4.view.resolution.action.SumAmountWindow', {
    extend: 'B4.view.resolution.action.BaseResolutionWindow',

    alias: 'widget.sumamountwindow',

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
    title: 'Штраф',
    closeAction: 'destroy',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    bodyPadding: 0,
    border: null,
    accountOperationCode: 'SumAmountOperation',
    resolutionIds: null,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    flex: 0.7,
                    title: 'Сумма штрафа',
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
                                readOnly: true
                            },
                            items:[
                                {
                                    xtype: 'textfield',
                                    name: 'PenaltyAmount',
                                    maxLength: 200,
                                    fieldLabel: 'Сумма штрафа',
                                    readOnly: false
                                    
                                },
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
                amount: me.down('[name=PenaltyAmount]').getValue(),
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
            