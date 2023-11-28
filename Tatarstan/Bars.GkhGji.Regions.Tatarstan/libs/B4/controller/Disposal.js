Ext.define('B4.controller.Disposal', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.Disposal',
        'B4.aspects.permission.TatDisposal',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.FieldRequirementAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.DisposalTextValues',
        'B4.aspects.GkhBlobText',
        'B4.form.SelectWindow',
        'B4.QuickMsg',
        'B4.enums.TypeCheck'
    ],

    models: [
        'Disposal',
        'ActCheck',
        'ActSurvey',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.DisposalVerificationSubject',
        'disposal.TypeSurvey',
        'disposal.SurveyPurpose',
        'disposal.InspFoundationCheck',
        'disposal.SurveyObjective',
    ],

    stores: [
        'RealityObjectGjiForSelect',
        'RealityObjectGjiForSelected',
        'Disposal',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.TypeSurvey',
        'disposal.DisposalVerificationSubject',
        'disposal.SurveyPurpose',
        'disposal.InspFoundationCheck',
        'disposal.SurveyObjective',
        'dict.SurveyPurposeForSelect',
        'dict.SurveyPurposeForSelected',
        'dict.SurveyObjectiveForSelect',
        'dict.SurveyObjectiveForSelected',
        'dict.NormativeDocForSelect',
        'dict.NormativeDocForSelected',
        'dict.SurveySubjectForSelect',
        'dict.SurveySubjectForSelected',
        'dict.TypeSurveyGjiForSelect',
        'dict.TypeSurveyGjiForSelected',
        'dict.Inspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ExpertGjiForSelect',
        'dict.ExpertGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected',
        'dict.Municipality',
    ],

    views: [
        'disposal.EditPanel',
        'disposal.InspFoundationCheckGrid',
        'disposal.SubjectVerificationGrid',
        'disposal.SurveyPurposeGrid',
        'disposal.SurveyObjectiveGrid',

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
            /*
            Аспект формирвоания документов для Распоряжения
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'disposalCreateButtonAspect',
            buttonSelector: 'disposaleditpanel gjidocumentcreatebutton',
            containerSelector: 'disposaleditpanel',
            typeDocument: 10, // Тип документа Распоряжение
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил необходимы пользовательские параметры
                if (params.ruleId == 'DisposalToActCheckByRoRule' || params.ruleId == 'DisposalToActSurveyRule') {
                    return false;
                }
            }
        },
        {
            xtype: 'disposalperm',
            editFormAspectName: 'disposalEditPanelAspect'
        },
        {
            xtype: 'tatdisposalperm',
            editFormAspectName: 'disposalEditPanelAspect'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitStart', applyTo: '[name=TimeVisitStart]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitEnd', applyTo: '[name=TimeVisitEnd]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcDate', applyTo: '[name=NcDate]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcNum', applyTo: '[name=NcNum]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcDateLatter', applyTo: '[name=NcDateLatter]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcNumLatter', applyTo: '[name=NcNumLatter]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcSent', applyTo: '[name=NcSent]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcObtained', applyTo: '[name=NcObtained]', selector: '#disposalEditPanel' }
            ]
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'disposalPrintAspect',
            buttonSelector: '#disposalEditPanel #btnPrint',
            codeForm: 'Disposal',
            getUserParams: function(reportId) {

                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
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
                transfersuccess: function(asp, entityId) {
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
            otherActions: function(actions) {
                actions[this.editPanelSelector + ' #sfIssuredDisposal'] = { 'beforeload': { fn: this.onBeforeLoadInspectorManager, scope: this } };
            },
            saveRecord: function(rec) {
                var me = this;

                Ext.Msg.confirm('Внимание!', 'Убедитесь, что вид проверки указан правильно', function(result) {
                    if (result == 'yes') {
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
            onBeforeLoadInspectorManager: function(field, options, store) {
                options = options || {};
                options.params = options.params || {};

                options.params.headOnly = true;

                return true;
            },
            disableButtons: function(value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
                var idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function(asp, rec, panel) {
                var me = this;

                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
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
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldInspectors = panel.down('#trigFInspectors');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    panel.down('#tfBaseName').setValue(obj.baseName);
                    panel.down('#tfPlanName').setValue(obj.planName);

                    asp.disableButtons(false);
                }).error(function() {
                    asp.controller.unmask();
                });

                me.controller.getStore('disposal.Expert').load();
                me.controller.getStore('disposal.ProvidedDoc').load();
                me.controller.getStore('disposal.Annex').load();
                me.controller.getStore('disposal.TypeSurvey').load();
                me.controller.getStore('disposal.DisposalVerificationSubject').load();

                me.controller.getStore('disposal.SurveyPurpose').load();
                me.controller.getStore('disposal.SurveyObjective').load();
                me.controller.getStore('disposal.InspFoundationCheck').load();

                me.controller.getAspect('disposalNoticeDescriptionAspect').doInjection();

                // Значение по умолчанию "Внеплановая документарная" для поля "Вид проверки", если не задано
                var cbTypeCheck = panel.down('#cbTypeCheck');
                if (cbTypeCheck.getValue() == null) {
                    panel.down('#cbTypeCheck').setValue(B4.enums.TypeCheck.NotPlannedDocumentation);
                }

                // Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('disposalStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем отчеты
                me.controller.getAspect('disposalPrintAspect').loadReportStore();

                // обновляем кнопку Сформирвоать
                me.controller.getAspect('disposalCreateButtonAspect').setData(rec.get('Id'));
            },
            onSaveSuccess: function(asp, rec) {
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
                var wnd = Ext.ComponentQuery.query(this.multiSelectWindowSelector + ' #multiSelectedPanel')[0];
                if (wnd) {
                    wnd.hide();
                }

                if (this.controller.params && this.controller.params.inspectionId > 0)
                    operation.params.inspectionId = this.controller.params.inspectionId;
            },

            onRowSelect: function(rowModel, record, index, opt) {
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
                getdata: function(asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    records.each(function(rec, index) { recordIds.push(rec.get('RealityObjectId')); });

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

            onBeforeLoad: function(store, operation) {
                //получаем текущую запись документа Распоряжения
                var record = this.controller.getAspect('disposalEditPanelAspect').getRecord();

                if (this.controller.params && this.controller.params.inspectionId > 0)
                    operation.params.inspectionId = this.controller.params.inspectionId;

                if (record.get('TypeDisposal') == 20) {
                    //Если акт обследования делается из Распоряжения на проверку Предписания то
                    //получаем все дома из предписаний по которым создано это распоряжение
                    operation.params.documentId = record.get('Id');
                }
            },

            onRowSelect: function(rowModel, record, index, opt) {
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
                getdata: function(asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    records.each(function(rec, index) { recordIds.push(rec.get('RealityObjectId')); });

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

            onBeforeLoad: function(store, operation) {
                if (this.controller.params) {
                    operation.params.kindCheckId = this.controller.params.kindCheckId;
                }
            },

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddTypeSurveys', 'DisposalTypeSurvey', {
                        typeIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function(response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Типы обследований сохранены успешно');
                        return true;
                    }).error(function() {
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
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec, index) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function(response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function() {
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
                getdata: function(asp, records) {

                    var recordIds = [];

                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddExperts', 'DisposalExpert', {
                            expertIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function(response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
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
            isPaginable: false,
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    renderer: function(value, metaData) {
                        metaData.style = "white-space: normal;";
                        return value;
                    }
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    sortable: false,
                    renderer: function(value, metaData) {
                        metaData.style = "white-space: normal;";
                        return value;
                    }
                }
            ],
            onBeforeLoad: function (store, operation) {
                operation.start = undefined;
                operation.page = undefined;
                operation.limit = undefined;
                operation.params.disposalId = this.controller.params.documentId;
            },
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddProvidedDocuments', 'DisposalProvidedDoc', {
                            providedDocIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function(response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
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
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Disposal', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'disposalNoticeDescriptionAspect',
            fieldSelector: '[name=NoticeDescription]',
            editPanelAspectName: 'disposalEditPanelAspect',
            getAction: 'GetNoticeDescription',
            saveAction: 'SaveNoticeDescription',
            controllerName: 'Disposal',
            valueFieldName: 'NoticeDescription',
            previewLength: 200,
            autoSavePreview: true,
            previewField: 'NoticeDescription'
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalSurveySubjectAspect',
            modelName: 'disposal.DisposalVerificationSubject',
            storeName: 'disposal.DisposalVerificationSubject',
            gridSelector: 'disposalsubjectverificationgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalSurveySubjectSelectWindow',
            storeSelect: 'dict.SurveySubjectForSelect',
            storeSelected: 'dict.SurveySubjectForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Предметы проверки',
            titleGridSelect: 'Предметы проверки для отбора',
            titleGridSelected: 'Выбранные предметы проверки',
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation = operation || {};
                operation.params = operation.params || {};
                if (me.controller.params) {
                    var existingIds = me.getGrid().getStore().collect('SurveySubjectId', false, true);
                    operation.params.ids = Ext.encode(existingIds);
                    operation.params.forSelect = true;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddSurveySubjects', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();

                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.getStore('disposal.TypeSurvey').load();
                        asp.controller.getStore('disposal.SurveyPurpose').load();
                        asp.controller.getStore('disposal.SurveyObjective').load();
                        asp.controller.getStore('disposal.InspFoundationCheck').load();
                        asp.controller.getStore('disposal.ProvidedDoc').load();

                        Ext.Msg.alert('Сохранение!', 'Предметы проверки сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalSurveyPurposeAspect',
            modelName: 'disposal.SurveyPurpose',
            storeName: 'disposal.SurveyPurpose',
            gridSelector: 'disposalsurveypurposegrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalSurveyPurposeSelectWindow',
            storeSelect: 'dict.SurveyPurposeForSelect',
            storeSelected: 'dict.SurveyPurposeForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор целей проверки',
            titleGridSelect: 'Цели проверки для отбора',
            titleGridSelected: 'Выбранные цели проверки',

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddSurveyPurposes', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Цели проверки сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalSurveyObjectiveAspect',
            modelName: 'disposal.SurveyObjective',
            storeName: 'disposal.SurveyObjective',
            gridSelector: 'disposalsurveyobjectivegrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalSurveyObjectiveSelectWindow',
            storeSelect: 'dict.SurveyObjectiveForSelect',
            storeSelected: 'dict.SurveyObjectiveForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор задач проверки',
            titleGridSelect: 'Задачи проверки для отбора',
            titleGridSelected: 'Выбранные задачи проверки',

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddSurveyObjectives', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Задачи проверки сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalInspFoundationCheckAspect',
            modelName: 'disposal.InspFoundationCheck',
            storeName: 'disposal.InspFoundationCheck',
            gridSelector: 'disposalinspfoundationcheckgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalInspFoundationCheckSelectWindow',
            storeSelect: 'dict.NormativeDocForSelect',
            storeSelected: 'dict.NormativeDocForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нормативных документов',
            titleGridSelect: 'Нормативные документы для отбора',
            titleGridSelected: 'Выбранные нормативные документы',

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspFoundationChecks', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Нормативные документы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
    ],

    init: function () {
        var me = this,
            actions = {};

        me.getStore('disposal.Expert').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.ProvidedDoc').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.TypeSurvey').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.DisposalVerificationSubject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.SurveyObjective').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.SurveyPurpose').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.InspFoundationCheck').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);

        me.control(actions);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('disposalEditPanelAspect').setData(me.params.documentId);
        }
    },

    onBeforeLoad: function(store, operation) {
        if (this.params && this.params.documentId)
            operation.params.documentId = this.params.documentId;
    },
});