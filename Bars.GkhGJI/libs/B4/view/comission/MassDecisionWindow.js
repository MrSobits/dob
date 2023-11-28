Ext.define('B4.view.comission.MassDecisionWindow', {
    extend: 'B4.view.comission.MassBaseWindow',

    alias: 'widget.comissionmassdecisionwindow',
    itemId: 'comissionmassdecisionwindow',

    requires: [
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.grid.Panel',

        'B4.form.EnumCombo',
        'B4.enums.ComissionDocumentDecision'
    ],

    modal: true,

    width: 500,
    title: 'Решение по протоколу',
    protocolIds: null,

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            layout: {
                type: 'form'
                //, align: 'stretch'
            },
            items: [{
                xtype: 'form',
                unstyled: true,
                border: false,
                layout: { type: 'vbox', align: 'stretch' },
                defaults: {
                    labelWidth: 80
                },
                items: [                    
                    {
                        xtype: 'b4enumcombo',
                        name: 'ComissionDocumentDecision',
                        itemId:'cbComissionDocumentDecision',
                        fieldLabel: 'Решение',
                        enumName: 'B4.enums.ComissionDocumentDecision',
                        allowBlank: false,
                    },
                    {
                        xtype: 'container',
                        margin: '0 0 0 0',
                        layout: 'hbox',
                        itemId: 'dfcntNextCommissionDate',
                        defaults: {
                            labelAlign: 'right'
                        },
                        items: [
                            {
                                xtype: 'datefield',
                                name: 'NextCommissionDate',
                                itemId: 'dfNextCommissionDate',
                                allowBlank: true,
                                labelWidth: 80,
                                fieldLabel: 'Дата',
                                format: 'd.m.Y',
                                flex: 0.5
                            },
                            {
                                xtype: 'numberfield',
                                name: 'HourOfProceedings',
                                itemId: 'nfHourOfProceedings',
                                margin: '0 0 0 10',
                                fieldLabel: '',
                                labelWidth: 25,
                                width: 45,
                                maxValue: 23,
                                minValue: 0
                            },
                            {
                                xtype: 'label',
                                text: ':',
                                margin: '5'
                            },
                            {
                                xtype: 'numberfield',
                                name: 'MinuteOfProceedings',
                                itemId: 'nfMinuteOfProceedings',
                                width: 45,
                                maxValue: 59,
                                minValue: 0
                            },                          
                        ]
                    },
                    {
                        xtype: 'tabtextarea',
                        padding: '5 0 0 0',
                        itemId: 'taDescription',
                        name: 'Description',
                        fieldLabel: 'Комментарий',
                        labelWidth: 80,
                        maxLength: 2000
                    }
                ]
            }]
        });
        me.callParent(arguments);
    }

});