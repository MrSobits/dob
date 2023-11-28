Ext.define('B4.view.cmnestateobj.group.structelement.Panel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.structelgroupelementspanel',
    
    title: 'Конструктивные элементы',
    
    requires: [
        'B4.view.cmnestateobj.group.structelement.Grid',
        'B4.view.cmnestateobj.group.structelement.WorkGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'groupelementsgrid',
                    flex: 1,
                    margin: -1
                },
                {
                    xtype: 'groupelementworksgrid',
                    flex: 1,
                    margin: -1,
                    disabled: true
                }
            ]
        });

        me.callParent(arguments);
    }
});