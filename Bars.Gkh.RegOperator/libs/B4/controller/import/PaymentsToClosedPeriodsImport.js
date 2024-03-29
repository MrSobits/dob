﻿/**
 * Контроллер импорта оплаты в закрытый период
 */
Ext.define('B4.controller.import.PaymentsToClosedPeriodsImport', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhImportAspect',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    views: ['import.PaymentsToClosedPeriodsPanel'],    

    mainView: 'import.PaymentsToClosedPeriodsPanel',
    mainViewSelector: 'paymentstoclosedperiodspanel',    

    aspects: [
        // Импорт
        {
            xtype: 'gkhimportaspect',
            name: 'import',
            viewSelector: 'form[name=paymentsToClosedPeriodsImportForm]', // Не на всю панель, а только на форму выбора файла. Иначе импорт будет запускаться на нажатие любой кнопки, например "обновить грид".
            importId: 'Bars.Gkh.RegOperator.Imports.PaymentsToClosedPeriodsImport',
            maxFileSize: 52428800,
            getUserParams: function () {
                var me = this;
                me.params = me.params || {};                
                me.params.periodId = me.controller.getMainView().down('b4selectfield[name=Period]').getValue();
                me.params.updateSaldoIn = !me.controller.getMainView().down('checkbox[name=UpdateSaldoIn]').getValue(); // Инверсия
                var fCashPaymentCenter = me.controller.getMainView().down('b4selectfield[name=CashPaymentCenter]');
                me.params.externalRkcId = fCashPaymentCenter.getStore().getById(fCashPaymentCenter.getValue()).get('Identifier');
            }           
        },
        // Ролевой доступ на переход в журнал предупреждений
        {
            xtype: 'gkhpermissionaspect',
            name: 'detailsPerm',            
            permissions: [
                {
                    name: 'Import.PaymentsToClosedPeriods.Warnings.View',
                    applyTo: '[name=ViewDetails]',
                    selector: 'paymentstoclosedperiodspanel',
                    applyBy: function (component, allowed) {
                        component.isAllowed = allowed;
                    }
                }
            ]
        }
    ],

    /** 
     * Инициализировать
     */
    init: function() {
        var me = this;
        me.control({
            'paymentstoclosedperiodspanel': {
                // Подписаться на событие просчёта отображения
                'render': { fn: me.onMainViewRender, scope: me },
                // Добавить обработчик на действия со строкой грида
                rowaction: me.onRowAction,
                // Подписаться на событие установки или снятия галочки на записи
                'select': { fn: me.onRowSelect, scope: me },
                'deselect': { fn: me.onRowSelect, scope: me },
            },
            // Подписаться на событие изменения значения в выпадающем списке Период
            'paymentstoclosedperiodspanel b4selectfield[name=Period]': {
                'change': {
                    fn: me.onSelectorChange,
                    scope: me
                },             
            },
            // Подписаться на событие изменения значения в выпадающем списке РКЦ
            'paymentstoclosedperiodspanel b4selectfield[name=CashPaymentCenter]': {
                'change': {
                    fn: me.onSelectorChange,
                    scope: me
                },             
            },
            // Привязять обработчик к повторному импорту
            'paymentstoclosedperiodspanel [action=ReImport]': {
                click: { fn: me.reImport, scope: me } },
        });
        me.callParent(arguments);
    },

    /**
     * Запустить
     */
    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);        
        me.bindContext(view);
        me.application.deployView(view);        
    },
    
    /**
     * Обработать событие просчёта отображения
     */
    onMainViewRender: function (grid) {
        var me = this;
        // Повесить обработчики на событие "перед началом загрузки хранилища".
        // В этом событии будут цепляться дополнительные параметры к запросу в хранилище.
        grid.down('#runningTasksGrid').getStore().on('beforeLoad', me.onStoreBeforeLoad, me);
        grid.getStore().on('beforeLoad', me.onStoreBeforeLoad, me);
    },

    /**
     * Обработать событие начала загрузки хранилища
     */
    onStoreBeforeLoad: function (store, operation) {
        var me = this;
        // Фильтр по importId. Взять его из аспекта.
        operation.params.importId = me.getAspect('import').importId;
    },

    /**
     * Обработать событие изменения значений в выпадающих списках Период и РКЦ.
     * Здесь реализуется контроль того, что нельзя запусить импорт пока не введены необходимые параметры.
     */
    onSelectorChange: function(cmp, newValue) {
        var me = this,
            mainView = me.getMainView(),
            importForm = mainView.down('#importForm'),
            fileField = importForm.down('b4filefield'), // Поле выбора файла
            button = importForm.down('button[action=import]'), // Кнопка "Загрузить"
            perid = mainView.down('b4selectfield[name=Period]'), // Выбор период
            cashPaymentCenter = mainView.down('b4selectfield[name=CashPaymentCenter]'); // Выбор РКЦ

        // Обнулено значение в выпадающем списке (период или РКЦ). Например, нажатием на крестик.
        if (!newValue) {
            // Заблокировать
            fileField.setValue(null);
            fileField.setDisabled(true);
            button.setDisabled(true);
        }
        // Выбрано значение в выпадающем списке (период или РКЦ)
        else {
            // И период и РКЦ теперь введены
            if (perid.getValue() != null && cashPaymentCenter.getValue() != null) {
                // Разблокировать
                fileField.setDisabled(false);
                button.setDisabled(false);
            }
        }
    },   

    /**
     * Обработать действия со строкой грида
     */
    onRowAction: function (grid, action, rec) {
        var me = this;
        // Перейти в журнал предупреждений
        if (action.toLowerCase() === 'gotoresult') {            
            me.application.redirectTo(Ext.String.format('warninginpaymentstoclosedperiodsimport/{0}', rec.data.Id));
        }
    },

    /**
     * Запустить выбранный импорт повторно
     */
    reImport: function() {
        var me = this,
            grid = me.getMainView(),
            selectedRecord = grid.getSelectionModel().getSelection(); // Выбранная запись из журнала импорта
        
        // Здесь не делается проверка, что есть выбранная запись, т.к. реализована блокировка кнопки

        me.mask('Обработка...');        
        B4.Ajax.request({
            url: B4.Url.action('ReImport', 'ClosedPeriodsImport'),
            params: {
                Id: selectedRecord[0].internalId 
            },
            timeout: 2 * 60 * 1000 // 2 мин. (с запасом на загрузку сервера, т.к. обычно импорты делаются под закрытие отчётного периода)
        })
        .next(function (response) {
            var resp = Ext.isEmpty(response.responseText) ? response : Ext.JSON.decode(response.responseText);
            me.unmask();
            Ext.Msg.show({
                title: 'Успех',
                msg: resp.message,
                width: 300,
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.INFO
            });                        
        })
        .error(function (response) {            
            var resp = Ext.isEmpty(response.responseText) ? response : Ext.JSON.decode(response.responseText);
            me.unmask();
            Ext.Msg.show({
                title: 'Ошибка',
                msg: resp.message,
                width: 300,
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.ERROR
            });            
        });
    },

    /**
     * Обработать событие установки или снятия галочки на записи.
     * Здесь обыгрывется блокировка кнопки "повторить импорт".
     * Реализована возможность запускать повторные импорты только по одному.
     */
    onRowSelect: function (rowModel, record, index, eOpts) {
        var me = this,
            grid = me.getMainView(),
            selectedRecords = grid.getSelectionModel().getSelection();

        grid.down('#reImportButton').setDisabled(true); // Сначала заблокировать
        // Выбрана ровно одна запись
        if (selectedRecords.length === 1) {
            grid.down('#reImportButton').setDisabled(false); // Разблокировать
        }
    }
});