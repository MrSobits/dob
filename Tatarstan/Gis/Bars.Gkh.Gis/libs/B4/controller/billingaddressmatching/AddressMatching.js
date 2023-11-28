Ext.define('B4.controller.billingaddressMatching.AddressMatching', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.aspects.GridEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: ['billingaddressmatching.ImportedAddress', 'billingaddressmatching.FiasAddress'],
    views: [
        'billingaddressmatching.ImportedGrid',
        'billingaddressmatching.FiasGrid'
    ],

    mainView: 'billingaddressmatching.AddressMatchingPanel',
    mainViewSelector: 'importedgrid',

    refs: [
        {
            ref: 'mainPanel',
            selector: 'importaddressmatchingpnl'
        }
    ],

    aspects: [
        {
            /*
            * Аспект вызова сопоставления
            */
            xtype: 'grideditwindowaspect',
            name: 'billingAddressMatchGridWindowAspect',
            gridSelector: 'importedgrid',
            storeName: 'billingaddressmatching.ImportedAddress',
            modelName: 'billingaddressmatching.ImportedAddress',
            rowAction: function(grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                    case 'match':
                        this.controller.matchAddress(record);
                        break;
                    }
                }
            }
        }
    ],

    index: function() {
        var view = this.getMainPanel()
            || Ext.widget('importaddressmatchingpnl'),
            billingInfoPanel = view.down('breadcrumbs[name = "billingAddrInfo"]'),
            fiasInfoPanel = view.down('breadcrumbs[name = "fiasAddrInfo"]');

        this.bindContext(view);
        this.application.deployView(view);

        if (billingInfoPanel) {
            billingInfoPanel.update({ text: 'Адрес домов импорта' });
        }
        if (fiasInfoPanel) {
            fiasInfoPanel.update({ text: 'Адрес домов ФИАС' });
        }
    },

    init: function() {
        var me = this,
            actions = {
                'importedgrid, importedfiasgrid': {
                    render: { fn: me.onRenderGrid, scope: me }
                },
                'importedgrid b4updatebutton': {
                    click: { fn: me.updateGrid, scope: me }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    onRenderGrid: function(grid) {
        var store = grid.getStore();
        store.load();
    },
    
    updateGrid: function (btn) {
        var store = btn.up('importaddressmatchingpnl').down('importedgrid').getStore();
        store.load();
    },

    //вызов функции сопоставления адресов
    matchAddress: function(record) {
        var me = this,
            fiasGrid = Ext.ComponentQuery.query('importedfiasgrid')[0],
            fiasSelectRecord = fiasGrid.getSelectionModel().getSelection()[0],
            id = record ? record.getId() : null;

        if (!fiasSelectRecord) {
            B4.QuickMsg.msg('Внимание', 'Выберите ФИАС запись дома', 'warning');
            return;
        }
        Ext.MessageBox.show({
            title: 'Сопоставление адреса',
            modal: true,
            msg: 'Вы уверены что хотите сопоставить выбранные адреса?',
            buttonText: { yes: "Да", no: "Нет" },
            fn: function(btn) {
                switch (btn) {
                case 'yes':
                    me.matchSelectedAddresses(id, fiasSelectRecord.getId());
                    break;
                }
            }
        });
    },

    //сопоставить выбранные адреса
    matchSelectedAddresses: function(billingId, fiasAddressId) {
        var me = this,
            gisGridStore = Ext.ComponentQuery.query('importedgrid')[0].getStore();

        me.mask('Сохранение', me.getMainComponent());

        B4.Ajax.request(
            {
                method: 'POST',
                url: B4.Url.action('ManualBillingAddressMatch', 'GisAddressMatching'),
                params: {
                    addrMatchId: billingId,
                    fiasId: fiasAddressId
                },
                timeout: 999999
            }).next(function (response) {
                var responseObj = Ext.decode(response.responseText);
                me.unmask();
                B4.QuickMsg.msg(
                    responseObj.success ? 'Успешно' : 'Внимание!', 
                    responseObj.success ? responseObj.message : 'Не удалось сопоставить выбранные адреса',
                    responseObj.success ? 'success' : 'warning');
                gisGridStore.load();
            }).error(function (response) {
                me.unmask();
                B4.QuickMsg.msg('Ошибка!', 'Не удалось сохранить новый адрес', 'error');
            });
    }
});