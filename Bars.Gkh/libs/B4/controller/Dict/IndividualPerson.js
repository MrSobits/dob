Ext.define('B4.controller.dict.IndividualPerson', {
    extend: 'B4.base.Controller',

    requires: [
       
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.Inspector',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
    ],

    models: ['dict.IndividualPerson',
        'dict.IndividualPersonResolution'
    ],
    stores: ['dict.IndividualPerson',
        'dict.IndividualPersonResolution'
    ],
    views: [
        'dict.IndividualPerson.Grid',
       // 'SelectWindow.MultiSelectWindow',
        'dict.IndividualPerson.EditWindow',
        'dict.IndividualPerson.ResolutionGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.IndividualPerson.Grid',
    mainViewSelector: 'individualpersonGrid',

    //селектор окна котоырй потом используется при открытии
    editWindowSelector: '#individualpersonEditWindow',
    
    refs: [{
        ref: 'mainView',
        selector: 'individualpersonGrid'
    }],

    aspects: [
        //{
        //    xtype: 'inspectordictperm'
        //},
        {
            xtype: 'grideditwindowaspect',
            name: 'inspectorGridWindowAspect',
            gridSelector: 'individualpersonGrid',
            editFormSelector: '#individualpersonEditWindow',
            storeName: 'dict.IndividualPerson',
            modelName: 'dict.IndividualPerson',
            editWindowView: 'dict.individualperson.EditWindow',
            onSaveSuccess: function (asp, record) {
                debugger;
                asp.controller.individualPersonid = record.getId();

             //   this.updateControls(asp.controller.individualPersonid);
            },
            
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    debugger;
                    asp.controller.setCurrentId(record.getId());
                    asp.controller.getStore('dict.IndividualPersonResolution').load();
                    
                    var individualPersonid = record.getId();
                    asp.controller.individualPersonid = individualPersonid;

                    //var fieldresolutionid = form.down('#resolutionid');
                    //var fielddocumentdate = form.down('#documentdate');
                    //var fielddocumentnumber = form.down('#documentnumber');
                    //var fielddocumentTime = form.down('#documentTime');
                    //var fielddocumentYear = form.down('#documentYear');
                   // this.getStore('dict.IndividualPerson').load();

                    //if (inpectorId > 0) {
                    //    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    //    B4.Ajax.request({
                    //        method: 'POST',
                    //        url: B4.Url.action('ListIndividualPerson', 'Resolution'),
                    //        params: {
                    //            inpectorId: asp.controller.inpectorId
                    //        }
                    //    }).next(function (response) {
                    //        //десериализуем полученную строку
                    //        var obj = Ext.JSON.decode(response.responseText);
                    //        debugger;
                    //        asp.controller.unmask();
                    //    }).error(function () {
                    //        asp.controller.unmask();
                    //    });
                    //} else {
                    //   // fieldInspectors.updateDisplayedText(null);
                    //  //  fieldInspectors.setValue(null);
                        
                    //}
                }
            }
        },
        //{
        //    xtype: 'grideditwindowaspect',
        //    name: 'inspectorGridWindowAspect',
        //    gridSelector: 'individualpersonEditWindow',
        //    editFormSelector: '#individualpersonresolutiongrid',
        //    storeName: 'dict.IndividualPerson',
        //    modelName: 'dict.IndividualPerson',
        //    editWindowView: 'dict.individualperson.ResolutionGrid',
        //    onSaveSuccess: function (asp, record) {
        //        debugger;
        //        asp.controller.inpectorId = record.getId();

        //        this.updateControls(asp.controller.inpectorId);
        //    },

        //    listeners: {
        //        aftersetformdata: function (asp, record, form) {
        //            debugger;
        //            asp.controller.setCurrentId(record.getId());
        //            asp.controller.getStore('dict.IndividualPerson').load();

        //            var inpectorId = record.getId();
        //            asp.controller.inpectorId = inpectorId;

        //            var fieldresolutionid = form.down('#resolutionid');
        //            var fielddocumentdate = form.down('#documentdate');
        //            var fielddocumentnumber = form.down('#documentnumber');
        //            var fielddocumentTime = form.down('#documentTime');
        //            var fielddocumentYear = form.down('#documentYear');
        //            // this.getStore('dict.IndividualPerson').load();

        //            if (inpectorId > 0) {
        //                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
        //                B4.Ajax.request({
        //                    method: 'POST',
        //                    url: B4.Url.action('ListIndividualPerson', 'Resolution'),
        //                    params: {
        //                        inpectorId: asp.controller.inpectorId
        //                    }
        //                }).next(function (response) {
        //                    //десериализуем полученную строку
        //                    var obj = Ext.JSON.decode(response.responseText);
        //                    debugger;
        //                    asp.controller.unmask();
        //                }).error(function () {
        //                    asp.controller.unmask();
        //                });
        //            } else {
        //                // fieldInspectors.updateDisplayedText(null);
        //                //  fieldInspectors.setValue(null);

        //            }
        //        }
        //    }
        //},
        //{
        //    /*
        //    множественный выбор Жил инспекций
        //   аспект взаимодействия триггер-поля инспекторы с массовой формой выбора зон жи
        //   по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
        //   По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
        //   и сохраняем инспекторов через серверный метод /Inspector/AddZonalInspection
        //   */
        //    xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        //    name: 'inspectorZonalMultiSelectWindowAspect',
        //    fieldSelector: '#inspectorEditWindow #zonInspectorsTrigerField',
        //    multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        //    multiSelectWindowSelector: '#inspectorZonalSelectWindow',
        //    storeSelect: 'dict.ZonalInspectionForSelect',
        //    storeSelected: 'dict.inspector.ZonalInspection',
        //    textProperty: 'ZoneName',
        //    columnsGridSelect: [
        //        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
        //    ],
        //    columnsGridSelected: [
        //        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
        //    ],
        //    titleSelectWindow: 'Выбор ЗЖИ',
        //    titleGridSelect: 'ЗЖИ для отбора',
        //    titleGridSelected: 'Выбранные ЗЖИ',
        //    onSelectedBeforeLoad: function (store, operation) {
        //        operation.params['inpectorId'] = this.controller.inpectorId;
        //    },

        //    otherActions: function (actions) {
        //        actions[this.fieldSelector] = { 'triggerClear': { fn: this.onClearInspections, scope: this } };
        //    },

        //    onClearInspections: function () {
        //        var me = this;

        //        me.controller.mask('Сохранение', me.controller.getMainComponent());
        //        B4.Ajax.request({
        //            method: 'POST',
        //            url: B4.Url.action('AddZonalInspection', 'Inspector'),
        //            params: {
        //                objectIds: Ext.encode([]),
        //                inpectorId: me.controller.inpectorId
        //            }
        //        });

        //        me.controller.unmask();
        //    },

        //    listeners: {
                
        //        getdata: function (asp, records) {
        //            debugger;
        //            var recordIds = [];
        //            records.each(function (rec) { recordIds.push(rec.get('Id')); });
        //            asp.controller.mask('Сохранение', asp.controller.getMainComponent());
        //            B4.Ajax.request({
        //                method: 'POST',
        //                url: B4.Url.action('AddZonalInspection', 'Inspector'),
        //                params: {
        //                    objectIds: Ext.encode(recordIds),
        //                    inpectorId: asp.controller.inpectorId
        //                }
        //            }).next(function () {
        //                Ext.Msg.alert('Сохранение!', 'ЗЖИ сохранены успешно');
        //                asp.controller.unmask();
        //                return true;
        //            }).error(function () {
        //                asp.controller.unmask();
        //            });

        //            return true;
        //        }
        //    }
        //}
    ],

    init: function () {
        debugger;
        
        this.getStore('dict.IndividualPersonResolution').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {

        operation.params.individualPersonid = this.individualPersonid;

        debugger;
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('individualpersonGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.IndividualPerson').load();
;
    },
    
    setCurrentId: function (id) {
        debugger;
        this.individualPersonid = id;
        var store = this.getStore('dict.IndividualPersonResolution');
        store.removeAll();
        
        var editwindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
        //editwindow.down('#individualpersonEditWindow').setDisabled(!id);

        if (id) {
            store.load();
        }
    }
});