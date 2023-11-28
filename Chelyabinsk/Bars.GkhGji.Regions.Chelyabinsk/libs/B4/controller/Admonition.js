Ext.define('B4.controller.Admonition', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
         'B4.aspects.GkhGridEditForm',
         'B4.aspects.ButtonDataExport',
         'B4.Ajax', 'B4.Url'
    ],

    appealCitsAdmonition: null,

    mixins: {
        mask: 'B4.mixins.MaskBody'
        //ToDo Пока невозможно перевести реестр обращения на роуты
        /* Закоментировал в связи с невозможностью перевода на роутинг
        ,
        context: 'B4.mixins.Context'*/
    },

    stores: [
        'appealcits.Admonition',
        'appealcits.AppCitAdmonVoilation'
    ],

    models: [
        'appealcits.Admonition'
    ],

    views: [
        'appealcits.AdmonitionGrid',
        'appealcits.AdmonitionEditWindow',
        'appealcits.AdmonVoilationGrid',
        'appealcits.AdmonVoilationEditWindow',
        'appealcits.MainPanel',
        'appealcits.AdmonitionFilterPanel'
    ],

    mainView: 'appealcits.MainPanel',
    mainViewSelector: 'appealcitsMainPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealcitsMainPanel'
        },
        {
            ref: 'admonitionEditWindow',
            selector: 'admonitioneditwindow'
        }
    ],

    aspects: [
         {
             xtype: 'gkhbuttonprintaspect',
             name: 'admonitionPrintAspect',
             buttonSelector: '#admonitioneditwindow #btnPrint',
             codeForm: 'AppealCitsAdmonition',
             getUserParams: function () {
                 var param = { Id: appealCitsAdmonition };
                 this.params.userParams = Ext.JSON.encode(param);
             }
         },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'disposalGjiButtonExportAspect',
            gridSelector: '#admonitiongrid',
            buttonSelector: '#admonitiongrid #btnExport',
            controllerName: 'AppealCits',
            actionName: 'ExportAdmonition'
        },
         {
             xtype: 'grideditwindowaspect',
             name: 'admonitionGridWindowAspect',
             gridSelector: 'admonitiongrid',
             editFormSelector: 'admonitioneditwindow',
             modelName: 'appealcits.Admonition',
             storeName: 'appealcits.Admonition',
             editWindowView: 'appealcits.AdmonitionEditWindow',
             otherActions: function (actions) {
                 actions['#appealcitsAdmonitionFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                 actions['#appealcitsAdmonitionFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                 actions['#appealcitsAdmonitionFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
             },
             onUpdateGrid: function () {
                 var str = this.controller.getStore('appealcits.Admonition');
                 str.currentPage = 1;
                 str.load();
             },
             onChangeDateStart: function (field, newValue, oldValue) {
                 if (this.controller.params) {
                     this.controller.params.dateStart = newValue;
                 }
             },
             onChangeDateEnd: function (field, newValue, oldValue) {
                 if (this.controller.params) {
                     this.controller.params.dateEnd = newValue;
                 }
             },
             onSaveSuccess: function () {
                 // перекрываем чтобы окно незакрывалось после сохранения
                 B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             },
             listeners: {
                 aftersetformdata: function (asp, rec, form) {
                     var me = this;
                     appealCitsAdmonition = rec.getId();
                     me.controller.getAspect('admonitionPrintAspect').loadReportStore();
                     var grid = form.down('admonVoilationGrid'),
                     store = grid.getStore();
                     store.filter('AppealCitsAdmonition', rec.getId());
                 }
             }
         },

         {
             xtype: 'grideditwindowaspect',
             name: 'admoVoilationGridWindowAspect',
             gridSelector: '#admonVoilationGrid',
             editFormSelector: '#admonVoilationEditWindow',
             storeName: 'appealcits.AppCitAdmonVoilation',
             modelName: 'appealcits.AppCitAdmonVoilation',
             editWindowView: 'appealcits.AdmonVoilationEditWindow',
             listeners: {
                 getdata: function (asp, record) {
                     if (!record.get('Id')) {
                         record.set('AppealCitsAdmonition', appealCitsAdmonition);
                     }
                 }               
             }
         },
    ],

    index: function (operation) {
        var me = this,
            view = me.getMainView() || Ext.widget('appealcitsMainPanel');
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date(new Date().getFullYear(), 11, 31);
        me.bindContext(view);
        this.application.deployView(view);
        //me.getAspect('manOrgLicenseNotificationGisEditPanelAspect').setData(id);
        
        this.getStore('appealcits.Admonition').load();
       // this.getStore('appealcits.Admonition').filter()
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date(new Date().getFullYear(), 11, 31);
        this.getStore('appealcits.Admonition').on('beforeload', this.onBeforeLoadAdmonition, this);
        this.getStore('appealcits.Admonition').load();
        me.callParent(arguments);
    },

    onBeforeLoadAdmonition: function (store, operation) {
        operation.params.isFiltered = true;
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },
});