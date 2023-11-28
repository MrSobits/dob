Ext.define('B4.view.basestatement.AddWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.store.Contragent',
        'B4.view.Control.GkhTriggerField'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    minHeight: 180,
    maxHeight: 230,
    bodyPadding: 5,
    itemId: 'baseStatementAddWindow',
    title: 'Проверка по обращению граждан',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                anchor: '100%',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'appealCitizens',
                    itemId: 'trigfAppealCitizens',
                    fieldLabel: 'Обращение(я)'
                },
                {
                    xtype: 'combobox',
                    name: 'PersonInspection',
                    fieldLabel: 'Объект проверки',
                    displayField: 'Display',
                    store: B4.enums.PersonInspection.getStore(),
                    valueField: 'Value',
                    itemId: 'cbPersonInspection',
                    editable: false
                },
                {
                    xtype: 'combobox',
                    name: 'TypeJurPerson',
                    fieldLabel: 'Тип контрагента',
                    displayField: 'Display',
                    store: B4.enums.TypeJurPerson.getStore(),
                    valueField: 'Value',
                    itemId: 'cbTypeJurPerson',
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    itemId: 'sfContragent',
                    fieldLabel: 'Юридическое лицо',
                    store: 'B4.store.Contragent',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальное образование',
                            dataIndex: 'Municipality',
                            flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListWithoutPaging'
                            }
                        },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'PhysicalPerson',
                    fieldLabel: 'ФИО',
                    itemId: 'tfPhysicalPerson',
                    maxLength: 300,
                    disabled: true
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