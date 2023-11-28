Ext.define('B4.view.protocol197.MassAdditionWindow', {
    extend: 'Ext.Window',
    alias: 'widget.protocol197massaddwindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.ComboBox',
        'B4.store.dict.municipality.MoArea',
        'B4.store.dict.municipality.Settlement'
    ],

    width: 400,
    title: 'Массовое добавление приложений',
    layout: 'fit',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            items: [
                {
                    xtype: 'form',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    padding: '5 5 0 5',
                    defaults: {
                        labelWidth: 120,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: 2,
                            style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                            html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите тип приложения, заполните параметры и укажите количество экземпляров</span>'
                        },
                        {
                            xtype: 'fieldset',
                            padding: '5 5 0 5',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    anchor: '100%',
                                    fieldLabel: 'Тип документа',
                                    enumName: 'B4.enums.TypeAnnex',
                                    name: 'TypeAnnex'
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbIsProof',
                                    name: 'IsProof',
                                    fieldLabel: 'Доказательство',
                                    disabled: false,
                                    editable: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Count',
                                    itemId: 'nfCount',
                                    fieldLabel: 'Количество элементов',
                                    allowDecimals: false,
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false
                                }
                                
                            ]
                        }
                    ]
                }
            ],
            tbar: {
                items: [
                    {
                        xtype: 'b4savebutton',
                        text: 'Продолжить',
                        tooltip: 'Продолжить'
                    },
                    { xtype: 'tbfill' },
                    { xtype: 'b4closebutton' }
                ]
            }
        });
        me.callParent(arguments);
    }
});