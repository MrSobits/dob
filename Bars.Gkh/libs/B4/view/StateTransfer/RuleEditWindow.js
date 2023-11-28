Ext.define('B4.view.StateTransfer.RuleEditWindow', {
    extend: 'B4.form.Window',

    layout: 'form',
    width: 600,
    bodyPadding: 5,
    itemId: 'stateTransferRuleEditWindow',
    title: 'Правило перехода статуса',
    trackResetOnLoad: true,
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.StateTransfer',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: { labelWidth: 100, labelAlign: 'right', anchor: '100%' },
            items: [
                {
                    xtype: 'b4combobox',
                    itemId: 'stateTransferComboBox',
                    name: 'StateTransfer',
                    labelAlign: 'right',
                    allowBlank: false,
                    editable: false,
                    fields: ['Id', 'Name', 'TypeName', 'TypeId', 'Role'],
                    fieldLabel: 'Переход',
                    url: '/StateTransfer/ListFiltredTransfers',
                    queryMode: 'local',
                    triggerAction: 'all'
                },
                {
                    xtype: 'b4combobox',
                    name: 'RuleId',
                    itemId: 'stateTransferRuleComboBox',
                    labelAlign: 'right',
                    allowBlank: false,
                    editable: false,
                    fieldLabel: 'Правило',
                    url: '/StateTransferRule/ListRules',
                    queryMode: 'local',
                    triggerAction: 'all'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        { xtype: 'buttongroup', columns: 1, items: [{ xtype: 'b4savebutton' }] },
                        { xtype: 'tbfill' },
                        { xtype: 'buttongroup', columns: 1, items: [{ xtype: 'b4closebutton' }] }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});