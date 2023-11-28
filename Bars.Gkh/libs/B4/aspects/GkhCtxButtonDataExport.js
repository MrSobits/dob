/*
Данный аспект предназначен для Экспорта грида в эксель
*/

Ext.define('B4.aspects.GkhCtxButtonDataExport', {
    extend: 'B4.aspects.ButtonDataExport',

    alias: 'widget.gkhctxbuttondataexportaspect',

    getGrid: function () {
        var me = this;
        return me.componentQuery(me.gridSelector);
    }
});