Ext.define('B4.controller.dict.IndividualPerson', {
    extend: 'B4.base.Controller',

    requires: [
       
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.Inspector',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
    ],

    models: ['TransportOwner',
        'dict.IndividualPerson',
        'dict.IndividualPersonResolution'
    ],
    stores: ['TransportOwner',
        'dict.IndividualPerson',
        'dict.IndividualPersonResolution'
    ],
    views: [
        'dict.IndividualPerson.Grid',
       // 'SelectWindow.MultiSelectWindow',
        'dict.IndividualPerson.EditWindow',
        'dict.individualperson.TransportGrid',
        'dict.IndividualPerson.ResolutionGrid',
        'dict.individualperson.TransportEditWindow'
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
            otherActions: function (actions) {
                actions['#individualpersonEditWindow #cbIsPlaceResidenceOutState'] = { 'change': { fn: this.onChangeIsPlaceResidence, scope: this } };
                actions['#individualpersonEditWindow #cbIsActuallyResidenceOutState'] = { 'change': { fn: this.onChangeIsActuallyResidence, scope: this } };                
            },
            onChangeIsPlaceResidence: function (field, newValue) {
                var form = field.up('#individualpersonEditWindow'),
                    tfPlaceResidenceOutState = form.down('#tfPlaceResidenceOutState');

                if (newValue == true) {
                    tfPlaceResidenceOutState.show();
             
                }
                else {
                    tfPlaceResidenceOutState.hide();
                }
            },
            onChangeIsActuallyResidence: function (field, newValue) {
                var form = field.up('#individualpersonEditWindow'),
                    tfActuallyResidenceOutState = form.down('#tfActuallyResidenceOutState');

                if (newValue == true) {
                    tfActuallyResidenceOutState.show();

                }
                else {
                    tfActuallyResidenceOutState.hide();
                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    debugger;
                    asp.controller.setCurrentId(record.getId());
                    asp.controller.getStore('dict.IndividualPersonResolution').load();
                    
                    var individualPersonid = record.getId();
                    asp.controller.individualPersonid = individualPersonid;

                    if (record.getId()) {
                        var trgrid = form.down('individualpersontransportgrid');
                        var trstore = trgrid.getStore();
                        trstore.load();
                    }

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
        {
            /*
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'individualpersontransportAspect',
            gridSelector: 'individualpersontransportgrid',
            editFormSelector: '#individualpersonTransportEditWindow',
            storeName: 'TransportOwner',
            modelName: 'TransportOwner',
            editWindowView: 'dict.individualperson.TransportEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('IndividualPerson', this.controller.individualPersonid);
                    }
                }
            }
        },
    ],

    init: function () {
        debugger;
        
        this.getStore('dict.IndividualPersonResolution').on('beforeload', this.onBeforeLoad, this);
        this.getStore('TransportOwner').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        operation.params.individualPersonid = this.individualPersonid;
        operation.params.personId = this.individualPersonid;
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