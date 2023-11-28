Ext.define('B4.ux.grid.column.YesNo', {
    extend: 'Ext.grid.column.Column',
    alias: ['widget.yesnocolumn'],
    alternateClassName: 'B4.grid.YesNoColumn',

    useEmpty: true,

    constructor: function(config) {
        var me = this;

        Ext.applyIf(config, {
            renderer: function(value) {
                if (Ext.isBoolean(value)) {
                    return value ? 'Да' : 'Нет';
                } else {
                    return me.useEmpty ? '' : 'Нет';
                }
            }
        });

        me.callParent([config]);
    }
});