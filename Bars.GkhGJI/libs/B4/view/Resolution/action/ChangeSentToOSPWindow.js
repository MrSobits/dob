Ext.define('B4.view.resolution.action.ChangeSentToOSPWindow', {
    extend: 'B4.view.resolution.action.BaseResolution3Window',

    alias: 'widget.changesenttoospwindow',

    requires: [
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.grid.Panel',
        'B4.ux.grid.selection.CheckboxModel'
    ],

    modal: true,
    closable: false,
    maximized: false,
    width: 500,
    minWidth: 300,
    height: 150,
    minHeight: 150,
    title: 'Изменение "Направлено в ОСП"',
    closeAction: 'destroy',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    bodyPadding: 0,
    border: null,
    accountOperationCode: 'ChangeSentToOSPOperation',
    resolutionIds: null,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    flex: 0.7,
                    title: 'Направлено приставам',
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
                                labelWidth: 250,
                                labelAlign: 'right',
                                readOnly: false
                            },
                            items:[
                                {
                                    xtype: 'checkbox',
                                    name: 'SentToOSP',
                                    fieldLabel: 'Направлено приставам',
                                    allowBlank: true
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
                resolutionIds: Ext.JSON.encode(me.resolutionIds),
                sentToOSP: me.down('[name=SentToOSP]').getValue(),
            };

        return params;
    }    
});         
            