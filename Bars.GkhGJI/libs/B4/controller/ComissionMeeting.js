Ext.define('B4.controller.ComissionMeeting', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.ComissionMeetingButtonPrintAspect',
        'B4.ux.grid.column.Enum',
        'B4.aspects.GkhBlobText',
        'B4.enums.ComissionDocumentDecision',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.view.appealcits.DetailsEditWindow',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'comission.ComissionMeeting',
        'comission.ComissionMeetingInspector',
        'dict.Inspector',
        'dict.Subpoena',
        'ProtocolGji',
        'Resolution',
        'resolution.Definition'
       
    ],
    stores: [
        'comission.ComissionMeeting',
        'comission.ComissionMeetingInspector',
        'dict.InspectorForSelect',
        'comission.ListProtocols',
        'comission.ListResolutions',
        'comission.ListResolutionDefinitions',
        'comission.ListSubpoena',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected'
    ],
    views: [
        'comission.Grid',
        'comission.EditWindow',
        'comission.InspectorGrid',
        'comission.InspectorEditWindow',
        'comission.ProtocolGrid',
        'comission.ResolutionGrid',
        'comission.ResolutionDefinitionGrid',
        'comission.SubpoenaGrid',
        'comission.DecisionWindow',
        'comission.MassDecisionWindow',
        'resolution.action.BaseResolutionWindow',
        'resolution.action.SumAmountWindow'
    ],
    mainView: 'comission.Grid',
    mainViewSelector: 'comissiongrid',
    globalAppeal: null,
    comissionId: null,
    comissionmeetId: null,
    refs: [
        {
            ref: 'mainView',
            selector: 'comissiongrid'
        },
        {
            ref: 'comissionEditWindow',
            selector: 'comissionEditWindow'
        },
        {
            ref: 'protocolView',
            selector: 'comissionprotocolgrid'
        },
        {
            ref: 'resolutionView',
            selector: 'comissionresolutiongrid'
        },
        {
            ref: 'resolutionDefinitionView',
            selector: 'comissionresolutiondefinitiongrid'
        }
    ],
    comission :null,

    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'ComissionMeetingMainPrintAspect',
            buttonSelector: '#comissionEditWindow #btnPrint',
            codeForm: 'ComissionReport',
            getUserParams: function () {
                var param = { Id: this.controller.comissionmeetId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'comissionmeetingbuttonprintaspect',
            name: 'comissionmeetingPrintAspect',
            buttonSelector: 'comissionprotocolgrid gkhbuttonprint[action=DocumentComissionPrint]',
            codeForm: 'ComissionMeetingReport, CourtClaimDirectedOwner, CourtClaimShared, CourtClaimUnderage,CourtClaimDirectedShared, CourtClaimDirectedUnderage, CourtOrderPrMulti, ExecutiveClaim, ExecutiveClaimRepeat',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    grid = me.controller.getProtocolView(),
                    records = grid.getSelectionModel().getSelection(),
                    recIds = [],
                    param = { comissionId: me.controller.comissionmeetId };
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
            xtype: 'comissionmeetingbuttonprintaspect',
            name: 'comissionmeetingResolutionPrintAspect',
            buttonSelector: 'comissionresolutiongrid gkhbuttonprint[action=ResolutionPrint]',
            codeForm: 'Resolution',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    grid = me.controller.getResolutionView(),
                    records = grid.getSelectionModel().getSelection(),
                    recIds = [],
                    param = { comissionId: me.controller.comissionmeetId };
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
            xtype: 'comissionmeetingbuttonprintaspect',
            name: 'comissionmeetingResolutionDefinitionPrintAspect',
            buttonSelector: 'comissionresolutiondefinitiongrid gkhbuttonprint[action=ResolutionDefinitionPrint]',
            codeForm: 'ResolutionDefinition',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    grid = me.controller.getResolutionDefinitionView(),
                    records = grid.getSelectionModel().getSelection(),
                    recIds = [],
                    param = { comissionId: me.controller.comissionmeetId };
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
            name: 'comissiongridButtonExportAspect',
            gridSelector: 'comissiongrid',
            buttonSelector: 'comissiongrid #btnExport',
            controllerName: 'GjiScript',
            actionName: 'MKDLicRequestExport'
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'comissiongridStateTransferAspect',
            gridSelector: 'comissiongrid',
            stateType: 'adm_comission_meeting',
            menuSelector: 'comissiongridStateMenu'
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'comissionStateButtonAspect',
            stateButtonSelector: '#comissionEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    var model = this.controller.getModel('comission.ComissionMeeting');
                    model.load(entityId, {
                        success: function (rec) {
                            this.controller.getAspect('comissiongridGridAspect').setFormData(rec);
                        },
                        scope: this
                    })


                }
            }
        },
        //{
        //    xtype: 'gkhblobtextaspect',
        //    name: 'commitDescriptLTAspect',
        //    fieldSelector: '[name=Description]',
        //    editPanelAspectName: 'comissionDecGridAspect',
        //    controllerName: 'ComissionMeetingOperation',
        //    valueFieldName: 'Description',
        //    previewLength: 2000,
        //    autoSavePreview: true,
        //    previewField: false
        //},
        //{
        //    xtype: 'grideditwindowaspect',
        //    name: 'comissionDecGridAspect',
        //    gridSelector: 'comissionprotocolgrid',
        //    editFormSelector: '#comissiondecisionwindow',
        //    storeName: 'comission.ListProtocols',
        //    modelName: 'ProtocolGji',
        //    editWindowView: 'comission.DecisionWindow'
        //},
        {
            xtype: 'grideditwindowaspect',
            name: 'comissiongridGridAspect',
            gridSelector: 'comissiongrid',
            editFormSelector: '#comissionEditWindow',
            storeName: 'comission.ComissionMeeting',
            modelName: 'comission.ComissionMeeting',
            editWindowView: 'comission.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения

                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },           
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    comission = record.getId();
                    asp.controller.comission = record.getId();
                    //пытаемся проставить прочитано для инспектора и/или руководителя управления
                    if (record.getId()) {
                        me.controller.getAspect('ComissionMeetingMainPrintAspect').loadReportStore();
                        asp.controller.comissionmeetId = record.getId();
                        asp.controller.comissionId = record.get('ZonalInspection');
                        asp.controller.getAspect('comissionStateButtonAspect').setStateData(record.getId(), record.get('State'));
                        var inspgrid = form.down('comissioninspectorgrid'),
                            inspstore = inspgrid.getStore();
                        inspgrid.setDisabled(false);
                        inspstore.filter('comissionmeetId', record.getId());

                        var subpoenagrid = form.down('subpoenaGrid');
                        var substore = subpoenagrid.getStore();
                        substore.removeAll();
                        substore.on('beforeload',
                            function (store, operation) {
                                operation.params.comissionmeetId = record.getId();
                            },
                            me);
                        substore.load();

                        var protgrid = form.down('comissionprotocolgrid');
                        var protstore = protgrid.getStore();
                        protstore.removeAll();
                        protstore.on('beforeload',
                            function (store, operation) {
                                operation.params.comissionmeetId = record.getId();
                            },
                            me);
                        protstore.load();

                        var resolgrid = form.down('comissionresolutiongrid');
                        var resolstore = resolgrid.getStore();
                        resolstore.removeAll();
                        resolstore.on('beforeload',
                            function (store, operation) {
                                operation.params.comissionmeetId = record.getId();
                            },
                            me);
                        resolstore.load();

                        var resoldefgrid = form.down('comissionresolutiondefinitiongrid');
                        var resoldefstore = resoldefgrid.getStore();
                        resoldefstore.removeAll();
                        resoldefstore.on('beforeload',
                            function (store, operation) {
                                operation.params.comissionmeetId = record.getId();
                            },
                            me);
                        resoldefstore.load();

                        me.loadProtocolOperations(protgrid);
                        me.loadResolutionOperations(resolgrid);

                        asp.controller.getAspect('comissionmeetingPrintAspect').loadReportStore('prot');
                        asp.controller.getAspect('comissionmeetingResolutionPrintAspect').loadReportStore('res');
                        asp.controller.getAspect('comissionmeetingResolutionDefinitionPrintAspect').loadReportStore('resdef');
                    }
                    else
                    {                       
                        var inspgrid = form.down('comissioninspectorgrid'),
                            inspstore = rogrid.getStore();
                        inspstore.clearData();
                        inspgrid.setDisabled(true);
                        var protgrid = form.down('comissionprotocolgrid');
                        var protstore = protgrid.getStore();
                        protstore.removeAll();
                    }
                   
                }
            },
            loadProtocolOperations: function (grid) {
                var menuButton = grid.down('button[name=comissionoperation]');

                B4.Ajax.request(
                    B4.Url.action('ListProtocolOperations', 'ComissionMeetingOperation')
                ).next(function (r) {
                    var list = Ext.decode(r.responseText);

                    menuButton.menu.removeAll();

                    if (list && list.data) {
                        Ext.each(list.data, function (item) {
                            menuButton.menu.add({
                                xtype: 'menuitem',
                                text: item.Name,
                                action: item.Code.toLowerCase()
                            });
                        });
                    }
                });

            },
            loadResolutionOperations: function (grid) {
                var menuButton = grid.down('button[name=resolutionoperation]');

                B4.Ajax.request(
                    B4.Url.action('ListResolutionOperations', 'Resolution')
                ).next(function (r) {
                    var list = Ext.decode(r.responseText);

                    menuButton.menu.removeAll();

                    if (list && list.data) {
                        Ext.each(list.data, function (item) {
                            menuButton.menu.add({
                                xtype: 'menuitem',
                                text: item.Name,
                                action: item.Code.toLowerCase()
                            });
                        });
                    }
                });

            }
        },
        //{
        //    xtype: 'grideditwindowaspect',
        //    name: 'comissionInspectorGridAspect',
        //    gridSelector: 'comissioninspectorgrid',
        //    editFormSelector: '#comissionInspectorEditWindow',
        //    storeName: 'comission.ComissionMeetingInspector',
        //    modelName: 'comission.ComissionMeetingInspector',
        //    editWindowView: 'comission.InspectorEditWindow',
        //    otherActions: function (actions) {             
        //        actions[this.editFormSelector + ' #sfInspector'] = { 'beforeload': { fn: this.onBeforeLoadInspector, scope: this } };
        //    },
        //    listeners: {
        //        getdata: function (asp, record) {
        //            if (!record.get('Id')) {
        //                record.set('ComissionMeeting', asp.controller.comissionmeetId);
        //            }
        //        }
        //    },
        //    onBeforeLoadInspector: function (field, options, store) {

        //        debugger;
        //        options = options || {};
        //        options.params = options.params || {};

        //        options.params.zonalInspectionIds = this.controller.comissionId;

        //        return true;
        //    },
        //},
        {
            /* 
            * Аспект взаимодействия таблицы статьи закона с массовой формой выбора статей
            * По нажатию на Добавить открывается форма выбора статей.
            * По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            * И сохранение статей
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'comissionInspectorAspect',
            gridSelector: 'comissioninspectorgrid',
            saveButtonSelector: 'comissioninspectorgrid #inspectorSaveButton',
            storeName: 'comission.ComissionMeetingInspector',
            modelName: 'comission.ComissionMeetingInspector',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#comissionInspectorMultiSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            titleSelectWindow: 'Выбор членов комиссии',
            titleGridSelect: 'Сотрудники для отбора',
            titleGridSelected: 'Выбранные сотрудники',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Должность', xtype: 'b4enumcolumn', enumName: 'B4.enums.TypeCommissionMember', dataIndex: 'TypeCommissionMember', flex: 1, filter: true }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.zonalInspectionIds = this.controller.comissionId;
                operation.params.onlyActive = true;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {

                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddMembers', 'ComissionMeetingOperation'),
                            method: 'POST',
                            params: {
                                inspectorIds: Ext.encode(recordIds),
                                commeetingId: asp.controller.comissionmeetId
                            }
                        }).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать членов комиссии');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    mainView: 'comission.Grid',
    mainViewSelector: 'comissiongrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {

        this.params = {};
        var view = this.getMainView() || Ext.widget('comissiongrid');      
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('comission.ComissionMeeting').load();
    },

    onLaunch: function () {
        debugger;
        var grid = this.getMainView();
        if (this.params && this.params.recId > 0) {
            var model = this.getModel('comission.ComissionMeeting');
            this.getAspect('comissiongridGridAspect').editRecord(new model({ Id: this.params.recId }));
            this.params.recId = 0;
        }
    },

    init: function () {
        var me = this,
        actions = {};
      //  actions['comissionprotocolgrid'] = { gotoprotocol: { fn: this.rowActionProt, scope: this } };
        actions['comissionprotocolgrid'] = {
            'gotoprotocol': { fn: this.rowActionProt, scope: this },
            'editpotocol': { fn: this.editProt, scope: this }
        };
        actions['comissionresolutiongrid'] = {
            'gotoresolution': { fn: this.rowActionResol, scope: this }
        };
        actions['comissionprotocolgrid button[name=comissionoperation] menuitem'] = { click: { fn: this.onprotocolOperationClick, scope: this } };
        actions['comissionresolutiongrid button[name=resolutionoperation] menuitem'] = { click: { fn: this.onresolutionOperationClick, scope: this } };
       // actions['comissionprotocolgrid'] = { 'editpotocol': { fn: this.editProt, scope: this } };
        actions['comissiondecisionwindow b4savebutton'] = { click: { fn: me.onApplyDecisionOperation, scope: me } }
        actions['comissiondecisionwindow #cbComissionDocumentDecision'] = { change: { fn: me.onchangecbComissionDocumentDecision, scope: me } }
        actions['comissionmassdecisionwindow b4savebutton'] = { click: { fn: me.onApplyMassDecisionOperation, scope: me } }
        actions['comissionprotocolgrid b4updatebutton'] = { click: { fn: me.onUpdateProtocolGrid, scope: me } }
        actions['comissionresolutiongrid b4updatebutton'] = { click: { fn: me.onUpdateResolutionGrid, scope: me } }
        actions['sumamountwindow b4savebutton'] = { click: { fn: me.onApplyResolutionOperation, scope: me } }
        actions['comissionmassdecisionwindow #cbComissionDocumentDecision'] = { change: { fn: me.onchangecbMassComissionDocumentDecision, scope: me } }
        actions['comissionmassdecisionwindow #taDescription'] = { 'focus': { fn: this.showBigDescription, scope: this } };
        actions['comissiondecisionwindow #taDescription'] = { 'focus': { fn: this.showBigDescription, scope: this } };
        me.control(actions);
        me.params = {};
        this.getStore('comission.ComissionMeeting').on('beforeload', this.onBeforeLoadDoc, this);
        me.callParent(arguments);
    },

    onUpdateProtocolGrid: function () {
        var str = this.getStore('comission.ListProtocols');
        str.load();
    },
    onUpdateResolutionGrid: function () {
        var str = this.getStore('comission.ListResolutions');
        str.load();
    },

    showBigDescription: function (field, data, record) {
        var currentDescriptonText = field.getValue();
        win = Ext.create('B4.view.appealcits.DetailsEditWindow');
        var valuefield = win.down('#dfDescription');
        var closebutton = win.down('b4closebutton');
        var savebutton = win.down('b4savebutton');
        valuefield.setValue(currentDescriptonText);
        win.show();
        closebutton.addListener('click', function () {
            win.close();
        });
        savebutton.addListener('click', function () {
            currentDescriptonText = valuefield.getValue();
            field.setValue(currentDescriptonText);
            win.close();
        });

    },

    onApplyMassDecisionOperation: function (btn) {
        var me = this,
            win = btn.up('comissionmassdecisionwindow');
        var cbComissionDocumentDecision = win.down('#cbComissionDocumentDecision');
        var dfNextCommissionDate = win.down('#dfNextCommissionDate');
        var nfHourOfProceedings = win.down('#nfHourOfProceedings');
        var nfMinuteOfProceedings = win.down('#nfMinuteOfProceedings');
        var minuteOfProceedings = nfMinuteOfProceedings.getValue();
        var hourOfProceedings = nfHourOfProceedings.getValue();
        var taDescription = win.down('#taDescription');
        var decValue = cbComissionDocumentDecision.getValue();
        var docIds = win.protocolIds;
        debugger;
        var nextCommisDate = dfNextCommissionDate.getValue();
        var description = taDescription.getValue();
        thiscomission = me.comissionmeetId;
        me.mask('Установка значения', win);
        B4.Ajax.request(B4.Url.action('UpdateComissionDocumentState', 'Protocol197', {
            documentIds: Ext.encode(docIds),
            comissionId: thiscomission,
            decValue: decValue,
            nextCommisDate: nextCommisDate,
            description: description,
            massOperation: true,
            hourOfProceedings: hourOfProceedings,
            minuteOfProceedings: minuteOfProceedings
        })).next(function (response) {
            //десериализуем полученную строку

            me.unmask();
        }).error(function () {
            debugger;
            me.unmask();
        });
        win.close();
        var store = me.getStore('comission.ListProtocols');
        store.load();

    },

    onprotocolOperationClick: function (item) {
        debugger;
        var me = this,
            recs = me.getProtocolView().getSelectionModel().getSelection(),
            action = item.action.toLowerCase(),
            rec,
            operationRules = {
                'createresolution': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор протокола', 'Необходимо выбрать хотя бы один протокол!');
                        return false;
                    }
                    return true;
                },
                'viewincomission': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор протокола', 'Необходимо выбрать хотя бы один протокол!');
                        return false;
                    }
                    return true;
                },
            },
            rule = operationRules[action];
        if (rule(recs) === false) {
            return false;
        }

        rec = recs[0];
        switch (action) {
            case 'createresolution':
                me.showConfirmWindow(recs);
                break;
            case 'viewincomission':
                me.showMassDecisionWindow(recs);
                break;
        }
    },

    onresolutionOperationClick: function (item) {
        debugger;
        var me = this,
            recs = me.getResolutionView().getSelectionModel().getSelection(),
            action = item.action.toLowerCase(),
            rec,
            operationRules = {
                'sumamountoperation': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор постановления', 'Необходимо выбрать хотя бы одно постановление!');
                        return false;
                    }
                    return true;
                },
            },
            rule = operationRules[action];
        if (rule(recs) === false) {
            return false;
        }

        rec = recs[0];
        switch (action) {
            case 'sumamountoperation':
                me.showSumAmountWindow(recs);
                break;
        }
    },

    showSumAmountWindow: function (recs) {
        var resolutionIds = [],
            win = Ext.widget('sumamountwindow');

        Ext.each(recs, function (rec) {
            resolutionIds.push(rec.getId());
        });

        win.resolutionIds = resolutionIds;
        debugger;
        win.show();
    },

    onApplyResolutionOperation: function (btn) {
        var me = this,
            win = btn.up('sumamountwindow'),
            params = {};
        params.resolutionIds = Ext.JSON.encode(win.resolutionIds);
        debugger;
        me.mask('Изменение суммы штрафа...', win.getEl());
        win.down('form').submit({
            url: B4.Url.action('SetSumAmount', 'Resolution', { 'b4_pseudo_xhr': true }),
            params: params,
            success: function (f, action) {
                var json = Ext.JSON.decode(action.response.responseText),
                    grid = me.getResolutionView();

                me.unmask();
                if (json.data.data) {
                    Ext.Msg.alert('Результат', 'Выполнено успешно');

                    win.close();
                    if (grid) {
                        grid.getStore().load();
                    }
                }
                else {
                    Ext.Msg.alert('Результат', 'Неправильная сумма штрафа');

                }


            },
            failure: function (f, action) {
                me.unmask();
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Результат', json.message);
                win.close();
            }
        });
    },

    showMassDecisionWindow: function (recs) {
        var me = this,
            documentIds = [],
            thiscomission = me.comissionmeetId;
        debugger;
        Ext.each(recs, function (rec) {
            documentIds.push(rec.getId());
        });

        if (thiscomission) {
            win = Ext.widget('comissionmassdecisionwindow');
            var cbComissionDocumentDecision = win.down('#cbComissionDocumentDecision');
            cbComissionDocumentDecision.setValue(20);
            win.setTitle('Решение по выбранным записям');
            win.protocolIds = documentIds;
            win.show();
            //  this.getAspect('commitDescriptLTAspect').doInjection();
        }
    },

    showConfirmWindow: function (recs) {
        var me = this,
        documentIds = [],
        thiscomission = me.comissionmeetId;         

        Ext.each(recs, function (rec) {
            documentIds.push(rec.getId());
        });
        Ext.Msg.confirm('Решение по документу!', 'Вы действительно хотите сформировать постановления?', function (result) {
            if (result == 'yes') {
                this.mask('Формирование постановлений', B4.getBody());
                B4.Ajax.request(B4.Url.action('UpdateComissionDocumentsState', 'Protocol197', {
                    documentIds: Ext.encode(documentIds),
                    massOperation: true,
                    comissionId: thiscomission,
                    decValue: B4.enums.ComissionDocumentDecision.Resolution                
                })).next(function (response) {
                    //десериализуем полученную строку

                    me.unmask();
                }).error(function (e) {
                    Ext.Msg.alert('Ошибка', e.message);
                    me.unmask();
                });

            }
        }, this);      
    },

    onchangecbComissionDocumentDecision: function (field, newValue) {
        var form = field.up('comissiondecisionwindow'),
            dfNextCommissionDate = form.down('#dfNextCommissionDate'),
            nfHourOfProceedings = form.down('#nfHourOfProceedings'),
            nfMinuteOfProceedings = form.down('#nfMinuteOfProceedings'),
            dfcntNextCommissionDate = form.down('#dfcntNextCommissionDate'),
            taDescription = form.down('#taDescription');

        if (newValue == B4.enums.ComissionDocumentDecision.NewComisison) {
            dfcntNextCommissionDate.show();
        }
        else if (newValue == B4.enums.ComissionDocumentDecision.Decline) {
            dfcntNextCommissionDate.show();
            taDescription.show();
        }
        else {
            dfNextCommissionDate.setValue(null);
            taDescription.setValue(null);
            nfHourOfProceedings.setValue(null);
            nfMinuteOfProceedings.setValue(null);
            dfcntNextCommissionDate.hide();
            taDescription.hide();
        }
    },

    onchangecbMassComissionDocumentDecision: function (field, newValue) {
        var form = field.up('comissionmassdecisionwindow'),
            dfNextCommissionDate = form.down('#dfNextCommissionDate'),
            nfHourOfProceedings = form.down('#nfHourOfProceedings'),
            nfMinuteOfProceedings = form.down('#nfMinuteOfProceedings'),
            dfcntNextCommissionDate = form.down('#dfcntNextCommissionDate'),
        taDescription = form.down('#taDescription');

        if (newValue == B4.enums.ComissionDocumentDecision.NewComisison) {
            dfcntNextCommissionDate.show();
        }
        else if (newValue == B4.enums.ComissionDocumentDecision.Decline)
        {
            dfcntNextCommissionDate.show();
            taDescription.show();
        }
        else {
            dfNextCommissionDate.setValue(null);
            taDescription.setValue(null);
            nfHourOfProceedings.setValue(null);
            nfMinuteOfProceedings.setValue(null);
            dfcntNextCommissionDate.hide();
            taDescription.hide();
        }
    },

    onApplyDecisionOperation: function (btn) {
        debugger;
        var me = this,
            win = btn.up('comissiondecisionwindow');
        var cbComissionDocumentDecision = win.down('#cbComissionDocumentDecision');
        var dfNextCommissionDate = win.down('#dfNextCommissionDate');
        var nfHourOfProceedings = win.down('#nfHourOfProceedings');
        var nfMinuteOfProceedings = win.down('#nfMinuteOfProceedings');
        var minuteOfProceedings = nfMinuteOfProceedings.getValue();
        var hourOfProceedings = nfHourOfProceedings.getValue();
        var taDescription = win.down('#taDescription');
        var decValue = cbComissionDocumentDecision.getValue();
        var docId = win.protocolId;
        var nextCommisDate = dfNextCommissionDate.getValue();
        var description = taDescription.getValue();
        thiscomission = me.comissionmeetId;
        me.mask('Установка значения', win);
        B4.Ajax.request(B4.Url.action('UpdateComissionDocumentState', 'Protocol197', {
            documentId: docId,
            comissionId: thiscomission,
            decValue: decValue,
            nextCommisDate: nextCommisDate,
            description: description,
            hourOfProceedings: hourOfProceedings,
            minuteOfProceedings: minuteOfProceedings
        })).next(function (response) {
            //десериализуем полученную строку
          
            me.unmask();
        }).error(function () {
            debugger;
            me.unmask();
        });
        win.close();
        var store = me.getStore('comission.ListProtocols');
        store.load();

    },

    editProt: function (record) {     
        debugger;
        var me = this,
            documentGji = record.get('Id'),
            currentDec = record.get('ComissionDocumentDecision'),
            thisprotnumber = record.get('DocumentNumber'),
            thiscomission = me.comissionmeetId;
        if (thiscomission) {
            win = Ext.widget('comissiondecisionwindow');
            var cbComissionDocumentDecision = win.down('#cbComissionDocumentDecision');
            cbComissionDocumentDecision.setValue(currentDec);
            win.setTitle('Решение по протоколу ' + thisprotnumber);
            win.protocolId = documentGji;
            win.show();           
          //  this.getAspect('commitDescriptLTAspect').doInjection();
        }
 

    },

    rowActionProt: function (record) {
        debugger;
        var me = this,
            typeReminder = 10,
            portal = me.getController('PortalController'),
            controllerEditName,
            params = {},
            model,
            documentGji,
            inspection,
            defaultParams;

        switch (typeReminder) {

            default:
                {
                    documentGji = record.get('Id');
                    inspection = record.get('InspectionId');
                    model = me.getModel('InspectionGji');
                    var typeBase = record.get('TypeBase');
                    var TypeDocumentGji = record.get('TypeDocumentGji');
                    controllerEditName = me.getControllerName(typeBase);
                    params = new model({ Id: inspection });

                    // Получаем тип документа, в зависимости от типа задаем имя дефолтного контроллера (откроется вкладка по умолчанию) и дефолтных параметров
                    if (documentGji) {
                        defaultParams = me.getDefaultParams(TypeDocumentGji);
                        params.defaultController = defaultParams.controllerName;
                        params.defaultParams = {
                            inspectionId: inspection,
                            documentId: documentGji,
                            title: defaultParams.docName
                        };
                    }
                    if (controllerEditName) {
                        portal.loadController(controllerEditName, params);
                    }
                }
                break;
        }

    },

    rowActionResol: function (record) {
        debugger;
        var me = this,
            typeReminder = 10,
            portal = me.getController('PortalController'),
            controllerEditName,
            params = {},
            model,
            documentGji,
            inspection,
            defaultParams;

        switch (typeReminder) {

            default:
                {
                    documentGji = record.get('Id');
                    inspection = record.get('InspectionId');
                    model = me.getModel('InspectionGji');
                    var typeBase = record.get('TypeBase');
                    var TypeDocumentGji = record.get('TypeDocumentGji');
                    controllerEditName = me.getControllerName(typeBase);
                    params = new model({ Id: inspection });
                    // Получаем тип документа, в зависимости от типа задаем имя дефолтного контроллера (откроется вкладка по умолчанию) и дефолтных параметров
                    if (documentGji) {
                        defaultParams = me.getDefaultParams(TypeDocumentGji);
                        params.defaultController = defaultParams.controllerName;
                        params.defaultParams = {
                            inspectionId: inspection,
                            documentId: documentGji,
                            title: 'Постановление'
                        };
                    }
                    if (controllerEditName) {
                        portal.loadController(controllerEditName, params);
                    }
                }
                break;
        }

    },

    onBeforeLoadDoc: function (store, operation) {
        operation.params.isFiltered = true;
    },


    getControllerName: function (typeBase) {
        switch (typeBase) {
            //Инспекционная проверка
            case 10:
                return 'B4.controller.baseinscheck.Navigation';
            //Обращение граждан                  
            case 20:
                return 'B4.controller.basestatement.Navigation';
            //Плановая проверка юр лиц                
            case 30:
                return 'B4.controller.basejurperson.Navigation';
            //Распоряжение руководства               
            case 40:
                return 'B4.controller.basedisphead.Navigation';
            //Требование прокуратуры                 
            case 50:
                return 'B4.controller.baseprosclaim.Navigation';
            //Постановление прокуратуры                  
            case 60:
                return 'B4.controller.resolpros.Navigation';
            //Проверка деятельности ТСЖ                   
            case 70:
                return 'B4.controller.baseactivitytsj.Navigation';
            //Отопительный сезон                    
            case 80:
                return 'B4.controller.baseheatseason.Navigation';
            //Административное дело
            case 90:
                return '';
            //Протокол МВД
            case 100:
                return 'B4.controller.protocolmvd.Navigation';
            //Проверка по плану мероприятий
            case 110:
                return 'B4.controller.baseplanaction.Navigation';
            //Протокол МЖК
            case 120:
                return 'B4.controller.protocolmhc.Navigation';
            case 140:
                return 'B4.controller.protocol197.Navigation';
            //Без основания                     
            case 150:
                return 'B4.controller.basedefault.Navigation';
        }

        return '';
    },

    getDefaultParams: function (typeDocument) {
        var result = {};
        switch (typeDocument) {
            //Распоряжение
            case 10:
                {
                    result.controllerName = 'B4.controller.Disposal';
                    result.docName = B4.DisposalTextValues.getSubjectiveCase();
                }
                break;
            //Предписание
            case 50:
                {
                    result.controllerName = 'B4.controller.Prescription';
                    result.docName = 'Предписание';
                }
                break;
            //Постановление
            case 70:
                {
                    result.controllerName = 'B4.controller.Resolution';
                    result.docName = 'Постановление';
                }
                break;

            //Постановление
            case 140:
                {
                    result.controllerName = 'B4.controller.protocol197.Edit';
                    result.docName = 'Протокол 19.7';
                }
                break;
            case 60:
                {
                    result.controllerName = 'B4.controller.ProtocolGji';
                    result.docName = 'Протокол';
                }
                break;
        }

        return result;
    },
});