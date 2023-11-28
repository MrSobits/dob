Ext.define('B4.controller.objectcr.ScheduleExecutionWork', {
    /*
    * Контроллер раздела график выполнения работ
    */
    extend: 'B4.controller.MenuItemController',

    requires:
    [
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhInlineGrid'
    ],

    models: ['objectcr.TypeWorkCr', 'objectcr.MonitoringSmr','objectcr.TypeWorkCrAddWork','dict.AdditWork'],
    stores: ['objectcr.ScheduleExecutionWork','objectcr.TypeWorkCrAddWork','objectcr.TypeWorkCrAddWorkForSelect','objectcr.TypeWorkCrAddWorkForSelected'],
    views: ['objectcr.ScheduleExecutionWorkGrid',
             'objectcr.ScheduleExecutionWorkEditWindow',
            'objectcr.scheduleexecutionwork.AddDateGrid',
            'objectcr.ScheduleExecutionWorkStageGrid',
            'objectcr.scheduleexecutionwork.AddDateWindow'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    objectCrId:null,
    twcrId:null,
    workid:null,

    mainView: 'objectcr.ScheduleExecutionWorkGrid',
    mainViewSelector: 'scheduleexecutionworkgrid',

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
        //{
        //    xtype: 'gkhstatepermissionaspect',
        //    name: 'scheduleExecutionWorkObjectCrPerm',
        //    editFormAspectName: 'scheduleExecutionWorkGridAspect',
        //    permissions: [
        //           // { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Edit', applyTo: 'b4savebutton', selector: 'scheduleexecutionworkgrid' },
        //            { name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.AddDate', applyTo: '#additionalDateButton', selector: 'scheduleexecutionworkgrid' },
        //            {
        //                name: 'GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Column.FinanceSource',
        //                applyTo: '[dataIndex=FinanceSourceName]',
        //                selector: 'scheduleexecutionworkgrid',
        //                applyBy: function (component, allowed) {
        //                    if (component)
        //                        component.setVisible(allowed);
        //                }
        //            }
        //    ]
        //},
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела график выполнения работ
            */
            xtype: 'grideditwindowaspect',
            name: 'scheduleExecutionWorkGKhGridAspect',
            storeName: 'objectcr.ScheduleExecutionWork',
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'scheduleexecutionworkgrid',
            editFormSelector: 'scheduleExecutionWorkEditWindow',
            editWindowView: 'objectcr.ScheduleExecutionWorkEditWindow',
            otherActions: function (actions) {
                actions['scheduleexecutionworkgrid #additionalDateButton'] = { 'click': { fn: this.onAddDateButtonClick, scope: this } };
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                        twcrId = record.getId();
                workid = record.get('Work').Id;
                debugger;
                var form = asp.getForm(),
                    archGrid = form.down('scheduleexecutionworkstagegrid'),
                    archStore = archGrid.getStore();
                archStore.on('beforeload', function (store, operation) {
                    operation.params.typeWorkId = twcrId;
                     },
                        me);
                    archStore.load();
                    }
            },
           
            onAddDateButtonClick: function () {
                var editWindow = this.componentQuery('#scheduleExecutionWorkAddDateWindow');

                if (editWindow && !editWindow.getBox().width) {
                    editWindow = editWindow.destroy();
                }

                if (!editWindow) {
                    editWindow = this.controller.getView('objectcr.scheduleexecutionwork.AddDateWindow').create(
                        {
                            renderTo: B4.getBody().getActiveTab().getEl()
                        });

                    editWindow.show();
                }

                var store = editWindow.down('grid').getStore();
                store.clearFilter(true);
                store.filter('objectCrId', this.controller.getContextValue(this.controller.getMainView(), 'objectcrId'));
            }
        },
        {
             xtype: 'gkhinlinegridmultiselectwindowaspect',
             name: 'typeWorkCrAddWorkAspect',
             gridSelector: 'scheduleexecutionworkstagegrid',
             storeName: 'objectcr.TypeWorkCrAddWork',
             modelName: 'objectcr.TypeWorkCrAddWork',
             multiSelectWindow: 'SelectWindow.MultiSelectWindow',
             multiSelectWindowSelector: '#typeWorkCrAddWorkForSelectedMultiSelectWindow',
             storeSelect: 'objectcr.TypeWorkCrAddWorkForSelect',
             storeSelected: 'objectcr.TypeWorkCrAddWorkForSelected',
             titleSelectWindow: 'Выбор этапов работ',
             titleGridSelect: 'Этапы работ',
             titleGridSelected: 'Выбранные этапы работ',
             columnsGridSelect: [
                 { header: 'Код записи', xtype: 'gridcolumn', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                 { header: 'Описание', xtype: 'gridcolumn', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
             ],
             columnsGridSelected: [
                 { header: 'Код записи', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, sortable: false },
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
             ],
             onBeforeLoad: function (store, operation) {
                 operation.params.workId = workid;
             },
             listeners: {
                 getdata: function (me, records) {
                     var store = me.controller.getStore(me.storeName);
                    me.controller.mask('Сохранение', me.controller.getMainComponent());
                     records.each(function (rec) {
                         if (rec.get('Id')) {
                             var recordtwaw = me.controller.getModel('objectcr.TypeWorkCrAddWork').create();
                             recordtwaw.set('TypeWorkCr', twcrId);
                             recordtwaw.set('AdditWork', rec.get('Id'));
                             recordtwaw.set('AdditWorkName', rec.get('Name'));

                             store.insert(0, recordtwaw);
                         }
                     });
                     me.controller.unmask();
                     return true;
                 }
             
             }
         },
        //{
        //    /*
        //    * Аспект взаимодействия таблицы и формы редактирования раздела график выполнения работ
        //    */
        //    xtype: 'gkhinlinegridaspect',
        //    name: 'scheduleExecutionWorkGridAspect',
        //    modelName: 'objectcr.TypeWorkCr',
        //    gridSelector: 'scheduleexecutionworkgrid',
        //    otherActions: function (actions) {
        //        actions['scheduleexecutionworkgrid #additionalDateButton'] = { 'click': { fn: this.onAddDateButtonClick, scope: this } };
        //    },
        //    onAddDateButtonClick: function () {
        //        var editWindow = this.componentQuery('#scheduleExecutionWorkAddDateWindow');

        //        if (editWindow && !editWindow.getBox().width) {
        //            editWindow = editWindow.destroy();
        //        }

        //        if (!editWindow) {
        //            editWindow = this.controller.getView('objectcr.scheduleexecutionwork.AddDateWindow').create(
        //                {
        //                    renderTo: B4.getBody().getActiveTab().getEl()
        //                });

        //            editWindow.show();
        //        }

        //        var store = editWindow.down('grid').getStore();
        //        store.clearFilter(true);
        //        store.filter('objectCrId', this.controller.getContextValue(this.controller.getMainView(), 'objectcrId'));
        //    }
        //},
        {
            xtype: 'gkhinlinegridaspect',
            name: 'scheduleExecutionWorkAddDateGridAspect',
            modelName: 'objectcr.TypeWorkCr',
            gridSelector: 'schexworkdategrid',
            otherActions: function(actions) {
                actions['schexworkdategrid b4closebutton'] = { 'click': { fn: this.onCloseButtonClick, scope: this } };
                actions['schexworkdategrid #fillDateButton'] = { 'click': { fn: this.onFillDateButtonClick, scope: this } };
            },
            onCloseButtonClick: function () {
                var editWindow = this.componentQuery('#scheduleExecutionWorkAddDateWindow');
                if (editWindow)
                    editWindow.close();
            },
            onFillDateButtonClick: function () {
                var asp = this;
                var window = new Ext.window.Window({
                    title: 'Выберите доп. срок:',
                    width: 220,
                    bodyPadding: 10,
                    itemId: 'datePickWindow',
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    items: [{
                        xtype: 'datepicker',
                        listeners: {
                            select: function (datpick, date) {
                                Ext.Msg.confirm('Внимание', 'Вы уверены, что хотите изменить дополнительный срок для каждого вида работ? ',  function (confirmationResult) {
                                    if (confirmationResult == 'yes') {
                                        var store = asp.getGrid().getStore();
                                        store.each(function (record) {
                                            record.set('AdditionalDate', date);
                                        });
                                        
                                        datpick.up('#datePickWindow').close();
                                    } 
                                });
                            }
                        }
                    }]
                });

                window.show();
            }
        }
    ],

    init: function () {
        var actions = {};
    //    actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('scheduleexecutionworkgrid'),
            store;

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');
        objectCrId = id;
        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    },

    //onMainViewAfterRender: function () {
    //    var aspect = this.getAspect('scheduleExecutionWorkObjectCrPerm'),
    //        objectCrId = aspect.controller.getContextValue(aspect.controller.getMainComponent(), 'objectcrId');
    //        this.mask('Загрузка', this.getMainComponent());
    //        B4.Ajax.request(B4.Url.action('GetByObjectCrId', 'MonitoringSmr', {
    //            objectCrId: objectCrId
    //        })).next(function (response) {
    //            var obj = Ext.JSON.decode(response.responseText);
    //            var model = this.getModel('objectcr.MonitoringSmr');

    //            model.load(obj.MonitoringSmrId, {
    //                success: function (rec) {
    //                    aspect.setPermissionsByRecord(rec);
    //                },
    //                scope: this
    //            });
    //            this.unmask();
    //            return true;
    //        }, this).error(function () {
    //            this.unmask();
    //            Ext.Msg.alert('Сообщение', 'Произошла ошибка');
    //        }, this);
    //}
});