Ext.define('B4.view.protocol197.PetitionEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {type: 'vbox', align: 'stretch'},
    width: 600,
    bodyPadding: 5,
    itemId: 'protocol197PetitionEditWindow',
    title: 'Форма ходатайства',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.store.dict.Inspector',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.ComboBox',
        'B4.enums.YesNoNotSetPartially',
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'PetitionDate',
                    fieldLabel: 'Дата ходатайства',
                    format: 'd.m.Y',
                    labelWidth: 150
                },
                {
                    xtype: 'textfield',
                    name: 'PetitionAuthorFIO',
                    fieldLabel: 'ФИО',
                    flex: 1
                },
                {
                    xtype: 'textfield',
                    name: 'PetitionAuthorDuty',
                    fieldLabel: 'Должность',
                    flex: 1
                },
                {
                    xtype: 'textfield',
                    name: 'Workplace',
                    fieldLabel: 'Место работы',
                    flex: 1
                },
                {
                    xtype: 'textarea',
                    name: 'PetitionText',
                    fieldLabel: 'Текст ходатайства',
                    maxLength: 2000,
                    flex: 1
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'Aprooved',
                    fieldLabel: 'Решение',
                    itemId: 'ecAprooved',
                    width: 450,
                    minWidth: 450,
                    enumName: B4.enums.YesNoNotSetPartially
                },
                {
                    xtype: 'textarea',
                    name: 'PetitionDecisionText',
                    fieldLabel: 'Текст решения',
                    maxLength: 2000,
                    flex: 1
                },
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