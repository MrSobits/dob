﻿Ext.define('B4.view.appealcits.GridTopToolBar', {
    extend: 'Ext.toolbar.Toolbar',

    alias: 'widget.appealcitsgridtoptoolbar',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            dock: 'top',
            defaults: {
                margin: '10px 10px 0 0'
            },
            items: [
                {
                    xtype: 'b4addbutton'
                },
                {
                    xtype: 'button',
                    iconCls: 'icon-table-go',
                    text: 'Выгрузка в Excel',
                    textAlign: 'left',
                    itemId: 'btnExport'
                },
                {
                    xtype: 'checkbox',
                    itemId: 'cbShowCloseAppeals',
                    boxLabel: 'Показать закрытые обращения',
                    labelAlign: 'right',
                    checked: false
                },
                {
                    xtype: 'checkbox',
                    itemId: 'cbShowExtensTimes',
                    boxLabel: 'Показать продленные обращения',
                    labelAlign: 'right',
                    checked: false
                },
                {
                    xtype: 'checkbox',
                    name: 'ShowOnlyFromEais',
                    boxLabel: 'Показать только обращения ЕАИС "Обращения граждан"',
                    labelAlign: 'right',
                    checked: false
                }
            ]
        });

        me.callParent(arguments);
    }
});