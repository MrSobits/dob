Ext.define('B4.controller.dict.Subpoena', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid', 'B4.aspects.permission.GkhInlineGridPermissionAspect', 'B4.aspects.GridEditWindow'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.Subpoena'],
    stores: ['dict.Subpoena'],

    views: [
        'dict.subpoena.Grid',
        'dict.subpoena.EditWindow',
    ],

    mainView: 'dict.subpoena.Grid',
    mainViewSelector: 'subpoenaGrid',
    
    refs: [
        {
            ref: 'mainView',
            selector: 'subpoenaGrid'
        }
    ],

    aspects: [
        //{
        //    xtype: 'inlinegridpermissionaspect',
        //    gridSelector: 'subpoenaGrid',
        //    permissionPrefix: 'GkhGji.Dict.Subpoena'
        //},
        //{
        //    xtype: 'gkhinlinegridaspect',
        //    name: 'SubpoenaGridAspect',
        //    storeName: 'dict.Subpoena',
        //    modelName: 'dict.Subpoena',
        //    gridSelector: 'subpoenaGrid'
        //},
        {
            // Аспект взаимодействия таблицы справочника Повестки и формы редактирования
            xtype: 'grideditwindowaspect',
            name: 'subpoenaGridWindowAspect',
            gridSelector: 'subpoenaGrid',
            editFormSelector: '#subpoenaeditwindow',
            storeName: 'dict.Subpoena',
            modelName: 'dict.Subpoena',
            editWindowView: 'dict.subpoena.EditWindow',
            onSaveSuccess: function (asp, record) {
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
        }
    ],


    index: function () {
        var view = this.getMainView() || Ext.widget('subpoenaGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.Subpoena').load();
    }
});