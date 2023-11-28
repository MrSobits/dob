Ext.define('B4.controller.documentsgjiregister.ProtocolGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GjiDocumentRegister',
        'B4.aspects.DocumentGjiMassButtonPrintAspect',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    models: [
        'ProtocolGji',
        'InspectionGji'
    ],
    stores: ['view.ProtocolGji'],

    views: ['documentsgjiregister.ProtocolGrid'],

    mainView: 'documentsgjiregister.ProtocolGrid',
    mainViewSelector: '#docsGjiRegisterProtocolGrid',

    aspects: [
        {
            xtype: 'documentgjimassbuttonprintaspect',
            name: 'documentgjimassbuttonProtocolPrintAspect',
            buttonSelector: 'registerprotocol2025grid gkhbuttonprint[action=ProtocolPrint]',
            codeForm: 'Protocol', 
            reportstoremethod: '/ComissionMeetingReport/GetProtocol2025ReportList',
            getUserParams: function () {
                var me = this,
                    grid = me.controller.getMainView(),
                    records = grid.getSelectionModel().getSelection(),
                    recIds = [],
                    param = { comissionId: 0 };
                debugger;
                Ext.each(records,
                    function (rec) {
                        recIds.push(rec.get('Id'));
                    });

                Ext.apply(me.params, { recIds: Ext.JSON.encode(recIds) });
                me.params.userParams = Ext.JSON.encode(param);
                debugger;
            },
            printReport: function (itemMenu) {
                var me = this,
                    frame = Ext.get('downloadIframe');
                if (frame != null) {
                    Ext.destroy(frame);
                }
                debugger;
                me.getUserParams(itemMenu.actionName);

                if (Ext.JSON.decode(me.params.recIds).length == 0) {
                    Ext.Msg.alert('Ошибка', 'Необходимо выбрать хотя бы одну запись для печати');
                    return;
                }

                Ext.apply(me.params, { reportId: itemMenu.actionName });
                debugger;
                me.mask('Обработка...');
                B4.Ajax.request({
                    url: B4.Url.action('ReportLawsuitOnwerPrint', 'ComissionMeetingReport'),
                    params: me.params,
                    timeout: 9999999
                })
                    .next(function (resp) {
                        var tryDecoded;

                        me.unmask();
                        try {
                            tryDecoded = Ext.JSON.decode(resp.responseText);
                        } catch (e) {
                            tryDecoded = {};
                        }

                        var id = resp.data ? resp.data : tryDecoded.data;
                        debugger;
                        if (id > 0) {
                            Ext.DomHelper.append(document.body,
                                {
                                    tag: 'iframe',
                                    id: 'downloadIframe',
                                    frameBorder: 0,
                                    width: 0,
                                    height: 0,
                                    css: 'display:none;visibility:hidden;height:0px;',
                                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                                });

                            me.fireEvent('onprintsucess', me);
                        }
                    })
                    .error(function (err) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка', err.message || err);
                    });
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'protocolButtonExportAspect',
            gridSelector: '#docsGjiRegisterProtocolGrid',
            buttonSelector: '#docsGjiRegisterProtocolGrid #btnExport',
            controllerName: 'Protocol',
            actionName: 'Export'
        },
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrProtocolGridEditFormAspect',
            gridSelector: '#docsGjiRegisterProtocolGrid',
            modelName: 'ProtocolGji',
            storeName: 'view.ProtocolGji'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterProtocolStateTransferAspect',
            gridSelector: '#docsGjiRegisterProtocolGrid',
            stateType: 'gji_document_prot',
            menuSelector: 'docsGjiRegisterProtocolGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('ProtocolGji');
                    model.load(record.getId(), {
                        success: function (rec) {
                            record.set('DocumentNumber', rec.get('DocumentNumber'));
                        },
                        scope: this
                    });
                }
            }
        }
    ],

    init: function () {
        var me = this;
        this.getStore('view.ProtocolGji').on('beforeload', this.onBeforeLoad, this);
        me.control({
            'registerprotocol2025grid': { 'render': { fn: me.onMainViewRender, scope: me } }
        });
        this.callParent(arguments);
    },

    onMainViewRender: function (grid) {
        var me = this;
        debugger;
        grid.getStore('view.ProtocolGji').load;    
        me.getAspect('documentgjimassbuttonProtocolPrintAspect').loadReportStore('docregistry');
    },

    onLaunch: function () {
        this.getStore('view.ProtocolGji').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {

            if (this.params.filterParams.realityObjectId)
                operation.params.realityObjectId = this.params.filterParams.realityObjectId;

            if (this.params.filterParams.dateStart)
                operation.params.dateStart = this.params.filterParams.dateStart;

            if (this.params.filterParams.dateEnd)
                operation.params.dateEnd = this.params.filterParams.dateEnd;
        }
    }
});