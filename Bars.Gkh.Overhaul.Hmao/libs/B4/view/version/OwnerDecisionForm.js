Ext.define('B4.view.version.OwnerDecisionForm', {
    extend: 'Ext.form.Panel',
    alias: 'widget.versionownerdecisionform',
    requires: [
        'B4.form.FileField'
    ],

    bodyStyle: Gkh.bodyStyle,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    border: false,

    items: [
        {
            xtype: 'fieldset',
            title: 'Основание',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            defaults: {
                labelAlign: 'right',
                labelWidth: 130
            },
            items: [
                {
                    xtype: 'checkbox',
                    fieldLabel: 'Есть решение собственников',
                    name: 'HasOwnerDecision',
                    labelWidth: 200,
                    padding: '0 0 10 0',
                    listeners: {
                        change: function(component, newValue, oldValue, eOpts) {
                            component.up('fieldset').down('b4filefield').allowBlank = !newValue;
                            component.up('form').getForm().isValid();
                        }
                    }
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentBase',
                    fieldLabel: 'Документ (основание)'
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentNumber',
                    fieldLabel: 'Номер документа'
                },
                {
                    xtype: 'datefield',
                    name: 'Date',
                    format: 'd.m.Y',
                    fieldLabel: 'Дата'
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Файл',
                    name: 'File'
                },
                {
                    xtype: 'textarea',
                    name: 'Remark',
                    fieldLabel: 'Примечание',
                    maxLength: 1000
                }
            ]
        }
    ]
});