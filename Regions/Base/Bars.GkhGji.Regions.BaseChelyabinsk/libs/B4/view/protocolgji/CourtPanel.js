Ext.define('B4.view.protocolgji.CourtPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.protocolgjiCourtPanel',
    itemid: 'protocolGjiCourtPanel',
    
    requires: [
        'B4.form.FiasSelectAddress',
        'B4.store.Contragent',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.EnumCombo',
        'B4.store.comission.ComissionMeeting',
        'B4.store.dict.JurInstitution',
        'B4.enums.YesNoNotSet'
    ],
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            bodyStyle: Gkh.bodyStyle,
            title: 'Решение суда',
            border: false,
            bodyPadding: 5,
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            autoScroll: true,
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '5 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 170
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'vbox',
                            padding: '5 0 5 25',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 220
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'ToCourt',
                                    fieldLabel: 'Материалы переданы в суд',
                                    flex: 1
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'CourtCaseNumber',
                                    fieldLabel: 'Номер судебного решения',
                                    flex: 1
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'JudSector',
                                    fieldLabel: 'Судебный участок',
                                    store: 'B4.store.dict.JurInstitution',
                                    readOnly: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    width: 600,
                                    flex: 1
                                },
                                {
                                    xtype: 'combobox',
                                    editable: false,
                                    fieldLabel: 'Взыскан ли штраф',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'IsFineCharged'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'ChargeAmount',
                                    fieldLabel: 'Наложенная сумма',
                                    flex: 0.7,
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'vbox',
                            padding: '5 0 5 25',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 220
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateToCourt',
                                    fieldLabel: 'Дата направления материала в суд',
                                    format: 'd.m.Y',
                                    flex: 1
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'CourtCaseDate',
                                    fieldLabel: 'Дата судебного решения',
                                    format: 'd.m.Y',
                                    flex: 1
                                },
                                {
                                    xtype: 'b4combobox',
                                    name: 'CourtSanction',
                                    fieldLabel: 'Решение суда',
                                    fields: ['Id', 'Name', 'Code'],
                                    url: '/SanctionGji/List',
                                    flex: 1
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'FineChargeDate',
                                    fieldLabel: 'Дата взыскания',
                                    format: 'd.m.Y',
                                    flex: 1
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'PaidAmount',
                                    fieldLabel: 'Уплаченная сумма',
                                    flex: 1,
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false
                                }
                            ]
                        },
                    ]
                }
            ],
            viewConfig: {

               
            },
        });

        me.callParent(arguments);
    }
});