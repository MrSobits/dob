Ext.define('B4.controller.documentsgjiregister.Resolution', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GjiDocumentRegister',
        'B4.aspects.StateContextMenu',
        'Ext.tree.Panel',
        'B4.form.Window',
        'B4.form.FileField',
        'B4.aspects.regop.PersAccImportAspect',
        'B4.aspects.DocumentGjiMassButtonPrintAspect',
        'B4.aspects.GridEditCtxWindow',
        'B4.mixins.Context',
        'B4.aspects.GridEditForm',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ButtonDataExport',
        'Ext.ux.data.PagingMemoryProxy',
        'Ext.ux.IFrame'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'Resolution',
        'InspectionGji'
    ],
    stores: ['view.Resolution'],

    views: [
        'documentsgjiregister.ResolutionGrid',
        'resolution.action.BaseResolutionWindow',
        'resolution.action.SumAmountWindow',
        'resolution.action.DateProtocolWindow',
        'resolution.action.ChangeOSPWindow',
        'resolution.action.ChangeSentToOSPWindow'
    ],

    mainView: 'documentsgjiregister.ResolutionGrid',
    mainViewSelector: '#docsGjiRegisterResolutionGrid',

    aspects: [
        {
            xtype: 'documentgjimassbuttonprintaspect',
            name: 'documentgjimassbuttonResolutionPrintAspect',
            buttonSelector: 'registerresolutiongrid gkhbuttonprint[action=ResolutionPrint]',
            codeForm: 'Resolution',
            reportstoremethod: '/ComissionMeetingReport/GetResolutionReportList',
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
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrResolutionGridEditFormAspect',
            gridSelector: '#docsGjiRegisterResolutionGrid',
            modelName: 'Resolution',
            storeName: 'view.Resolution',

        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'resolutionButtonExportAspect',
            gridSelector: '#docsGjiRegisterResolutionGrid',
            buttonSelector: '#docsGjiRegisterResolutionGrid #btnExport',
            controllerName: 'Resolution',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterResolutionStateTransferAspect',
            gridSelector: '#docsGjiRegisterResolutionGrid',
            stateType: 'gji_document_resol',
            menuSelector: 'docsGjiRegisterResolutionGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('Resolution');
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
        me.control({
            'registerresolutiongrid': { 'render': { fn: me.onMainViewRender, scope: me } },
            'registerresolutiongrid button[name=resolutionoperation] menuitem': { 'click': { fn: me.onResolutionOperationClick, scope: me } },
            'sumamountwindow b4savebutton': { click: { fn: me.onApplyResolutionOperation, scope: me } },
            'resolutiondateprotocolwindow b4savebutton': { click: { fn: me.onApplyCreateProtocolOperation, scope: me } },
            'changeospwindow b4savebutton': { click: { fn: me.onApplyChangeOSPOperation, scope: me } },
            'changesenttoospwindow b4savebutton': { click: { fn: me.onApplyChangeSentToOSPOperation, scope: me } }
        });

        me.getStore('view.Resolution').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    onMainViewRender: function (grid) {
        var me = this;

        grid.getStore('view.Resolution').load;
        me.loadResolutionOperations(grid);
        me.getAspect('documentgjimassbuttonResolutionPrintAspect').loadReportStore('docregistry');
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

    },

    onResolutionOperationClick: function (item) {
        var me = this,
            recs = me.getMainView().getSelectionModel().getSelection(),
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
                'create2025operation': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор постановления', 'Необходимо выбрать хотя бы одно постановление!');
                        return false;
                    }
                    return true;
                },
                'changeospoperation': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор постановления', 'Необходимо выбрать хотя бы одно постановление!');
                        return false;
                    }
                    return true;
                },
                'changesenttoospoperation': function (records) {
                    if (!records || records.length < 1) {
                        Ext.Msg.alert('Выбор постановления', 'Необходимо выбрать хотя бы одно постановление!');
                        return false;
                    }
                    return true;
                }
            },
            rule = operationRules[action];
        debugger;
        if (rule(recs) === false) {
            return false;
        }

        rec = recs[0];
        switch (action) {
            case 'sumamountoperation':
                me.showSumAmountWindow(recs);
                break;
            case 'create2025operation':
                me.show2025Window(recs);
                break;
            case 'changeospoperation':
                me.showOSPWindow(recs);
                break;
            case 'changesenttoospoperation':
                me.showSentToOSPWindow(recs);
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
        win.show();
    },

    show2025Window: function (recs) {
        var resolutionIds = [],
            win = Ext.widget('resolutiondateprotocolwindow');

        Ext.each(recs, function (rec) {
            resolutionIds.push(rec.getId());
        });

        win.resolutionIds = resolutionIds;
        debugger;
        win.show();
    },

    showOSPWindow: function (recs) {
        var resolutionIds = [],
            win = Ext.widget('changeospwindow');

        Ext.each(recs, function (rec) {
            resolutionIds.push(rec.getId());
        });

        win.resolutionIds = resolutionIds;
        debugger;
        win.show();
    },

    showSentToOSPWindow: function (recs) {
        var resolutionIds = [],
            win = Ext.widget('changesenttoospwindow');

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
                    grid = me.getMainView();

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

    onApplyCreateProtocolOperation: function (btn) {
        debugger;
        var me = this,
            win = btn.up('resolutiondateprotocolwindow'),
            params = {};
        params.resolutionIds = Ext.JSON.encode(win.resolutionIds);
        debugger;
        me.mask('Создание протоколов...', win.getEl());
        win.down('form').submit({
            url: B4.Url.action('CreateProtocols', 'Resolution', { 'b4_pseudo_xhr': true }),
            params: params,
            success: function (f, action) {
                var json = Ext.JSON.decode(action.response.responseText),
                    grid = me.getMainView();

                me.unmask();
                if (json.data.data) {
                    Ext.Msg.alert('Результат', 'Выполнено успешно');

                    win.close();
                    if (grid) {
                        grid.getStore().load();
                    }
                }
                else {
                    Ext.Msg.alert('Результат', 'Ошибка');

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

    onApplyChangeOSPOperation: function (btn) {
        var me = this,
            win = btn.up('changeospwindow'),
            params = {};
        params.resolutionIds = Ext.JSON.encode(win.resolutionIds);
        params.ospId = win.getParams().ospId;
        debugger;
        me.mask('Изменение отдела судебных приставов...', win.getEl());
        win.down('form').submit({
            url: B4.Url.action('ChangeOSP', 'Resolution', { 'b4_pseudo_xhr': true }),
            params: params,
            success: function (f, action) {
                var json = Ext.JSON.decode(action.response.responseText),
                    grid = me.getMainView();

                me.unmask();
                if (json.data.data) {
                    Ext.Msg.alert('Результат', 'Выполнено успешно');

                    win.close();
                    if (grid) {
                        grid.getStore().load();
                    }
                }
                else {
                    Ext.Msg.alert('Результат', 'Ошибка!');

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

    onApplyChangeSentToOSPOperation: function (btn) {
        var me = this,
            win = btn.up('changesenttoospwindow'),
            params = {};
        params.resolutionIds = Ext.JSON.encode(win.resolutionIds);
        params.sentToOSP = win.getParams().sentToOSP;
        debugger;
        me.mask('Изменение "Направлено приставам"...', win.getEl());
        win.down('form').submit({
            url: B4.Url.action('ChangeSentToOSP', 'Resolution', { 'b4_pseudo_xhr': true }),
            params: params,
            success: function (f, action) {
                var json = Ext.JSON.decode(action.response.responseText),
                    grid = me.getMainView();

                me.unmask();
                if (json.data.data) {
                    Ext.Msg.alert('Результат', 'Выполнено успешно');

                    win.close();
                    if (grid) {
                        grid.getStore().load();
                    }
                }
                else {
                    Ext.Msg.alert('Результат', 'Ошибка!');

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

    onLaunch: function (grid) {
        this.getStore('view.Resolution').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {

            if (this.params.filterParams.realityObjectId)
                operation.params.realityObjectId = this.params.filterParams.realityObjectId;

            if (this.params.filterParams.dateStart)
                operation.params.dateStart = this.params.filterParams.dateStart;

            if (this.params.filterParams.dateEnd)
                operation.params.dateEnd = this.params.filterParams.dateEnd;

            if (this.params.filterParams.dateNotPayStart)
                operation.params.dateNotPayStart = this.params.filterParams.dateNotPayStart;

            if (this.params.filterParams.dateNotPayEnd)
                operation.params.dateNotPayEnd = this.params.filterParams.dateNotPayEnd;
        }
    }
});