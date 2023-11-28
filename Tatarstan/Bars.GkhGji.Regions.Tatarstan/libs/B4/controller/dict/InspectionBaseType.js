Ext.define('B4.controller.dict.InspectionBaseType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: ['dict.inspectionbasetype.Grid'],

    mainView: 'dict.inspectionbasetype.Grid',
    mainViewSelector: 'inspectionbasetyperid',

    refs: [
        {
            ref: 'mainView',
            selector: 'inspectionbasetypegrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'inspectionbasetypegrid',
            permissionPrefix: 'GkhGji.Dict.InspectionBaseType'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            storeName: 'dict.InspectionBaseType',
            modelName: 'dict.InspectionBaseType',
            gridSelector: 'inspectionbasetypegrid',
            listeners: {
                'beforesave': function (asp, store) {
                    var modifiedRecords = store.getModifiedRecords(),
                        validate = true;

                    Ext.each(modifiedRecords, function(rec) {
                        if (!rec.get('Name') || !rec.get('Code')) {
                            validate = false;
                        }
                    });

                    if (!validate) {
                        Ext.Msg.alert('Ошибка сохранения', 'Необходимо заполнить поля');
                    }

                    return validate;
                }
            }
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('inspectionbasetypegrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});