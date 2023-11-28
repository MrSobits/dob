Ext.define('B4.controller.ASFK', {
    extend: 'B4.base.Controller',

    asfkId: null,
    bdoperId: null,
    showAll: false,

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GkhGridMultiSelectWindowTree',
        'B4.aspects.permission.AppealCits',
        'B4.aspects.permission.AppealCitsAnswer',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.StateGridWindowColumn',
        'B4.store.asfk.ASFK',
        'B4.store.asfk.BDOPER',
        'B4.aspects.FieldRequirementAspect',
        'B4.model.administration.Operator'
    ],

    models: [
        'asfk.ASFK',
        'asfk.BDOPER',
        'asfk.ASFKResolution',
        'Resolution'
    ],
    stores: [
        'asfk.ASFK',
        'asfk.BDOPER',
        'asfk.ASFKResolution',
        'asfk.ListResolutionsForSelect',
        'asfk.ListResolutionsForSelected'
    ],
    views: [
        'asfk.ASFKGrid',
        'asfk.ASFKEditWindow',
        'asfk.BDOPERGrid',
        'asfk.BDOPEREditWindow',
        'asfk.ASFKResolutionGrid',
        'asfk.MultiSelectWindowResolution'
    ],
    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhGji.ASFK.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: '#asfkGrid',
                    applyBy: function (component, allowed) {
                        var me = this;
                        me.controller.params = me.controller.params || {};
                        if (allowed) {
                            component.show();
                        }
                        else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'asfkGridAspect',
            gridSelector: 'asfkgrid',
            editFormSelector: '#asfkEditWindow',
            storeName: 'asfk.ASFK',
            modelName: 'asfk.ASFK',
            editWindowView: 'asfk.ASFKEditWindow',
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var bdoperGrid = form.down('#bdoperGrid'),
                        bdoperStore = bdoperGrid.getStore();  
                    asfkId = record.data.Id;
                    asp.controller.asfkId = record.data.Id;
                    bdoperStore.filter('asfkId', asfkId);
                    bdoperStore.load();
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'bdoperGridAspect',
            gridSelector: 'bdopergrid',
            editFormSelector: '#bdoperEditWindow',
            storeName: 'asfk.BDOPER',
            modelName: 'asfk.BDOPER',
            editWindowView: 'asfk.BDOPEREditWindow',
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var resolGrid = form.down('#asfkResolutionGrid'),
                        resolStore = resolGrid.getStore();
                    bdoperId = record.data.Id;
                    asp.controller.bdoperId = record.data.Id;
                    resolStore.filter('bdoperId', bdoperId);
                    resolStore.load();
                }
            }
        },
        {
            /* 
               Аспект взаимодействия таблицы Постановлений и грида с одиночным доабавлением
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'asfkResolutionGridWindowAspect',
            selModelMode: 'SINGLE',
            gridSelector: '#asfkResolutionGrid',
            storeName: 'asfk.ASFKResolution',
            modelName: 'asfk.ASFKResolution',
            multiSelectWindow: 'asfk.MultiSelectWindowResolution',
            multiSelectWindowSelector: '#asfkMultiSelectWindowResolution',
            editFormSelector: '#bdoperEditWindow',
            editWindowView: 'asfk.BDOPEREditWindow',
            storeSelect: 'asfk.ListResolutionsForSelect',
            storeSelected: 'asfk.ListResolutionsForSelected',
            titleSelectWindow: 'Выбор постановления',
            titleGridSelect: 'Постановления',
            titleGridSelected: 'Выбранное постановление',
            columnsGridSelect: [
                {
                    text: 'Номер документа',
                    flex: 0.5,
                    dataIndex: 'DocumentNumber',
                    filter: { xtype: 'textfield' }
                },
                {
                    type: 'datecolumn',
                    text: 'Дата документа',
                    flex: 0.5,
                    dataIndex: 'DocumentDate',
                    filter: { xtype: 'datefield', format: 'd.m.Y' },
                    renderer: function (val) {
                        if (val != null) {
                            return Ext.Date.format(new Date(val), 'd.m.Y');
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    type: 'datecolumn',
                    text: 'Дата вступления в законную силу',
                    flex: 0.5,
                    dataIndex: 'InLawDate',
                    filter: { xtype: 'datefield', format: 'd.m.Y' },
                    renderer: function (val) {
                        if (val != null) {
                            return Ext.Date.format(new Date(val), 'd.m.Y');
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    text: 'ФИО нарушителя',
                    dataIndex: 'Fio',
                    flex: 1.5,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Контрагент',
                    dataIndex: 'ContragentName',
                    flex: 1.5,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Сумма штрафа',
                    dataIndex: 'PenaltyAmount',
                    flex: 0.5,
                    filter: { xtype: 'numberfield' }
                },
                {
                    text: 'Комиссия',
                    dataIndex: 'ComissionName',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    text: 'Номер документа',
                    flex: 1,
                    dataIndex: 'DocumentNumber',
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Сумма штрафа',
                    dataIndex: 'PenaltyAmount',
                    flex: 1,
                    filter: { xtype: 'numberfield' }
                }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.bdoperId = bdoperId;
                operation.params.showAll = this.getForm().down('#cbShowAllResolutions').value;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] <= 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать постановление');
                        return false;
                    }

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddPayFines', 'BDOPER', {
                        resolutionId: recordIds[0],
                        bdoperId: bdoperId
                    })).next(function () {
                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранено!', 'Постановление сохранено успешно');
                    }).error(function (result) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка', result.message ? result.message : 'Произошла ошибка');
                    });

                    return true;
                }
            }
        },
    ],

    mainView: 'asfk.ASFKGrid',
    mainViewSelector: 'asfkgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'asfkgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    //init: function () {
    //    this.control({

    //        '#bdoperGrid #selectResolBtn': { click: { fn: this.openAsfkPayFineWindow, scope: this } },
    //        '#asfkPayFineWindow #createPayFineBtn': { click: { fn: this.createPayFine, scope: this } },
    //        '#asfkPayFineWindow #closeBtn': { click: { fn: this.closeAsfkPayFineWindow, scope: this } }

    //    });

    //    this.callParent(arguments);
    //},

    index: function () {
        var view = this.getMainView() || Ext.widget('asfkgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('asfk.ASFK').load();
    }
});