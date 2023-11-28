Ext.define('B4.controller.Disposal', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.Disposal',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.DisposalTextValues',
        'B4.enums.TypeAgreementProsecutor',
        'B4.enums.TypeAgreementResult'
    ],

    models: [
        'Disposal',
        'ActCheck',
        'ActSurvey',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.TypeSurvey'
    ],

    stores: [
        'Disposal',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.TypeSurvey',
        'RealityObjectGjiForSelect',
        'RealityObjectGjiForSelected',
        'dict.TypeSurveyGjiForSelect',
        'dict.TypeSurveyGjiForSelected',
        'dict.Inspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ExpertGjiForSelect',
        'dict.ExpertGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected',
        'dict.Municipality'
    ],

    views: [
        'disposal.EditPanel',
        'disposal.AnnexEditWindow',
        'disposal.AnnexGrid',
        'disposal.ExpertGrid',
        'disposal.TypeSurveyGrid',
        'disposal.ProvidedDocGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'disposal.EditPanel',
    mainViewSelector: '#disposalEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#disposalAnnexGrid',
            controllerName: 'DisposalAnnex',
            name: 'disposalAnnexSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            Аспект формирвоания документов для Распоряжения
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'disposalCreateButtonAspect',
            buttonSelector: 'disposaleditpanel gjidocumentcreatebutton',
            containerSelector: 'disposaleditpanel',
            typeDocument: 10, // Тип документа Распоряжение
            onValidateUserParams: function (params) {
                debugger;
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил необходимы пользовательские параметры
                if (params.ruleId === 'DisposalToActCheckByRoRule' || params.ruleId === 'DisposalToActSurveyRule') {
                    return false;
                }
                return true;
            }
        },
        {
            xtype: 'disposalperm',
            editFormAspectName: 'disposalEditPanelAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'disposalPrintAspect',
            buttonSelector: '#disposalEditPanel #btnPrint',
            codeForm: 'Disposal',
            getUserParams: function (reportId) {
                debugger;
                var me = this,
                    param = { DocumentId: me.controller.params.documentId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'disposalStateButtonAspect',
            stateButtonSelector: '#disposalEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('disposalEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            /*
            Аспект основной панели Карточки распоряжения
            В нем вешаемся на событие aftersetpaneldata, чтобы загрузить подчиенные сторы
            А также проставить дополнительные значения
            Вешаемся на savesuccess чтобы после сохранения сразу получить Номер и обновить Вкладку
            */
            xtype: 'gjidocumentaspect',
            name: 'disposalEditPanelAspect',
            editPanelSelector: '#disposalEditPanel',
            modelName: 'Disposal',
            otherActions: function (actions) {
                debugger;
                var me = this;

                actions[me.editPanelSelector + ' #sfIssuredDisposal'] = { 'beforeload': { fn: me.onBeforeLoadInspectorManager, scope: me } };
                debugger;
            },
            onStateMenuItemClick: function () {
                debugger;
                var historyWin = this.getWindowHistory();

                if (this.entityId > 0 && this.currentState)
                    historyWin.updateGrid(this.entityId, this.currentState.TypeId);
                historyWin.show();
            },

           
            saveRecord: function (rec) {
                var me = this;
             
                Ext.Msg.confirm('Внимание!', 'Убедитесь, что вид проверки указан правильно', function (result) {
                    if (result === 'yes') {
                        if (me.fireEvent('beforesave', me, rec) !== false) {
                            if (me.hasUpload()) {
                                me.saveRecordHasUpload(rec);
                            } else {
                                me.saveRecordHasNotUpload(rec);
                            }
                        }
                    }
                });
            

            },
            onBeforeLoadInspectorManager: function (field, options, store) {
                options = options || {};
                options.params = options.params || {};

                options.params.headOnly = true;

                return true;
            },
            disableButtons: function (value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup'),
                    idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    callbackUnMask;
                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber'))
                    panel.setTitle(B4.DisposalTextValues.getSubjectiveCase() + " " + rec.get('DocumentNumber'));
                else
                    panel.setTitle(B4.DisposalTextValues.getSubjectiveCase());

                panel.down('#disposalTabPanel').setActiveTab(0);

                //получаем вид проверки
                if (asp.controller.params) {
                    asp.controller.params.kindCheckId = panel.down('#cbTypeCheck').getValue();
                }

                //Делаем запросы на получение Инспекторов
                //и обновляем соответсвующие Тригер филды
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('GetInfo', 'Disposal', { documentId: asp.controller.params.documentId }),
                    //для IE, чтобы не кэшировал GET запрос
                    cache: false
                }).next(function (response) {
                    asp.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText),
                        fieldInspectors = panel.down('#trigFInspectors');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    panel.down('#tfBaseName').setValue(obj.baseName);
                    panel.down('#tfPlanName').setValue(obj.planName);

                    asp.disableButtons(false);
                }).error(function () {
                    asp.controller.unmask();
                });

                // Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('disposalStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем отчеты
                me.controller.getAspect('disposalPrintAspect').loadReportStore();

                // обновляем кнопку Сформирвоать
                me.controller.getAspect('disposalCreateButtonAspect').setData(rec.get('Id'));
            },
            onSaveSuccess: function (asp, rec) {
                this.getPanel().setTitle(asp.controller.params.title + " " + rec.get('DocumentNumber'));

                if (asp.controller.params)
                    asp.controller.params.kindCheckId = asp.getPanel().down('#cbTypeCheck').getValue();
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки создания Акт на 1 дом и массовой формы выборка домов
            По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
            у главного аспекта вызывается метод создания документа createActCheck1House и передаются выбранные Id домов
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'disposalToActCheck1HouseGJIAspect',
            buttonSelector: '#disposalEditPanel [ruleId=DisposalToActCheckByRoRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalToActCheckByRoRuleSelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'RealityObjectGjiForSelect',
            storeSelected: 'RealityObjectGjiForSelected',
            selModelMode: 'SINGLE',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
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
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранный дом',

            onBeforeLoad: function (store, operation) {
                //спрятать панель выбранных домов
                var me = this,
                    wnd = Ext.ComponentQuery.query(me.multiSelectWindowSelector + ' #multiSelectedPanel')[0];

                if (wnd) {
                    wnd.hide();
                }

                if (me.controller.params && me.controller.params.inspectionId > 0)
                    operation.params.inspectionId = me.controller.params.inspectionId;
            },

            onRowSelect: function (rowModel, record, index, opt) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('disposalCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
                        params = creationAspect.getParams(btn);
                        params.realityIds = recordIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дом');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки создания Акт на обследование и массовой формы выборка домов
            По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
            у главного аспекта вызывается метод создания документа createActSurvey и передаются выбранные Id домов

            метод onRowSelect перекрываем потомучто необходимо изменить поведение выделения строки, чтобы можно было
            Выбрать только одну запись а не множество
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'disposalToActSurveyAspect',
            buttonSelector: '#disposalEditPanel [ruleId=DisposalToActSurveyRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalToActSurveySelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'RealityObjectGjiForSelect',
            storeSelected: 'RealityObjectGjiForSelected',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
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
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранный дом',

            onBeforeLoad: function (store, operation) {
                //получаем текущую запись документа Распоряжения
                var me = this,
                    record = me.controller.getAspect('disposalEditPanelAspect').getRecord();

                if (me.controller.params && me.controller.params.inspectionId > 0)
                    operation.params.inspectionId = me.controller.params.inspectionId;

                if (record.get('TypeDisposal') == 20) {
                    //Если акт обследования делается из Распоряжения на проверку Предписания то
                    //получаем все дома из предписаний по которым создано это распоряжение
                    operation.params.documentId = record.get('Id');
                }
            },

            onRowSelect: function (rowModel, record, index, opt) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
                //куда хотим добавить запись
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('disposalCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
                        params = creationAspect.getParams(btn);
                        params.realityIds = recordIds;

                        creationAspect.createDocument(params);

                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            аспект взаимодействия грида типов обследований с массовой формой выбора типов обслуживания
            по нажатию на кнопку добавления показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем Типы обслуживания через серверный метод /DisposalTypeSurvey/AddTypeSurveys
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalTypeSurveyAspect',
            modelName: 'disposal.TypeSurvey',
            storeName: 'disposal.TypeSurvey',
            gridSelector: '#disposalTypeSurveyGrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalTypeSurveySelectWindow',
            storeSelect: 'dict.TypeSurveyGjiForSelect',
            storeSelected: 'dict.TypeSurveyGjiForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор типов обследования',
            titleGridSelect: 'Типы обследования для отбора',
            titleGridSelected: 'Выбранные типы обследования',

            onBeforeLoad: function (store, operation) {
                var me = this;

                if (me.controller.params) {
                    operation.params.kindCheckId = me.controller.params.kindCheckId;
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddTypeSurveys', 'DisposalTypeSurvey', {
                        typeIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Типы обследований сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /Disposal/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'disposalInspectorMultiSelectWindowAspect',
            fieldSelector: '#disposalEditPanel #trigFInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы экспертов с массовой формой выбора экспертов
            По нажатию на Добавить открывается форма выбора экспертов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение экспертов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalExpertAspect',
            gridSelector: '#disposalExpertGrid',
            storeName: 'disposal.Expert',
            modelName: 'disposal.Expert',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalExpertMultiSelectWindow',
            storeSelect: 'dict.ExpertGjiForSelect',
            storeSelected: 'dict.ExpertGjiForSelected',
            titleSelectWindow: 'Выбор экспертов',
            titleGridSelect: 'Эксперты для отбора',
            titleGridSelected: 'Выбранные эксперты',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddExperts', 'DisposalExpert', {
                            expertIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать экспертов');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            По нажатию на Добавить открывается форма выбора предоставляемых документов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение предоставляемых документов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalProvidedDocumentAspect',
            gridSelector: '#disposalProvidedDocGrid',
            storeName: 'disposal.ProvidedDoc',
            modelName: 'disposal.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalProvidedDocMultiSelectWindow',
            storeSelect: 'dict.ProvidedDocGjiForSelect',
            storeSelected: 'dict.ProvidedDocGjiForSelected',
            titleSelectWindow: 'Выбор предоставляемых документов',
            titleGridSelect: 'Документы для выбора',
            titleGridSelected: 'Выбранные документы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 0.5, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.disposalId = this.controller.params.documentId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddProvidedDocuments', 'DisposalProvidedDoc', {
                            providedDocIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать документы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы нарушения с массовой формой выбора нарушений
            Тут есть 2 варианта
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalViolationAspect',
            gridSelector: '#disposalViolationGrid',
            storeName: 'disposal.Violation',
            modelName: 'disposal.Violation',
            multiSelectWindowSelector: '#disposalViolationMultiSelectWindow',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            otherActions: function (actions) {
                var me = this;
                actions['#disposalViolationGrid #updateButton'] = {
                    click: {
                        fn: function () {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };

                actions['msviolationsbasedisposal b4selectfield[name=RealityObject]'] = {
                    beforeload: {
                        fn: function (asp, options, store) {
                            // поулчаем дома по основанию проверки
                            options.params.inspectionId = me.controller.params.inspectionId;
                        }
                    }
                };
            },
            onBeforeLoad: function (store, operation) {
                if (this.controller.params.disposalType == 20)
                    operation.params.documentIds = this.controller.params.parentIds;
            },
            listeners: {
                beforegridaction: function (asp, grid, action) {
                    if (action.toLowerCase() == 'add') {
                        /*
                            Если приказ на проверку предписания, то выбираем из нарушений предписания
                            Если приказ не на проверку предписания то выбираем из списка всех нарушений
                        */
                        if (asp.controller.params.disposalType == 20) {
                            asp.multiSelectWindow = 'SelectWindow.MultiSelectWindow';
                            asp.storeSelect = 'prescription.ViolationForSelect';
                            asp.storeSelected = 'prescription.ViolationForSelected';
                            asp.columnsGridSelect = [
                                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
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
                                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
                            ];

                            asp.columnsGridSelected = [
                                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, sortable: false },
                                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, sortable: false },
                                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
                            ];
                        } else {
                            asp.multiSelectWindow = 'disposal.MultiSelectViolationsForBaseDisposal';
                            asp.storeSelect = 'dict.ViolationGjiForSelect';
                            asp.storeSelected = 'dict.ViolationGjiForSelected';
                            asp.columnsGridSelect = [
                                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ];

                            asp.columnsGridSelected = [
                                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ];
                        }
                    }
                },

                getdata: function (asp, records) {
                    var recordIds = [],
                        insViolIds = [],
                        roField = asp.getForm().down('b4selectfield[name=RealityObject]'),
                        roId;

                    if (roField != null) {

                        roId = roField.getValue();

                        if (!roId) {
                            Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дом');
                            return false;
                        } else {
                            asp.controller.params.realObjId = roId;
                        }
                    }

                    records.each(function (rec) {
                        if (rec.get('InspectionViolationId') > 0) {
                            insViolIds.push(rec.get('InspectionViolationId'));
                        } else {
                            recordIds.push(rec.getId());
                        }
                    });

                    if (recordIds[0] > 0 || insViolIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddViolations', 'DisposalViol'),
                            method: 'POST',
                            params: {
                                violIds: Ext.encode(recordIds),
                                insViolIds: Ext.encode(insViolIds),
                                documentId: asp.controller.params.documentId,
                                realObjId: roId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.getStore('disposal.RealityObjViolation').load();
                            return true;
                        }).error(function (e) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка!', e.message || e);
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
             * Аспект взаимодействия Таблицы Приложений с формой редактирования
             */
            xtype: 'grideditwindowaspect',
            name: 'disposalAnnexAspect',
            gridSelector: '#disposalAnnexGrid',
            editFormSelector: '#disposalAnnexEditWindow',
            storeName: 'disposal.Annex',
            modelName: 'disposal.Annex',
            editWindowView: 'disposal.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Disposal', this.controller.params.documentId);
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('disposal.Expert').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.ProvidedDoc').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.TypeSurvey').on('beforeload', me.onBeforeLoad, me);

        me.control({
            '#disposalEditPanel  #cbTypeAgreementProsecutor, #cbTypeAgreementResult': { 'change': me.onChangeTypeAgreement }
        });

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('disposalEditPanelAspect').setData(me.params.documentId);

            //Обновляем таблицу Экспертов
            me.getStore('disposal.Expert').load();

            //Обновляем таблицу Предоставляемых документов
            me.getStore('disposal.ProvidedDoc').load();

            //Обновляем таблицу Приложения
            me.getStore('disposal.Annex').load();

            //обновляем таблицу типов обследования
            me.getStore('disposal.TypeSurvey').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params && me.params.documentId)
            operation.params.documentId = me.params.documentId;
    },

    onChangeTypeAgreement: function () {
        var me = this,
            view = me.getMainView(),
            docDate = view.down('#cbDocumentDateWithResultAgreement'),
            docNum = view.down('#cbDocumentNumberWithResultAgreement'),
            typeAgreementProsecutor = view.down('#cbTypeAgreementProsecutor').getValue(),
            typeAgreementResult = view.down('#cbTypeAgreementResult').getValue(),
            isEnabledocdate = ((typeAgreementResult === B4.enums.TypeAgreementResult.Agreed || typeAgreementResult === B4.enums.TypeAgreementResult.NotAgreed) && typeAgreementProsecutor === B4.enums.TypeAgreementProsecutor.RequiresAgreement) && docDate.allowedEdit,
            isEnabledocnum = ((typeAgreementResult === B4.enums.TypeAgreementResult.Agreed || typeAgreementResult === B4.enums.TypeAgreementResult.NotAgreed) && typeAgreementProsecutor === B4.enums.TypeAgreementProsecutor.RequiresAgreement) && docNum.allowedEdit;

        docDate.setDisabled(!isEnabledocdate);
        docNum.setDisabled(!isEnabledocnum);
    }
});