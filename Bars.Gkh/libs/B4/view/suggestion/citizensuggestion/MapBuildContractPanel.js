﻿Ext.define('B4.view.suggestion.citizensuggestion.MapBuildContractPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.citizensuggestionmappanel',
    closable: true,
    title: 'Карта',
    layout: 'fit',
    initComponent: function () {
        var me = this;
        debugger;
        me.Id =  'ya-map-' + Ext.id(),
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'component',
                    renderTpl: new Ext.XTemplate(
                        '<div id="' + me.Id + '" style="width: 100%; height: 100%;"></div>'
                    )
                }
            ]
        });

        me.callParent(arguments);
    }
});
