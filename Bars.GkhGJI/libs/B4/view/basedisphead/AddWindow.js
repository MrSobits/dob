﻿Ext.define('B4.view.basedisphead.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minHeight: 300,
    maxHeight: 300,
    bodyPadding: 5,
    itemId: 'baseDispHeadAddWindow',
    title: 'Дело об административном правонарушении',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.Contragent',
        'B4.form.ComboBox',
        'B4.store.dict.Inspector',
        'B4.enums.TypeBaseDispHead',
        'B4.enums.TypeFormInspection',
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection'
    ],

    initComponent: function() {
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
                    xtype: 'b4selectfield',
                    name: 'Head',
                    itemId: 'sflHead',
                    fieldLabel: 'Проверяющий',
                    textProperty: 'Fio',
                   

                    store: 'B4.store.dict.Inspector',
                    columns: [
                        { text: 'ФИО', dataIndex: 'Fio', flex: 1 },
                        { text: 'Должность', dataIndex: 'Position', flex: 1 }
                    ],
                    editable: false
                },
                {
                    xtype: 'combobox',
                    itemId: 'cbTypeBase',
                    name: 'TypeBaseDispHead',
                    fieldLabel: 'Основание',
                    displayField: 'Display',
                    store: B4.enums.TypeBaseDispHead.getStore(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'combobox',
                    name: 'TypeForm',
                    itemId: 'cbTypeForm',
                    fieldLabel: 'Форма проверки',
                    displayField: 'Display',
                    store: B4.enums.TypeFormInspection.getStore(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'datefield',
                    name: 'DispHeadDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4combobox',
                    name: 'PersonInspection',
                    fieldLabel: 'Объект проверки',
                    displayField: 'Display',
                    itemId: 'cbPersonInspection',
                    editable: false,
                    storeAutoLoad: true,
                    valueField: 'Id',
                    url: '/Inspection/ListPersonInspection'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    textProperty: 'ShortName',
                    itemId: 'sfContragent',
                    fieldLabel: 'Юридическое лицо',
                    store: 'B4.store.Contragent',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальный район',
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
                                url: '/Municipality/ListMoAreaWithoutPaging'
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
                    allowBlank: true,
                    itemId: 'tfPhysicalPerson'
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});