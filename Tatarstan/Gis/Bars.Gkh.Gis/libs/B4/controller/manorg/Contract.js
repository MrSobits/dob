Ext.define('B4.controller.manorg.Contract', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.manorg.Contract',
        'B4.aspects.permission.manorg.ContractTat',
        'B4.enums.TypeManagementManOrg',
        'B4.enums.TypeContractManOrgRealObj',
        'B4.aspects.fieldrequirement.ManorgTsjJskContract',
        'B4.aspects.fieldrequirement.ManorgOwnersContract'
    ],

    models: [
        'manorg.contract.Base',
        'manorg.contract.Transfer',
        'manorg.contract.Owners',
        'manorg.contract.JskTsj',
        'realityobj.DirectManagContract',

        'manorg.contract.ManOrgAddContractService',
        'manorg.contract.ManOrgAgrContractService',
        'manorg.contract.ManOrgComContractService'
    ],

    stores: [
        'manorg.contract.Base',
        'manorg.contract.Transfer',
        'manorg.ForSelect',
        'manorg.ForSelected',
        'realityobj.ByManOrg'
    ],

    views: [
        'manorg.contract.Grid',
        'manorg.contract.TransferEditWindow',
        'manorg.contract.OwnersEditWindow',
        'manorg.contract.JskTsjEditWindow',
        'manorg.contract.RelationEditWindow',
        'SelectWindow.MultiSelectWindow',
        'manorg.contract.DirectManagEditWindow',
        'manorg.contract.OwnersCommunalServiceGrid',
        'manorg.contract.OwnersAdditionServiceGrid',
        'manorg.contract.OwnersWorkServiceGrid',
        'manorg.contract.JskTsjCommunalServiceGrid',
        'manorg.contract.JskTsjAdditionServiceGrid',
        'manorg.contract.JskTsjWorkServiceGrid',
        'manorg.contract.TransferCommunalServiceGrid',
        'manorg.contract.TransferAdditionServiceGrid',
        'manorg.contract.TransferWorkServiceGrid',
        'manorg.contract.RelationCommunalServiceGrid',
        'manorg.contract.RelationAdditionServiceGrid',
        'manorg.contract.OwnersCommunalServiceEditWindow',
        'manorg.contract.OwnersAdditionServiceEditWindow',
        'manorg.contract.OwnersWorkServiceEditWindow',
        'manorg.contract.JskTsjCommunalServiceEditWindow',
        'manorg.contract.JskTsjAdditionServiceEditWindow',
        'manorg.contract.JskTsjWorkServiceEditWindow',
        'manorg.contract.TransferAdditionServiceEditWindow',
        'manorg.contract.TransferCommunalServiceEditWindow',
        'manorg.contract.TransferWorkServiceEditWindow',
        'manorg.contract.RelationAdditionServiceEditWindow',
        'manorg.contract.RelationCommunalServiceEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        { ref: 'jskTsjEditWindow', selector: 'jsktsjeditwindow' },
        { ref: 'jskTsjCommunalServiceGrid', selector: 'contractjsktsjcommunalservicegrid' },
        { ref: 'jskTsjAdditionServiceGrid', selector: 'contractjsktsjadditionservicegrid' },
        { ref: 'jskTsjWorkServiceGrid', selector: 'contractjsktsjworkservicegrid' },

        { ref: 'ownersEditWindow', selector: 'ownerseditwindow' },
        { ref: 'ownersAdditionServiceGrid', selector: 'contractownersadditionservicegrid' },
        { ref: 'ownersCommunalServiceGrid', selector: 'contractownerscommunalservicegrid' },
        { ref: 'ownersWorkServiceGrid', selector: 'contractownersworkservicegrid' },

        { ref: 'transferAdditionServiceGrid', selector: 'transferadditionservicegrid' },
        { ref: 'transferCommunalServiceGrid', selector: 'transfercommunalservicegrid' },
        { ref: 'transferWorkServiceGrid', selector: 'transferworkservicegrid' },

        { ref: 'relationAdditionServiceGrid', selector: 'relationadditionservicegrid' },
        { ref: 'relationCommunalServiceGrid', selector: 'relationcommunalservicegrid' }
    ],

    params: {},

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'manorgcontractperm'
        },
        {
            xtype: 'manorgcontracttatperm'
        },
        {
            xtype: 'manorgownerscontractfieldrequirement'
        },
        {
            xtype: 'manorgtsjjskcontractfieldrequirement'
        },

        //Передача управления
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgContractTransferWindowAspect',
            gridSelector: 'manorgContractGrid',
            modelName: 'manorg.contract.Transfer',
            modelType: B4.enums.TypeContractManOrgRealObj.ManagingOrgJskTsj,
            editFormSelector: '#manorgTransferEditWindow',
            editWindowView: 'manorg.contract.TransferEditWindow',
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' #sfRealityObject'] = {
                    'beforeload': {
                        fn: function (store, operation) {
                            operation.params = operation.params || {};
                            operation.params.manorgId = me.controller.getContextValue(me.controller.getMainView(), 'manorgId');
                        },
                        scope: me
                    }
                };
            },

            rowAction: function (grid, action, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowAction.apply(this, arguments);
                }
            },

            rowDblClick: function (view, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowDblClick.apply(this, arguments);
                }
            },

            gridAction: function (grid, action) {},

            listeners: {
                getdata: function (asp, record) {
                    var controller = asp.controller;

                    if (!record.getId()) {
                        record.set('ManagingOrganization', controller.getContextValue(controller.getMainView(), 'manorgId'));
                        record.set('TypeContractManOrgRealObj', this.modelType);
                    }
                },

                aftersetformdata: function (asp, record) {
                    asp.controller.setContract(record.getId());

                    asp.controller.mask('Загрузка', asp.controller.getMainView());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'ManOrgContractOwners', {
                        contractId: record.getId()
                    })).next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText);

                        var editWindow = Ext.ComponentQuery.query(asp.editFormSelector)[0];

                        var selectField = editWindow.down('#sfRealityObject');
                        selectField.setValue(obj);

                        asp.controller.unmask();
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                    editWindow.down('#docDateMonth')
                       .setValue({ 'ThisMonthPaymentDocDate': record.get('ThisMonthPaymentDocDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesBeginDateRb')
                       .setValue({ 'ThisMonthInputMeteringDeviceValuesBeginDate': record.get('ThisMonthInputMeteringDeviceValuesBeginDate') });


                    editWindow.down('#thisMonthInputMeteringDeviceValuesEndDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesEndDate': record.get('ThisMonthInputMeteringDeviceValuesEndDate') });

                    editWindow.down('#thisMonthPaymentServiceDateRb')
                        .setValue({ 'ThisMonthPaymentServiceDate': record.get('ThisMonthPaymentServiceDate') });
                }
            }
        },

        /*
        *аспект взаимодействия грида с 10 и 100 (УК Прочее)
        */
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgContractOwnersGridWindowAspect',
            gridSelector: 'manorgContractGrid',
            modelName: 'manorg.contract.Owners',
            modelType: B4.enums.TypeContractManOrgRealObj.ManagingOrgOwners,
            editFormSelector: '#manorgContractOwnersEditWindow',
            editWindowView: 'manorg.contract.OwnersEditWindow',
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' #sfRealityObject'] = {
                    'beforeload': {
                        fn: function (store, operation) {
                            operation.params = operation.params || {};
                            operation.params.manorgId = me.controller.getContextValue(me.controller.getMainView(), 'manorgId');
                        },
                        scope: me
                    }
                };
            },

            rowAction: function (grid, action, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowAction.apply(this, arguments);
                }
            },

            rowDblClick: function (view, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowDblClick.apply(this, arguments);
                }
            },

            gridAction: function (grid, action) {
                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    switch (action.toLowerCase()) {
                        case 'addmanorgowners':
                            this.editRecord();
                            break;
                        case 'update':
                            this.updateGrid();
                            break;
                    }
                }
            },

            listeners: {
                getdata: function(asp, record) {
                    var controller = asp.controller;
                    if (!record.getId()) {
                        record.set('ManagingOrganization', controller.getContextValue(controller.getMainView(), 'manorgId'));
                        record.set('TypeContractManOrgRealObj', this.modelType);
                    }
                },

                beforesaverequest: function () {
                    var me = this,
                        form = me.getForm();
                    var rec = me.getRecordBeforeSave(form.getRecord());
                  
                    if (!rec.VerificationDate && rec.getId() != 0) {
                        B4.Ajax.request(B4.Url.action('VerificationDate', 'ManOrgJskTsjContract', {
                            contractId: rec.getId(),
                            startDate: form.down('[name=StartDate]').getValue()
                        })).next(function () {
                            rec.VerificationDate = true;
                            me.saveRequestHandler();

                        }).error(function (response) {
                            Ext.Msg.prompt({
                                title: 'Предупреждение',
                                msg: response.message,
                                buttons: Ext.Msg.OKCANCEL,
                                fn: function (btnId) {
                                    if (btnId === "ok") {
                                        rec.VerificationDate = true;
                                        me.saveRequestHandler();
                                    }
                                }
                            });
                        }
                        );
                        return false;
                    } else {
                        rec.VerificationDate = false;
                        return true;
                    }
                },

                aftersetformdata: function(asp, record) {
                    asp.controller.setContract(record.getId());

                    asp.controller.mask('Загрузка', asp.controller.getMainView());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'ManOrgContractOwners', {
                        contractId: record.getId()
                    })).next(function(response) {
                        var obj = Ext.JSON.decode(response.responseText);

                        var editWindow = Ext.ComponentQuery.query(asp.editFormSelector)[0];

                        var selectField = editWindow.down('#sfRealityObject');
                        selectField.setValue(obj);

                        asp.controller.unmask();
                    }).error(function() {
                        asp.controller.unmask();
                    });

                    var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                    editWindow.down('#docDateMonth')
                       .setValue({ 'ThisMonthPaymentDocDate': record.get('ThisMonthPaymentDocDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesBeginDateRb')
                       .setValue({ 'ThisMonthInputMeteringDeviceValuesBeginDate': record.get('ThisMonthInputMeteringDeviceValuesBeginDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesEndDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesEndDate': record.get('ThisMonthInputMeteringDeviceValuesEndDate') });

                    editWindow.down('#thisMonthPaymentServiceDateRb')
                        .setValue({ 'ThisMonthPaymentServiceDate': record.get('ThisMonthPaymentServiceDate') });
                }
            }
        },

        /*
        *аспект взаимодействия грида c 20 и 40 ( ТСЖ  ЖСХ) 
        */
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgJskTsjContractGridWindowAspect',
            gridSelector: 'manorgContractGrid',
            modelName: 'manorg.contract.JskTsj',
            modelType: B4.enums.TypeContractManOrgRealObj.JskTsj,
            editFormSelector: '#jskTsjContractEditWindow',
            editWindowView: 'manorg.contract.JskTsjEditWindow',

            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #sfRealityObject'] = { 'beforeload': { fn: me.onBeforeLoadRealityObj, scope: me } };
            },

            /*устанавливаем id управляющей организации*/
            onBeforeLoadRealityObj: function (store, operation) {
                var me = this;
                operation.params = operation.params || {};
                operation.params.manorgId = me.controller.getContextValue(me.controller.getMainView(), 'manorgId');
            },

            gridAction: function (grid, action) {

                //тут мы по actionName кнопок подсовываем нужные параметры

                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    if (action.toLowerCase() == 'addjsktsj') {
                        this.editRecord();
                    }
                }
            },

            onSaveSuccess: function (asp, record) {
                this.setContract(record);
            },

            setContract: function (record) {
                var id = record.getId();
                this.controller.setContract(id);

                var store = this.controller.getStore('manorg.contract.Transfer');

                var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];
                var grid = editWindow.down('#manOrgContractRelationGrid');

                store.removeAll();
                grid.setDisabled(!id);

                if (id > 0) {
                    store.load();
                }
            },

            rowAction: function (grid, action, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowAction.apply(this, arguments);
                }
            },

            rowDblClick: function (view, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowDblClick.apply(this, arguments);
                }
            },

            listeners: {
                getdata: function (asp, record) {
                    var controller = asp.controller;

                    if (!record.getId()) {
                        record.set('ManagingOrganization', controller.getContextValue(controller.getMainView(), 'manorgId'));
                        //данный параметр modelType был проставлен в методе getModel
                        record.set('TypeContractManOrgRealObj', this.modelType);
                    }
                },

                beforesaverequest: function() {
                    var me = this,
                        form = me.getForm();
                    var rec = me.getRecordBeforeSave(form.getRecord());

                    if (!rec.VerificationDate && rec.getId() != 0) {
                        B4.Ajax.request(B4.Url.action('VerificationDate', 'ManOrgJskTsjContract', {
                            contractId: rec.getId(),
                            startDate: form.down('[name=StartDate]').getValue()
                        })).next(function() {
                            rec.VerificationDate = true;
                            me.saveRequestHandler();

                        }).error(function(response) {
                                Ext.Msg.prompt({
                                    title: 'Предупреждение',
                                    msg: response.message,
                                    buttons: Ext.Msg.OKCANCEL,
                                    fn: function(btnId) {
                                        if (btnId === "ok") {
                                            rec.VerificationDate = true;
                                            me.saveRequestHandler();
                                        }
                                    }
                                });
                            }
                        );
                        return false;
                    } else {
                        rec.VerificationDate = false;
                        return true;
                    }
                },

                aftersetformdata: function (asp, record) {
                    asp.setContract(record);

                    asp.controller.mask('Загрузка', asp.controller.getMainView());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'ManOrgJskTsjContract', {
                        contractId: record.getId()
                    })).next(function (response) {
                        
                        var obj = Ext.JSON.decode(response.responseText);

                        var editWindow = Ext.ComponentQuery.query(asp.editFormSelector)[0];
                        var selectField = editWindow.down('#sfRealityObject');
                        selectField.setValue(obj);

                        asp.controller.unmask();
                    }).error(function () {
                        asp.controller.unmask();
                    });
                    var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                    editWindow.down('#docDateMonth')
                         .setValue({ 'ThisMonthPaymentDocDate': record.get('ThisMonthPaymentDocDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesBeginDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesBeginDate': record.get('ThisMonthInputMeteringDeviceValuesBeginDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesEndDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesEndDate': record.get('ThisMonthInputMeteringDeviceValuesEndDate') });

                    editWindow.down('#thisMonthPaymentServiceDateRb')
                        .setValue({ 'ThisMonthPaymentServiceDate': record.get('ThisMonthPaymentServiceDate') });

                    editWindow.down('tabpanel tab[text=Сведения об услугах]').setDisabled(record.get('Id') == 0);
                    editWindow.down('tabpanel tab[text=Сведения о сроках]').setDisabled(record.get('Id') == 0);
                    editWindow.down('tabpanel tab[text=Передача управления]').setDisabled(record.get('Id') == 0);
                    editWindow.down('tabpanel tab[text=Сведения о плате]').setDisabled(record.get('Id') == 0);

                }
            }
        },

        {
            xtype: 'grideditwindowaspect',
            name: 'manOrgContractRelationWindowAspect',
            gridSelector: '#manOrgContractRelationGrid',
            editFormSelector: '#manOrgContractRelationEditWindow',
            storeName: 'manorg.contract.Transfer',
            modelName: 'manorg.contract.Transfer',
            editWindowView: 'manorg.contract.RelationEditWindow',
            setContract: function (record) {
                var id = record.getId();

                var relationAdditionServiceGrid = this.controller.getRelationAdditionServiceGrid(),
                    relationCommunalServiceGrid = this.controller.getRelationCommunalServiceGrid();

                this.controller.setContextValue(this.controller.getMainView(), 'tsjTskContractId', id);

                this.controller.setContractId(relationAdditionServiceGrid, id);
                this.controller.setContractId(relationCommunalServiceGrid, id);


            },
            listeners: {
                beforesave: function(asp, record) {
                    record.set('JskTsjContractId', asp.controller.getContextValue(asp.controller.getMainView(), 'contractId'));
                    record.set('ManOrgJskTsj', asp.controller.getContextValue(asp.controller.getMainView(), 'manorgId'));

                    return record;
                },

                aftersetformdata: function (asp, record) {
                    asp.setContract(record);


                    var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                    editWindow.down('#docDateMonth')
                         .setValue({ 'ThisMonthPaymentDocDate': record.get('ThisMonthPaymentDocDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesBeginDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesBeginDate': record.get('ThisMonthInputMeteringDeviceValuesBeginDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesEndDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesEndDate': record.get('ThisMonthInputMeteringDeviceValuesEndDate') });

                    editWindow.down('#thisMonthPaymentServiceDateRb')
                        .setValue({ 'ThisMonthPaymentServiceDate': record.get('ThisMonthPaymentServiceDate') });
                }
            },
            otherActions: function(actions) {
                var me = this;
                actions[me.editFormSelector + ' #sfManorg'] = { 'beforeload': { fn: me.onBeforeLoadManOrg, scope: me } };
            },

            onBeforeLoadManOrg: function(store, operation) {
                operation.params = operation.params || {};
                operation.params.manorgOnly = true;
                operation.params.showNotValid = false;
            }
        },

        {
            xtype: 'grideditctxwindowaspect',
            name: 'jskTsjCommunalServiceGridWindowAspect',
            gridSelector: 'contractjsktsjcommunalservicegrid',
            editFormSelector: 'jsktsjcommunalserviceeditwindow',
            modelName: 'manorg.contract.ManOrgComContractService',
            editWindowView: 'manorg.contract.JskTsjCommunalServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'contractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'jskTsjAdditionServiceGridWindowAspect',
            gridSelector: 'contractjsktsjadditionservicegrid',
            editFormSelector: 'jsktsjadditionserviceeditwindow',
            modelName: 'manorg.contract.ManOrgAddContractService',
            editWindowView: 'manorg.contract.JskTsjAdditionServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'contractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'jskTsjWorkServiceGridWindowAspect',
            gridSelector: 'contractjsktsjworkservicegrid',
            editFormSelector: 'jsktsjworkserviceeditwindow',
            modelName: 'manorg.contract.ManOrgAgrContractService',
            editWindowView: 'manorg.contract.JskTsjWorkServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'contractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            },
            otherActions: function (actions) {
                actions[this.editFormSelector + ' b4selectfield[name=Service]'] = {
                    'change': {
                        fn: function () {
                            this.controller.onChangeWorkService.apply(this, [this.editFormSelector].concat([].slice.call(arguments)));
                        },
                        scope: this
                    }
                };
            }
        },

        {
            xtype: 'grideditctxwindowaspect',
            name: 'ownersAdditionServiceGridWindowAspect',
            gridSelector: 'contractownersadditionservicegrid',
            editFormSelector: 'ownersadditionserviceeditwindow',
            modelName: 'manorg.contract.ManOrgAddContractService',
            editWindowView: 'manorg.contract.OwnersAdditionServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'contractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'ownersCommunalServiceGridWindowAspect',
            gridSelector: 'contractownerscommunalservicegrid',
            editFormSelector: 'ownerscommunalserviceeditwindow',
            modelName: 'manorg.contract.ManOrgComContractService',
            editWindowView: 'manorg.contract.OwnersCommunalServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'contractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'ownersWorkServiceGridWindowAspect',
            gridSelector: 'contractownersworkservicegrid',
            editFormSelector: 'ownersworkserviceeditwindow',
            modelName: 'manorg.contract.ManOrgAgrContractService',
            editWindowView: 'manorg.contract.OwnersWorkServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'contractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');
                    

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            },
            otherActions: function(actions) {
                actions[this.editFormSelector + ' b4selectfield[name=Service]'] = {
                     'change': {
                          fn: function() {
                               this.controller.onChangeWorkService.apply(this, [this.editFormSelector].concat([].slice.call(arguments)));
                          },
                          scope: this
                     }
                };
            }
        },


        {
            xtype: 'grideditctxwindowaspect',
            name: 'transferCommunalServiceGridWindowAspect',
            gridSelector: 'transfercommunalservicegrid',
            editFormSelector: 'transfercommunalserviceeditwindow',
            modelName: 'manorg.contract.ManOrgComContractService',
            editWindowView: 'manorg.contract.TransferCommunalServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'contractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'transferAdditionServiceGridWindowAspect',
            gridSelector: 'transferadditionservicegrid',
            editFormSelector: 'transferadditionserviceeditwindow',
            modelName: 'manorg.contract.ManOrgAddContractService',
            editWindowView: 'manorg.contract.TransferAdditionServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'contractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'transferWorkServiceGridWindowAspect',
            gridSelector: 'transferworkservicegrid',
            editFormSelector: 'transferworkserviceeditwindow',
            modelName: 'manorg.contract.ManOrgAgrContractService',
            editWindowView: 'manorg.contract.TransferWorkServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'contractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            },
            otherActions: function (actions) {
                actions[this.editFormSelector + ' b4selectfield[name=WorkService]'] = {
                    'change': {
                        fn: function () {
                            this.controller.onChangeWorkService.apply(this, [this.editFormSelector].concat([].slice.call(arguments)));
                        },
                        scope: this
                    }
                };
            }
        },

        {
            xtype: 'grideditctxwindowaspect',
            name: 'relationCommunalServiceGridWindowAspect',
            gridSelector: 'relationcommunalservicegrid',
            editFormSelector: 'relationcommunalserviceeditwindow',
            modelName: 'manorg.contract.ManOrgComContractService',
            editWindowView: 'manorg.contract.RelationCommunalServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'tsjTskContractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'relationAdditionServiceGridWindowAspect',
            gridSelector: 'relationadditionservicegrid',
            editFormSelector: 'relationadditionserviceeditwindow',
            modelName: 'manorg.contract.ManOrgAddContractService',
            editWindowView: 'manorg.contract.RelationAdditionServiceEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.get('Id')) {
                        record.set('Contract', me.controller.getContextValue(me.controller.getMainComponent(), 'tsjTskContractId'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this,
                        sf = form.down('b4selectfield[name=Service]'),
                        store = sf.getStore(),
                        view = me.controller.getMainView(),
                        manorgId = me.controller.getContextValue(view, 'manorgId');

                    if (manorgId) {
                        store.filters.clear();
                        store.filter('manorgId', manorgId);
                    }
                }
            }
        }
    ],

    mainView: 'manorg.contract.Grid',
    mainViewSelector: 'manorgContractGrid',

    jskTsjEditWindowSelector: '#jskTsjContractEditWindow',
    ownersEditWindowSelector: '#manorgContractOwnersEditWindow',
    transferEditWindowSelector: '#manorgTransferEditWindow',

    init: function () {
        this.callParent(arguments);

        this.getStore('manorg.contract.Transfer').on('beforeload', this.onBeforeLoadRelation, this);

        var actions = {};
        this.control(actions);
    },

    onChangeWorkService: function (windowSelector, cmp, newValue) {
        var view = cmp.up(windowSelector),
            fldType = view.down('#Type');
        
        if (newValue && newValue.TypeWork) {
            var type = B4.enums.TypeWork.displayRenderer(newValue.TypeWork);
            fldType.setValue(type);
        }
    },

    index: function (id) {
        var me = this,
        view = me.getMainView() || Ext.widget('manorgContractGrid'),
        store = view.getStore();

        B4.Ajax.request(B4.Url.action('Get', 'ManagingOrganization', {
            id: id
        })).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText);

            if (!obj) {
                Ext.Msg.alert('Внимание!', 'Не удалось определить управляющую организацию!');
                return;
            }
            me.setContextValue(view, 'TypeManagement', obj.data.TypeManagement);
            me.onMainViewAfterRender();

        }).error(function () {
        });

        if (view && view.headerFilterPlugin) {
            view.headerFilterPlugin.clearFilters();
        }

        store.filters.clear();

        store.filter([
            { property: "manorgId", value: id },
            { property: "fromManagOrg", value: true }]);

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);

        me.application.deployView(view, 'manorgId_info');

        me.getStore('manorg.contract.Base').load();
    },

    view: function (id) {
        this.index(id);
        this.application.getState().slice('managingorganizationedit/' + id);
    },

    onMainViewAfterRender: function () {
        var me = this,
            type = me.getContextValue(me.getMainView(), 'TypeManagement'),
            mainView = this.getMainView() || Ext.widget('manorgContractGrid');

        switch (type) {
            case 40:
            case 20:
                { // ТСЖ/ЖСК
                    mainView.down('#btnAddJskTsj').show();
                    mainView.down('#btnAddManOrgOwners').hide();
                    mainView.down('#gcIsTransManag').show();
                }
                break;
            case 10:
            case 100:
                { // УК
                    mainView.down('#btnAddJskTsj').hide();
                    mainView.down('#btnAddManOrgOwners').show();
                    mainView.down('#gcIsTransManag').hide();
                }
                break;
        }
    },

    onBeforeLoadRelation: function (store, operation) {
        if (this.params) {
            operation.params.contractId = this.getContractId();
            operation.params.fromManagOrg = true;
        }
    },

    setContract: function (id) {
        var jskTsjCommunalServiceGrid = this.getJskTsjCommunalServiceGrid(),
            jskTsjAdditionServiceGrid = this.getJskTsjAdditionServiceGrid(),
            jskTsjWorkServiceGrid = this.getJskTsjWorkServiceGrid(),
            ownersAdditionServiceGrid = this.getOwnersAdditionServiceGrid(),
            ownersCommunalServiceGrid = this.getOwnersCommunalServiceGrid(),
            ownersWorkServiceGrid = this.getOwnersWorkServiceGrid(),
            transferAdditionServiceGrid = this.getTransferAdditionServiceGrid(),
            transferCommunalServiceGrid = this.getTransferCommunalServiceGrid(),
            transferWorkServiceGrid = this.getTransferWorkServiceGrid();

        this.setContextValue(this.getMainView(), 'contractId', id);

        this.setContractId(jskTsjCommunalServiceGrid, id);
        this.setContractId(jskTsjAdditionServiceGrid, id);
        this.setContractId(jskTsjWorkServiceGrid, id);
        this.setContractId(ownersAdditionServiceGrid, id);
        this.setContractId(ownersCommunalServiceGrid, id);
        this.setContractId(ownersWorkServiceGrid, id);
        this.setContractId(transferAdditionServiceGrid, id);
        this.setContractId(transferCommunalServiceGrid, id);
        this.setContractId(transferWorkServiceGrid, id);
    },

    setContractId: function(grid, id) {
        if (grid) {
            grid.setDisabled(!id);

            if (id) {
                grid.getStore().filters.clear();
                grid.getStore().filter('contractId', id);
            }
        }
    },

    getContractId: function () {
        return this.getContextValue(this.getMainView(), 'contractId');
    }
});