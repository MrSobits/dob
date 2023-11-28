Ext.define('B4.controller.Import.ASFK', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhImportAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['Import.ASFKPanel'],

    mainView: 'Import.ASFKPanel',
    mainViewSelector: 'asfkimportpanel',

    aspects: [
    {
            xtype: 'gkhimportaspect',
            viewSelector: 'asfkimportpanel',
            importId: 'Bars.GkhGji.Regions.Voronezh.Import.ASFKImport',
            initComponent: function () {
                var me = this;
                Ext.apply(me,
                    {
                        maxFileSize: Gkh.config.General.MaxUploadFileSize * 1048576
                    });

                me.callParent(arguments);
            }
    }],

    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    }
});