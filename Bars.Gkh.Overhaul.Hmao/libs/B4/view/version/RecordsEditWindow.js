Ext.define('B4.view.version.RecordsEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.version.Stage1RecordsGrid',
        'B4.view.version.OwnerDecisionForm'
    ],
    alias: 'widget.versionrecordseditwin',
    title: 'Изменить номер очередности',
    modal: true,
    bodyPadding: '5 5 0 0',
    width: 500,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: false,
    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            defaults: {
                padding: '0 5 0 5',
                labelAlign: 'right',
                labelWidth: 170
            },
            items: [
                {
                    xtype: 'numberfield',
                    padding: '5 5 0 5',
                    hideTrigger: true,
                    allowDecimals: false,
                    minValue: 2013,
                    maxValue: 2100,
                    fieldLabel: 'Плановый год',
                    allowBlank: false,
                    name: 'Year'
                },
                {
                    xtype: 'checkbox',
                    fieldLabel: 'Пересчитать стоимость работ',
                    name: 'ReCalcSum'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    allowDecimals: false,
                    name: 'IndexNumber',
                    fieldLabel: 'Текущий номер',
                    readOnly: true
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    allowDecimals: false,
                    minValue: 1,
                    name: 'NewIndexNumber',
                    fieldLabel: 'Новый номер'
                },
                {
                    xtype: 'versionownerdecisionform',
                    padding: 5
                },
                {
                    xtype: 'stage1recordsgrid',
                    flex: 1
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
