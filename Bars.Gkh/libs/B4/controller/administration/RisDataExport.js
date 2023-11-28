Ext.define('B4.controller.administration.RisDataExport', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.administration.risdataexport.Panel',
        'B4.view.administration.risdataexport.AddWindow',
        'B4.store.administration.risdataexport.FormatDataExportTask',
        'B4.store.administration.risdataexport.FormatDataExportSection',
        'B4.enums.FormatDataExportStatus'

    ],

    models: [
        'administration.risdataexport.FormatDataExportTask',
        'administration.risdataexport.FormatDataExportSection'
    ],
    stores: [
        'administration.risdataexport.FormatDataExportTask',
        'administration.risdataexport.FormatDataExportSection'
    ],

    views: [
        'administration.risdataexport.Panel'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'exportTaskGridWindowAspect',
            gridSelector: 'risdataexporttaskgrid',
            editFormSelector: 'risdataexportaddwindow',
            editWindowView: 'administration.risdataexport.AddWindow',
            storeName: 'administration.risdataexport.FormatDataExportTask',
            modelName: 'administration.risdataexport.FormatDataExportTask',
            listeners: {
                aftersetformdata: function (asp, record) {
                    var form = asp.getForm(),
                        saveButton = form.down('b4savebutton'),
                        rawRecord = record.raw,
                        entityGroupCodeList = record.get('EntityGroupCodeList') || [],
                        entityGroupCodeListField = asp.getForm().down('[name=EntityGroupCodeList]'),
                        id = record.get('Id') || 0;

                    if (id === 0) {
                        saveButton.setText('Создать задачу');
                    } else {
                        if (entityGroupCodeList.length === 0) {
                            entityGroupCodeListField.updateDisplayedText('Выбраны все');
                        }
                        asp.customSetValues(rawRecord, 'PeriodType');
                        asp.customSetValues(rawRecord, 'StartDayOfWeekList');
                        asp.customSetValues(rawRecord, 'StartMonthList');
                        asp.customSetValues(rawRecord, 'StartDaysList');

                        saveButton.setIconCls('icon-arrow-refresh')
                        saveButton.setText('Перезапустить задачу');
                        record.setDirty()
                    }
                },
                getdata: function (asp, record) {
                    var periodType = record.get('PeriodType'),
                        startDayOfWeekList = record.get('StartDayOfWeekList'),
                        startMonthList = record.get('StartMonthList'),
                        startDaysList = record.get('StartDaysList'),
                        startDayOfWeekListValue = [],
                        startMonthListValue = [],
                        startDaysListValue = [],
                        entityGroupCodeListValue = record.get('EntityGroupCodeList') || [];

                    switch (periodType) {
                        case 2:
                            startDayOfWeekListValue = (function(value) {
                                var codes = [];
                                if (value) {
                                    Ext.each(value,
                                        function(v, i) {
                                            if (v) {
                                                codes.push(i + 1);
                                            }
                                        });
                                }
                                return codes;
                            })(startDayOfWeekList);
                            break;
                        case 3:
                            startMonthListValue = (function(value) {
                                var codes = [];
                                if (value) {
                                    Ext.each(value,
                                        function(v, i) {
                                            if (v) {
                                                codes.push(i + 1);
                                            }
                                        });
                                }
                                return codes;
                            })(startMonthList);
                            startDaysListValue = (function(value) {
                                var codes = [];
                                if (value) {
                                    if (typeof value === 'boolean') {
                                        codes.push(0);
                                    } else {
                                        Ext.each(value,
                                            function(v, i) {
                                                if (v) {
                                                    codes.push(i + 1);
                                                }
                                            });
                                    }
                                }
                                return codes;
                            })(startDaysList);
                            break;
                    }

                    record.set('StartDayOfWeekList', startDayOfWeekListValue);
                    record.set('StartMonthList', startMonthListValue);
                    record.set('StartDaysList', startDaysListValue);
                    record.set('EntityGroupCodeList', entityGroupCodeListValue);
                }
            },
            otherActions: function (actions) {
                var asp = this;

                if (asp.editFormSelector) {
                    actions[asp.editFormSelector + ' b4savebutton'] = {
                        'click': {
                            fn: asp.customSaveRequestHandler,
                            scope: asp
                        }
                    };
                }
            },
            customSaveRequestHandler: function () {
                var asp = this,
                    selectField = asp.getForm().down('[name=EntityGroupCodeList]'),
                    selectFieldText = selectField.getText(),
                    selectedValues = selectField.getValue() || [];

                if (selectFieldText === '' && selectedValues.length === 0) {
                    Ext.Msg.confirm('Создание задачи',
                        'Экспорт будет произведен по всем секциям. Вы действительно хотите запустить задачу?',
                        function(result) {
                            if (result == 'yes') {
                                asp.saveRequestHandler();
                            }
                        },
                        asp);
                } else {
                    asp.saveRequestHandler();
                }
            },
            customSetValues: function(record, paramName) {
                var asp = this,
                    form = asp.getForm(),
                    value = {};

                value[paramName] = record[paramName];
                form.down('[name=' + paramName + ']').setValue(value);
            },
            deleteRecord: function (record) {
                var asp = this,
                    me = asp.controller;

                Ext.Msg.confirm('Удаление задачи', 'Выполняемая задача будет прервана. Вы действительно хотите удалить задачу?', function (result) {
                    if (result == 'yes') {
                        var model = this.getModel(record);

                        var rec = new model({ Id: record.getId() });
                        asp.mask('Удаление', B4.getBody());
                        rec.destroy()
                            .next(function () {
                                asp.fireEvent('deletesuccess', asp);
                                me.updateTaskGrid();
                                asp.unmask();
                            }, asp)
                            .error(function (result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                asp.unmask();
                            }, asp);
                    }
                }, asp);
            },
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'administration.risdataexport.Panel',
    mainViewSelector: 'risdataexportpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'risdataexportpanel'
        },
        {
            ref: 'exportResultGrid',
            selector: 'risdataexportresultgrid'
        },
        {
            ref: 'exportTaskGrid',
            selector: 'risdataexporttaskgrid'
        },
        {
            ref: 'exportTaskWindow',
            selector: 'risdataexportaddwindow'
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'risdataexportresultgrid': { 'rowaction': { fn: me.onResultGridRowAction, scope: me } },
                'b4updatebutton': { 'click': { fn: me.onUpdateButtonClick, scope: me } },
                'risdataexportaddwindow radiogroup[name=PeriodType]': { 'change': { fn: me.onPeriodicityChange, scope: me } },
                'risdataexportaddwindow checkbox[name=StartNow]': { 'change': { fn: me.onStartNowChange, scope: me } },
                'risdataexportaddwindow checkboxgroup[name=StartDaysList] [inputValue=0]': { 'change': { fn: me.onLastDayChange, scope: me } },
                'risdataexportaddwindow': {
                    'show': { fn: me.onAddWindowShow, scope: me },
                    'close': { fn: me.updateTaskGrid, scope: me }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('risdataexportpanel');

        me.bindContext(view);
        me.application.deployView(view);

        me.getExportTaskGrid().getStore().load();
        me.getExportResultGrid().getStore().load();
    },

    onResultGridRowAction: function(grid, action, rec) {
        var logFile = rec.get('LogFile') || {},
            status = rec.get('Status'),
            msgTitle = 'Получение файла лога';

        switch (action) {
            case 'getlog':
                switch (status) {
                    case B4.enums.FormatDataExportStatus.Pending:
                    case B4.enums.FormatDataExportStatus.Running:
                        B4.QuickMsg.msg(msgTitle,
                            'Задача не завершена. Лог не доступен', 'warning');
                        return;
                }

                if (logFile.Id > 0) {
                    window.open(B4.Url.action('Download', 'FileUpload', { id: logFile.Id }));
                } else {
                    B4.QuickMsg.msg(msgTitle, 'При сохранении лога произошла ошибка. Файл недоступен', 'error');
                }
                return;
        }
    },

    onUpdateButtonClick: function(button) {
        button.up('b4grid').getStore().load();
    },

    onAddButtonClick: function (button) {
        var window = Ext.widget('risdataexportaddwindow');

        window.show();
    },

    onPeriodicityChange: function (component, newValue, oldValue) {
        var parentContainer = component.up('container'),
            dateIntervalField = parentContainer.down('container[name=DateInterval]'),
            timeIntervalField = parentContainer.down('container[name=TimeInterval]'),
            dayOfWeekField = parentContainer.down('checkboxgroup[name=StartDayOfWeekList]'),
            monthField = parentContainer.down('checkboxgroup[name=StartMonthList]'),
            dayField = parentContainer.down('checkboxgroup[name=StartDaysList]'),
            startNowCheckBox = parentContainer.down('checkbox[name=StartNow]');

        Ext.each(dateIntervalField.query(), function (component) { component.disable() });
        Ext.each(timeIntervalField.query(), function (component) { component.disable() });
        dayOfWeekField.disable();
        dayOfWeekField.unsetActiveError();
        monthField.disable();
        monthField.unsetActiveError();
        dayField.disable();
        dayField.unsetActiveError();
        startNowCheckBox.setRawValue(false);
        startNowCheckBox.disable();

        switch (newValue.PeriodType) {
            case 0:
                Ext.each(timeIntervalField.query(), function (component) { component.enable() });
                startNowCheckBox.enable();
                break;

            case 1:
                Ext.each(dateIntervalField.query(), function (component) { component.enable() });
                Ext.each(timeIntervalField.query(), function (component) { component.enable() });
                break;

            case 2:
                Ext.each(dateIntervalField.query(), function (component) { component.enable() });
                Ext.each(timeIntervalField.query(), function (component) { component.enable() });
                dayOfWeekField.enable();
                break;

            case 3:
                Ext.each(dateIntervalField.query(), function (component) { component.enable() });
                Ext.each(timeIntervalField.query(), function (component) { component.enable() });
                monthField.enable();
                dayField.enable();
                break;
        }
    },

    onStartNowChange: function(component, newValue) {
        var parentContainer = component.up('container'),
            timeIntervalField = parentContainer.down('container[name=TimeInterval]');

        Ext.each(timeIntervalField.query(), function (component) { component.setDisabled(newValue) });
    },

    onAddWindowShow: function(component) {
        var radioGroup = component.down('radiogroup[name=PeriodType]');

        radioGroup.fireEvent('change', radioGroup, radioGroup.getValue());
    },

    updateTaskGrid: function() {
        var grid = this.getMainView().down('risdataexporttaskgrid');
        grid.getStore().load();
    },

    onLastDayChange: function(component, newValue) {
        var checkboxGroup = component.up('checkboxgroup');

        Ext.each(checkboxGroup.query(), function(checkBox) {
            if (checkBox != component) {
                checkBox.setDisabled(newValue);
            }
        });
    }
});