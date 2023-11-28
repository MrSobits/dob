Ext.define('B4.controller.dict.RiskCategory', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    views: ['dict.riskcategory.Grid'],

    mainView: 'dict.riskcategory.Grid',
    mainViewSelector: 'riskcategory.Grid',

    refs: [
        {
            ref: 'mainView',
            selector: 'riskcategorygrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'riskcategorygrid',
            permissionPrefix: 'GkhGji.Dict.RiskCategory'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'riskCategoryGridAspect',
            storeName: 'dict.RiskCategory',
            modelName: 'dict.RiskCategory',
            gridSelector: 'riskcategorygrid',
            save: function () {
                var me = this,
                    store = this.getStore(),
                    hasErrorRow = false,
                    modifiedRecs = store.getModifiedRecords(),
                    removedRecs = store.getRemovedRecords(),
                    requiredFields = ['Code', 'Name','RiskFrom','RiskTo'];

                Ext.each(modifiedRecs,
                    function(rec, index) {
                        Ext.each(requiredFields,
                            function(field, index) {
                                if (Ext.isEmpty(rec.get(field))) {
                                    hasErrorRow = true;
                                }
                            });
                    });

                if (hasErrorRow) {
                    Ext.Msg.alert('Предупреждение!', 'Необходимо заполнить все поля');
                    return;
                }

                if (modifiedRecs.length > 0 || removedRecs.length > 0) {
                    if (this.fireEvent('beforesave', this, store) !== false) {
                        me.mask('Сохранение', this.getGrid());
                        store.sync({
                            callback: function () {
                                me.unmask();
                                store.load();
                            },
                            failure: me.handleDataSyncError,
                            scope: me
                        });
                    }
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('riskcategorygrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});