Ext.define('B4.controller.SMEVComplaints', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu',
        'B4.enums.CourtPracticeState'
    ],

    models: ['complaints.SMEVComplaints',
        'complaints.SMEVComplaintsRequest',
        'complaints.SMEVComplaintsExecutant'
    ],
    stores: ['complaints.SMEVComplaints',
        'complaints.SMEVComplaintsRequest',
        'complaints.SMEVComplaintsExecutant'
    ],
    views: [
        'complaints.EditWindow',
        'complaints.Grid',
        'complaints.ExecutantGrid',
        'complaints.ExecutantEditWindow',
        'complaintsrequest.Grid'
    ],
    mainView: 'complaints.Grid',
    mainViewSelector: 'complaintsgrid',
    courtpracticeId: null,
    globalAppeal: null,
    refs: [
        {
            ref: 'mainView',
            selector: 'complaintsgrid'
        },
        {
            ref: 'complaintsEditWindow',
            selector: 'complaintseditwindow'
        }
    ],

    aspects: [
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'complaintsgridStateTransferAspect',
            gridSelector: 'complaintsgrid',
            stateType: 'gji_smev_complaints',
            menuSelector: 'complaintsgridStateMenu'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'complaintsButtonExportAspect',
            gridSelector: 'complaintsgrid',
            buttonSelector: 'complaintsgrid #btnExport',
            controllerName: 'ComplaintsOperations',
            actionName: 'Export'
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'complaintsStateButtonAspect',
            stateButtonSelector: '#complaintsEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    var model = this.controller.getModel('complaints.SMEVComplaints');
                    model.load(entityId, {
                        success: function (rec) {
                            this.controller.getAspect('complaintsGridAspect').setFormData(rec);
                        },
                        scope: this
                    })


                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'complaintsGridAspect',
            gridSelector: 'complaintsgrid',
            editFormSelector: '#complaintsEditWindow',
            storeName: 'complaints.SMEVComplaints',
            modelName: 'complaints.SMEVComplaints',
            editWindowView: 'complaints.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },         
          
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    debugger;
                    asp.controller.courtpracticeId = record.getId();

                    if (asp.controller.courtpracticeId != 0) {
                        asp.controller.getAspect('complaintsStateButtonAspect').setStateData(asp.controller.courtpracticeId, record.get('State'));
                        var grid = form.down('complaintsexecutantgrid'),
                            store = grid.getStore();
                        store.on('beforeload',
                            function (store, operation) {
                                operation.params.complaintId = record.getId();
                            },
                            me);
                        grid.setDisabled(false)
                        store.load();    
                        var gridreq = form.down('complaintsrequestgrid'),
                            reqstore = gridreq.getStore();
                        reqstore.on('beforeload',
                            function (store, operation) {
                                operation.params.complaintId = record.getId();
                            },
                            me);
                        gridreq.setDisabled(false)
                        reqstore.load();    
                    }               

                }
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
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'complaintsexecutantgridAspect',
            gridSelector: 'complaintsexecutantgrid',
            editFormSelector: '#complaintsExecutantEditWindow',
            storeName: 'complaints.SMEVComplaintsExecutant',
            modelName: 'complaints.SMEVComplaintsExecutant',
            editWindowView: 'complaints.ExecutantEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('SMEVComplaints', asp.controller.courtpracticeId);
                    }
                }
            },

        }    
        
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {

        this.params = {};
        var view = this.getMainView() || Ext.widget('complaintsgrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('complaints.SMEVComplaints').load();
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        this.getStore('complaints.SMEVComplaints').on('beforeload', this.onBeforeLoadDoc, this);
        me.callParent(arguments);
    },

    onLaunch: function () {
        debugger;
        var grid = this.getMainView();        
        if (this.params && this.params.recId > 0) {
            var model = this.getModel('complaints.SMEVComplaints');
            this.getAspect('complaintsGridAspect').editRecord(new model({ Id: this.params.recId }));
            this.params.recId = 0;
        }
    },

    onBeforeLoadDoc: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    }

    
});