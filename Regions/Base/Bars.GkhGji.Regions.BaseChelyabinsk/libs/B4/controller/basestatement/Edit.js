﻿Ext.define('B4.controller.basestatement.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.permission.BaseStatement',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'Disposal',
        'BaseStatement',
        'RealityObjectGji'
    ],

    stores: [
        'basestatement.RealityObject',
        'appealcits.ForSelect',
        'appealcits.ForSelected',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'basestatement.RealityObjectGrid',
        'basestatement.EditPanel',
        'RoomNumsEditWindow'
    ],

    mainView: 'basestatement.EditPanel',
    mainViewSelector: '#baseStatementEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    baseStatementEditPanelSelector: '#baseStatementEditPanel',

    aspects: [
        {
            /*
            Аспект формирвоания документов для данного основания по обращению
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'baseStatementCreateButtonAspect',
            buttonSelector: '#baseStatementEditPanel gjidocumentcreatebutton',
            containerSelector: '#baseStatementEditPanel',
            typeBase: 20 // Тип проверка обращения
        },
        {
            xtype: 'basestatementperm',
            editFormAspectName: 'baseStatementEditPanelAspect'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                {
                    name: 'GkhGji.Inspection.BaseStatement.Register.RealityObject.Column.RoomNums', applyTo: '[dataIndex=RoomNums]', selector: '#baseStatementRealityObjectGrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                },
                {
                    name: 'GkhGji.Inspection.BaseStatement.Register.RealityObject.ShowAll_View', applyTo: '[name=ShowAll]', selector: '#baseStatementRealityObjectMultiSelectWindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                }
            ],
            editFormAspectName: 'baseStatementEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'baseStatementStateButtonAspect',
            stateButtonSelector: '#baseStatementEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('baseStatementEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
            Аспект основной панели Проверки по обращению граждан
            */
            xtype: 'gjiinspectionaspect',
            name: 'baseStatementEditPanelAspect',
            editPanelSelector: '#baseStatementEditPanel',
            modelName: 'BaseStatement',
            otherActions: function (actions) {
                //В данном методе добавляем дополнительные обработчики для получения Должностей у SelectField Инспекторов
                actions[this.editPanelSelector + ' #cbPersonInspection'] = { 'change': { fn: this.onPersonInspectionChange, scope: this } };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' #trigfAppealCitizens'] = { 'triggerClear': { fn: this.onTriggerClearAppealCitizens, scope: this } };

                actions[this.editPanelSelector + ' #btnDelete'] = { 'click': { fn: this.btnDeleteClick, scope: this } };
                actions[this.editPanelSelector + ' #cbPersonInspection'] = { 'change': { fn: this.onChangePerson, scope: this } };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.onChangeType, scope: this } };
            },
            onChangeType: function (field, newValue, oldValue) {
                this.controller.params = this.controller.params || {};
                
                if (oldValue != null && newValue != oldValue) {
                    this.getPanel().down('#sfContragent').setValue(null);
                }
                
                this.controller.params.typeJurOrg = newValue;
            },
            onChangePerson: function (field, newValue, oldValue) {
                var panel = this.getPanel(),
                    sfContragent = panel.down('#sfContragent'),
                    tfPhysicalPerson = panel.down('#tfPhysicalPerson'),
                    cbTypeJurPerson = panel.down('#cbTypeJurPerson');

                switch (newValue) {
                    case 10://физлицо
                        sfContragent.hide();
                        tfPhysicalPerson.show();
                        cbTypeJurPerson.hide();
                        break;
                    case 20://организация
                        sfContragent.show();
                        tfPhysicalPerson.hide();
                        cbTypeJurPerson.show();
                        break;
                    case 30://должностное лицо
                        sfContragent.show();
                        tfPhysicalPerson.show();
                        cbTypeJurPerson.show();
                        break;
                }
            },

            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = this.controller.params.typeJurOrg;
            },

            onSaveSuccess: function (asp, record) {
                asp.controller.setInspectionId(record.get('Id'));
            },

            onTriggerClearAppealCitizens: function () {
                this.controller.mask('Сохранение', this.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'BaseStatement', {
                    objectIds: "",
                    inspectionId: this.controller.params.inspectionId
                })).next(function (response) {
                    this.controller.unmask();
                    Ext.Msg.alert('Сохранение!', 'Обращения граждан сохранены успешно');

                    this.controller.getStore('basestatement.RealityObject').load();
                    return true;
                }, this).error(function () {
                    this.controller.unmask();
                }, this);
            },

            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {

                    asp.controller.params = asp.controller.params || {};

                    // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                    // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                    var callbackUnMask = asp.controller.params.callbackUnMask;
                    if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                        callbackUnMask.call();
                    }
                    
                    var statementId = rec.get('Id');
                    
                    asp.controller.setInspectionId(statementId);

                    //Делаем запросы на получение Обращения граждан и обновляем соответсвующий Тригер филд
                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'BaseStatement', {
                        inspectionId: statementId
                    })).next(function (response) {
                        asp.controller.unmask();
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);

                        var fieldAppealCitizens = panel.down('#trigfAppealCitizens');
                        fieldAppealCitizens.updateDisplayedText(obj.appealCitizensNames);
                        fieldAppealCitizens.setValue(obj.appealCitizensIds);
                    }).error(function () {
                        asp.controller.unmask();
                    });
                    
                    //Обновляем статусы
                    this.controller.getAspect('baseStatementStateButtonAspect').setStateData(statementId, rec.get('State'));
                    //Обновляем кнопку Сформировать
                    this.controller.getAspect('baseStatementCreateButtonAspect').setData(statementId);
                }
            },
            btnDeleteClick: function () {
                var panel = this.getPanel();
                var record = panel.getForm().getRecord();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ?', function (result) {
                    if (result == 'yes') {
                        this.mask('Удаление', B4.getBody());
                        record.destroy()
                            .next(function (result) {
                                //Обновляем дерево меню
                                this.unmask();
                                var tree = Ext.ComponentQuery.query(this.controller.params.treeMenuSelector)[0];
                                tree.getStore().load();
                            }, this)
                            .error(function (result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                this.unmask();
                            }, this);
                    }
                }, this);
            }
            
        },
        {
            /*
            аспект взаимодействия триггер-поля Обращения граждан с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /StatementGJI/AddAppealCitizens
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'appealCitizensMultiSelectWindowAspect',
            fieldSelector: '#baseStatementEditPanel #trigfAppealCitizens',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseStatementSelectWindow',
            storeSelect: 'appealcits.ForSelect',
            storeSelected: 'appealcits.ForSelected',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата обращения', xtype: 'datecolumn', format: 'd.m.Y', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Номер комиссии', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Управляющая организация', xtype: 'gridcolumn', dataIndex: 'ManagingOrganization', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Количество вопросов', xtype: 'gridcolumn', dataIndex: 'QuestionsCount', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, sortable: false },
                { header: 'Номер комиссии', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор обращения граждан',
            titleGridSelect: 'Обращения граждан для выбора',
            titleGridSelected: 'Выбранные обращения граждан',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'BaseStatement', {
                            objectIds: recordIds,
                            inspectionId: asp.controller.params.inspectionId
                        })).next(function (response) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Обращения граждан сохранены успешно');

                            asp.controller.getStore('basestatement.RealityObject').load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать обращения граждан');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'baseStatementRealityObjectAspect',
            gridSelector: '#baseStatementRealityObjectGrid',
            storeName: 'basestatement.RealityObject',
            modelName: 'RealityObjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseStatementRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            toolbarItems: [
                {
                    xtype: 'checkbox',
                    margin: '0 5 0 0',
                    name: 'ShowAll',
                    fieldLabel: 'Показать все дома',
                    labelWidth: 120,
                    labelAlign: 'right'
                }
            ],
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                var asp = this,
                    panel = asp.controller.getMainView(),
                    showAll = asp.getForm().down('[name=ShowAll]').getValue();
                if (!showAll) {
                    operation.params.typeJurPerson = panel.down('#cbTypeJurPerson').getValue();
                    operation.params.contragentId = panel.down('#sfContragent').getValue();
                }
                
                operation.params.isPhysicalPerson = panel.down('#cbPersonInspection').getValue() === 10 ? true : false;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddRealityObjects', 'InspectionGjiRealityObject'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                inspectionId: asp.controller.params.inspectionId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            },
            otherActions: function (actions) {
                actions[this.multiSelectWindowSelector + ' [name=ShowAll]'] = {
                    'change': {
                        fn: function () {
                            this.getSelectGrid().getStore().load();
                        }, scope: this
                    }
                };
            }
        },
        {
            /*
            * обработка кнопки "Добавить квартиры" через отдельное окно
            */
            xtype: 'grideditwindowaspect',
            name: 'roomNumsEditWindowAspect',
            gridSelector: 'baseStatementRealityObjGrid',
            editFormSelector: 'roomnumseditwindow',
            storeName: 'basestatement.RealityObject',
            modelName: 'RealityObjectGji',
            editWindowView: 'RoomNumsEditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.gridSelector + ' button[actionName=addRooms]'] = { 'click': { fn: me.addRoomNumsButtonHandler, scope: me } };
            },

            addRoomNumsButtonHandler: function () {
                var me = this,
                selectedArray = me.controller.getMainView().down('baseStatementRealityObjGrid').selModel.getSelection();
                if (selectedArray.length > 0) {
                    me.editRecord(selectedArray[0]);
                } else {
                    Ext.Msg.alert('Ошибка!', 'Необходимо выбрать проверяемый дом');
                }
            },
            listeners: {
                beforegridaction: function (asp, grid, action) {
                    //один грид юзают два аспекта, как результат две открытых формы вместо одной
                    if (action == 'add') {
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        this.getStore('basestatement.RealityObject').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('baseStatementEditPanelAspect').setData(this.params.inspectionId);

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.params.title);
        }
        this.getStore('basestatement.RealityObject').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.inspectionId > 0)
            operation.params.inspectionId = this.params.inspectionId;
    },

    setInspectionId: function (id) {
        if (this.params) {
            this.params.inspectionId = id;
        }
    }
});