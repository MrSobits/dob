Ext.define('B4.view.disposal.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.disposaleditpanel',
    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'disposalEditPanel',
    title: '',
    autoScroll: true,

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.Inspector',
        'B4.store.PoliticAuthority',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.Control.GkhTriggerField',
        'B4.view.disposal.TypeSurveyGrid',
        'B4.view.disposal.AnnexGrid',
        'B4.view.disposal.ExpertGrid',
        'B4.view.disposal.ProvidedDocGrid',
        'B4.view.disposal.SubjectVerificationGrid',
        'B4.view.disposal.DocConfirmGrid',
        'B4.view.disposal.DisposalAdditionalDocGrid',
        'B4.view.disposal.ViolationGrid',
        'B4.enums.TypeAgreementResult',
        'B4.enums.TypeAgreementProsecutor',
        'B4.enums.TypeViolator',
        'B4.enums.YesNo',
        'B4.enums.KindKNDGJI',
        'B4.enums.FamilyStatus',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.disposal.SurveyPurposeGrid',
        'B4.view.disposal.SurveyObjectiveGrid',
        'B4.view.disposal.InspFoundationGrid',
        'B4.view.disposal.InspFoundationCheckPanel',
        'B4.view.disposal.AdminRegulationGrid',
        'B4.view.disposal.DisposalControlMeasuresGrid',
        'B4.store.dict.Municipality'
    ],

    initComponent: function() {
        var me = this;

        me.title = B4.DisposalTextValues.getSubjectiveCase();
        //В воронеже смешанного вида контроля нет. А вроде в одной стране по одним законам живем.
        var currKindKNDGJI = B4.enums.KindKNDGJI.getItems();
        var newKindKNDGJI = [];
        Ext.iterate(currKindKNDGJI, function (val, key) {
            if (key != 3)
                newKindKNDGJI.push(val);
        });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    overflowY: 'hidden',
                    overflowX: 'hidden',
                    id: 'disposalTopPanel',
                    border: false,
                    frame: true,
                    defaults: {
                        border: false,
                        labelWidth: 170,
                        xtype: 'panel',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 50,
                                    width: 200,
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: true
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 140,
                                    width: 295,
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300
                                }
                            ]
                        },
                        {
                            padding: '0 15px 10px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            defaults: {
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    width: 200,
                                    name: 'DocumentYear',
                                    itemId: 'nfDocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 50,
                                    hideTrigger: true
                                },
                                {
                                    width: 295,
                                    name: 'DocumentNum',
                                    itemId: 'nfDocumentNum',
                                    fieldLabel: 'Номер',
                                    labelWidth: 140,
                                    hideTrigger: true
                                },
                                {
                                    name: 'LiteralNum',
                                    itemId: 'nfLiteralNum',
                                    fieldLabel: 'Буквенный подномер',
                                    labelAlign: 'right',
                                    xtype: 'textfield',
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    name: 'DocumentSubNum',
                                    itemId: 'nfDocumentSubNum',
                                    fieldLabel: 'Подномер',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'disposalTabPanel',
                    border: false,
                    flex: 1,
                    autoScroll: true,
                    listeners: {
                        render: function(p){
                            p.body.on('scroll', function (e) {
                                var elementDisposalTopPanel = Ext.getCmp('disposalTopPanel').body.dom;
                                elementDisposalTopPanel.scrollLeft = e.target.scrollLeft;
                                elementDisposalTopPanel.scrollTop = e.target.scrollTop;
                            }, p);
                        }
                    },
                    items: [
                        {
                            xtype: 'form',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Реквизиты',
                            name: 'Requisite',
                            border: false,
                            bodyPadding: 5,
                            frame: true,
                            autoScroll: true,
                            minWidth: 900,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            flex: .7,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right',
                                                readOnly: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'disposalBaseName',
                                                    itemId: 'tfBaseName',
                                                    fieldLabel: 'Основание обследования'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'disposalPlanName',
                                                    itemId: 'tfPlanName',
                                                    fieldLabel: 'Документ основания'
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'KindCheck',
                                                    fieldLabel: 'Вид проверки',
                                                    displayField: 'Name',
                                                    url: '/KindCheckGji/List',
                                                    valueField: 'Id',
                                                    itemId: 'cbTypeCheck',
                                                    readOnly: false,
                                                    editable: false
                                                }
                                                //{
                                                //    xtype: 'b4combobox',
                                                //    name: 'KindKNDGJI',
                                                //    fieldLabel: 'Вид контроля(надзора)',
                                                //    displayField: 'Display',
                                                //    itemId: 'cbKindKNDGJI',
                                                //    // store: B4.enums.KindKNDGJI.getStore(),
                                                //    items: newKindKNDGJI,
                                                //    valueField: 'Value',
                                                //    allowBlank: false,
                                                //    readOnly: false,
                                                //    editable: false
                                                //}
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            //padding: '0 0 5 0',
                                            flex: 1,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        allowBlank: false,
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            name: 'DateStart',
                                                            itemId: 'dfDateStart',
                                                            fieldLabel: 'Период проведения проверки с',
                                                            labelWidth: 200,
                                                            flex: 0.6
                                                        },
                                                        {
                                                            name: 'DateEnd',
                                                            itemId: 'dfDateEnd',
                                                            fieldLabel: 'по',
                                                            labelWidth: 60,
                                                            flex: 0.4
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            name: 'ObjectVisitStart',
                                                            itemId: 'dfObjectVisitStart',
                                                            fieldLabel: 'Выезд на объект с',
                                                            labelWidth: 200,
                                                            flex: 0.6
                                                        },
                                                        {
                                                            name: 'ObjectVisitEnd',
                                                            itemId: 'dfObjectVisitEnd',
                                                            fieldLabel: 'по',
                                                            labelWidth: 60,
                                                            flex: 0.4
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '5 0 0 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            fieldLabel: 'Время с',
                                                            name: 'TimeVisitStart',
                                                            xtype: 'timefield',
                                                            format: 'H:i',
                                                            labelWidth: 200,
                                                            submitFormat: 'Y-m-d H:i:s',
                                                            minValue: '8:00',
                                                            maxValue: '22:00',
                                                            flex: 0.6
                                                        },
                                                        {
                                                            fieldLabel: 'по',
                                                            name: 'TimeVisitEnd',
                                                            xtype: 'timefield',
                                                            format: 'H:i',
                                                            labelWidth: 60,
                                                            submitFormat: 'Y-m-d H:i:s',
                                                            minValue: '8:00',
                                                            maxValue: '22:00',
                                                            flex: 0.4
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    padding: '5 0 0 0',
                                                    name: 'PeriodCorrect',
                                                    fieldLabel: 'Срок проверки',
                                                    labelAlign: 'right',
                                                    labelWidth: 200,
                                                    maxLength: 500
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'Согласование с прокуратурой',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                editable: false,
                                                displayField: 'Display',
                                                valueField: 'Value',
                                                readOnly: false,
                                                labelWidth: 180,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    name: 'TypeAgreementProsecutor',
                                                    itemId: 'cbTypeAgreementProsecutor',
                                                    fieldLabel: 'Согласование с прокуратурой',
                                                    store: B4.enums.TypeAgreementProsecutor.getStore(),
                                                    flex: 0.7
                                                },
                                                {
                                                    name: 'TypeAgreementResult',
                                                    itemId: 'cbTypeAgreementResult',
                                                    fieldLabel: 'Результат согласования',
                                                    store: B4.enums.TypeAgreementResult.getStore(),
                                                    flex: 1,
                                                    labelWidth: 170
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    store: 'B4.store.PoliticAuthority',
                                                    textProperty: 'ContragentName',
                                                    name: 'PoliticAuthority',
                                                    fieldLabel: "Орган прокуратуры",
                                                    columns: [
                                                        { header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                                                        { header: 'Контрагент', xtype: 'gridcolumn', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } },
                                                        { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                                    ],
                                                    dockedItems: [
                                                        {
                                                            xtype: 'b4pagingtoolbar',
                                                            displayInfo: true,
                                                            store: 'B4.store.PoliticAuthority',
                                                            dock: 'bottom'
                                                        }
                                                    ],
                                                    flex: 0.7,
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'container',
                                                    flex: 1,
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            labelWidth: 250,
                                                            labelAlign: 'right',
                                                            name: 'DateStatement',
                                                            width: 350,
                                                            fieldLabel: 'Дата и время формирования заявления'
                                                        },
                                                        {
                                                            name: 'TimeStatement',
                                                            xtype: 'timefield',
                                                            format: 'H:i',
                                                            submitFormat: 'Y-m-d H:i:s',
                                                            padding: '0 0 0 5',
                                                            width: 60,
                                                            minValue: '8:00',
                                                            maxValue: '22:00'
                                                        },
                                                        {
                                                            xtype: 'component',
                                                            flex: 1
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            itemId: 'approveContainer',
                                            hidden: true,
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    labelAlign: 'right',
                                                    name: 'ProcAprooveDate',
                                                    flex: 1,
                                                    fieldLabel: 'Дата согласования'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    padding: '5 0 0 0',
                                                    name: 'ProcAprooveNum',
                                                    fieldLabel: 'Номер документа о согласовании',
                                                    labelAlign: 'right',
                                                    flex: 1,
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'b4filefield',
                                                    name: 'ProcAprooveFile',
                                                    flex: 1,
                                                    fieldLabel: 'Файл документа о согласовании',
                                                    editable: false
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            itemId: 'approveresContainer',
                                            hidden: true,
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right'
                                            },
                                            items: [                                            
                                                {
                                                    xtype: 'textfield',
                                                    padding: '5 0 0 0',
                                                    name: 'PositionProcAproove',
                                                    fieldLabel: 'Должность согласовавшего',
                                                    labelAlign: 'right',
                                                    flex: 1,
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    padding: '5 0 0 0',
                                                    name: 'FioProcAproove',
                                                    fieldLabel: 'ФИО согласовавшего',
                                                    labelAlign: 'right',
                                                    flex: 1,
                                                    maxLength: 250
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    labelWidth: 180,
                                                    name: 'ProsecutorDecDate',
                                                    fieldLabel: 'Дата решения прокурора'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 165,
                                                    name: 'ProsecutorDecNumber',
                                                    fieldLabel: 'Номер решения прокурора'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'Нарушители',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'NumberKUSP',
                                            fieldLabel: 'Номер КУСП',
                                            itemId: 'tfKUSP',
                                            maxLength: 500,
                                            labelWidth: 130
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'TypeViolator',
                                            itemId: 'cbTypeViolator',
                                            fieldLabel: 'Тип нарушителя',
                                            enumName: B4.enums.TypeViolator
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            fieldLabel: 'Территориальный признак',
                                            name: 'Municipality',
                                            store: 'B4.store.dict.Municipality',
                                            editable: false,
                                            allowBlank: false,
                                            columns: [
                                                {
                                                    text: 'Наименование', dataIndex: 'Name', flex: 1,
                                                    filter: {
                                                        xtype: 'textfield'
                                                    }
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.Contragent',
                                            textProperty: 'Name',
                                            name: 'ContragentName',
                                            fieldLabel: 'Юридическое лицо',
                                            itemId: 'sfContragentName',
                                            //disabled: true,
                                            editable: false,
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                                {
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
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 160,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'IndividualPersonId',
                                                    itemId: 'tfIndividualPersonId',
                                                    maxLength: 500,
                                                    labelWidth: 130,
                                                    hidden: true
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'ToAttracted',
                                                    fieldLabel: 'Привлечен повторно',
                                                    itemId: 'cbToAttracted',
                                                    flex: 0.3
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'Fio',
                                                    itemId: 'sfName',
                                                    editable: true,
                                                    labelWidth: 120,
                                                    labelAlign: 'right',
                                                    storeAutoLoad: false,
                                                    fieldLabel: 'ФИО',
                                                    emptyText: 'Введите ФИО...',
                                                    flex: 1,
                                                    typeAhead: false,
                                                    fields: ['Fio', 'DateBirth', 'PlaceResidence'],
                                                    url: '/Disposal/GetNameList',
                                                    mode: 'remote',
                                                    valueField: 'Fio',
                                                    displayField: 'Fio',
                                                    tpl: Ext.create('Ext.XTemplate',
                                                        '<ul class="x-list-plain"><tpl for=".">',
                                                        '<li role="option" class="x-boundlist-item">{Fio}, {DateBirth}, {PlaceResidence}</li>',
                                                        '</tpl></ul>'
                                                    ),
                                                    triggerAction: 'query',
                                                    minChars: 3,
                                                    autoSelect: true,
                                                    queryDelay: 500,
                                                    queryParam: 'filter',
                                                    loadingText: 'Загрузка...',
                                                    trigger1Cls: 'x-form-clear-trigger',
                                                    selectOnFocus: false,
                                                    allowBlank: false,
                                                    onTrigger1Click: function (field) {
                                                        this.clearValue();
                                                        this.setEditable(true);

                                                        //  me.fillAddressField();
                                                    },
                                                    //validator: function () {
                                                    //    return this.value ? true : "Выберите значение из списка";
                                                    //},
                                                    listeners: {
                                                        storebeforeload: {
                                                            fn: function (field, store, options, record, form) {

                                                                var me = this;
                                                                var editpanel = field.up('disposaleditpanel');
                                                                var selectfield = editpanel.down('#sfContragentName');
                                                                var enumcombobox = editpanel.down('#cbTypeViolator');
                                                                var Id = selectfield.getValue('Id');
                                                                var enumId = enumcombobox.getValue('Id');
                                                                
                                                                
                                                                options.params.contragentid = Id;
                                                                options.params.enumid = enumId;
                                                            },
                                                            scope: this
                                                        },
                                                        select: {
                                                            fn: function (combo, records) {
                                                                var record = records[0];
                                                                if (record) {
                                                                    var editpanel = combo.up('disposaleditpanel');

                                                                    var passportseries = editpanel.down('#dfPassportSeries');
                                                                    var passportnumber = editpanel.down('#dfPassportNumber');
                                                                    var passpoetissued = editpanel.down('#dfPassportIssued');
                                                                    var dataissue = editpanel.down('#dfDateIssue');
                                                                    var departmentcode = editpanel.down('#dfDepartmentCode');
                                                                    var databirth = editpanel.down('#tfDateBirth');
                                                                    var birthplace = editpanel.down('#tfBirthPlace');
                                                                    var placeresidence = editpanel.down('#sfPlaceResidence');
                                                                    var actuallyresidence = editpanel.down('#sfActuallyResidence');
                                                                    var job = editpanel.down('#sfTransport');
                                                                    var transport = editpanel.down('#sfTransport');

                                                                    passportseries.setValue(record.raw.PassportSeries);
                                                                    passportnumber.setValue(record.raw.PassportNumber);
                                                                    passpoetissued.setValue(record.raw.PassportIssued);
                                                                    dataissue.setValue(record.raw.DateIssue);
                                                                    departmentcode.setValue(record.raw.DepartmentCode);
                                                                    databirth.setValue(record.raw.DateBirth);
                                                                    birthplace.setValue(record.raw.BirthPlace);
                                                                    placeresidence.setValue(record.raw.PlaceResidence);
                                                                    actuallyresidence.setValue(record.raw.ActuallyResidence);
                                                                  //  job.setValue(record.raw.Job);
                                                                    transport.setValue(record.raw.Transport);

                                                                    combo.setEditable(false);
                                                   
                                                                } else {
                                                                    combo.setEditable(true);
                                                     
                                                                }
                                                                debugger;
                                                                

                                                           //    this.fillAddressField();
                                                            },
                                                            scope: this
                                                        }

                                                    }
                                                },
                                                {
                                                    xtype: 'button',
                                                    text: 'Посмотреть историю',
                                                    textAlign: 'centr',
                                                    itemId: 'btnHistory',
                                                    labelWidth: 100,
                                                    flex: 0.18
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Position',
                                                    fieldLabel: 'Должность',
                                                    itemId: 'tfPosition',
                                                    maxLength: 500,
                                                    labelWidth: 130
                                                },
                                            ],
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right',
                                               // disabled: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PassportSeries',
                                                    itemId: 'dfPassportSeries',
                                                    fieldLabel: 'Серия документа ФЛ',
                                                  //  allowBlank: true,
                                                    flex: 1,
                                                    //editable: true,
                                                    maxLength: 20
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PassportNumber',
                                                    itemId: 'dfPassportNumber',
                                                    fieldLabel: 'Номер документа ФЛ',
                                                   // allowBlank: true,
                                                    flex: 1,
                                                    //editable: true,
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PassportIssued',
                                                    itemId: 'dfPassportIssued',
                                                    fieldLabel: 'Паспорт выдан',
                                                    //allowBlank: true,
                                                    flex: 1,
                                                    //editable: true,
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateIssue',
                                                    itemId: 'dfDateIssue',
                                                    fieldLabel: 'Дата выдачи',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    //editable: true,
                                                    maxLength: 20
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DepartmentCode',
                                                    itemId: 'dfDepartmentCode',
                                                    fieldLabel: 'Код подразделения',
                                                  //  allowBlank: true,
                                                    flex: 1,
                                                    //editable: true,
                                                    maxLength: 20
                                                }, 
                                            ]
                                        },
                                        {
                                            xtype: 'datefield',
                                            fieldLabel: 'Дата рождения',
                                            name: 'DateBirth',
                                            itemId: 'tfDateBirth',
                                            maxLength: 250,
                                           // disabled: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            fieldLabel: 'Место рождения',
                                            name: 'BirthPlace',
                                            itemId: 'tfBirthPlace',
                                            maxLength: 250,
                                          //  disabled: true
                                        },
                                        { 
                                            xtype: 'b4enumcombo',
                                            name: 'FamilyStatus',
                                            itemId: 'cbFamilyStatus',
                                            fieldLabel: 'Семейное положение',
                                            enumName: B4.enums.FamilyStatus
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'PlaceResidence',
                                            itemId: 'sfPlaceResidence',
                                            fieldLabel: 'Адрес регистрации места жительства',
                                            emptyText: 'Введите Адрес...',
                                            flex: 1,
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ActuallyResidence',
                                            itemId: 'sfActuallyResidence',
                                            fieldLabel: 'Адрес фактического места жительства',
                                            emptyText: 'Введите Адрес...',
                                            flex: 1,
                                        },     
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 165,
                                                    name: 'Job',
                                                    itemId: 'sfJob',
                                                    fieldLabel: 'Место работы',
                                                    flex: 1
                                                },
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'Транспорт',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'NameTransport',
                                            fieldLabel: 'Наименование',
                                            itemId: 'tfNameTransport',
                                            maxLength: 500,
                                            labelWidth: 130
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'RegNamberTransport',
                                            fieldLabel: 'Регион регистрации номера',
                                            itemId: 'tfRegNamberTransport',
                                            maxLength: 500,
                                            labelWidth: 130
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 160,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                            ],
                                        },
                                        {
                                            xtype: 'container',
                                            margin: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right',
                                                // disabled: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'NamberTransport',
                                                    fieldLabel: 'Госномер',
                                                    itemId: 'tfNamberTransport',
                                                    maxLength: 500,
                                                    labelWidth: 130
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RegistrationNamberTransport',
                                                    fieldLabel: 'Регистр. номер',
                                                    itemId: 'tfRegistrationNamberTransport',
                                                    maxLength: 500,
                                                    labelWidth: 130
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'SeriesTransport',
                                                    fieldLabel: 'Серия номера',
                                                    itemId: 'tfSeriesTransport',
                                                    maxLength: 500,
                                                    labelWidth: 130
                                                }
                                            ]
                                        },
                                       
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [

                                            ]
                                        }
                                    ],
                                    
                                       title: 'Извещение',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'NoticeDate',
                                            fieldLabel: 'Дата явки',
                                            itemId: 'tfNoticeDate',
                                            maxLength: 250,
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NoticeTime',
                                            fieldLabel: 'Время явки',
                                            itemId: 'tfNoticeTime',
                                            maxLength: 500,
                                            labelWidth: 130
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'AddresCommission',
                                            fieldLabel: 'Адрес заседания',
                                            itemId: 'tfAddresCommission',
                                            maxLength: 500,
                                            labelWidth: 130
                                        },  
                                        {
                                            xtype: 'textfield',
                                            name: 'AddresDepartures',
                                            fieldLabel: 'Адрес оправления',
                                            itemId: 'tfAddresDepartures',
                                            maxLength: 500,
                                            labelWidth: 130
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Postcode',
                                            fieldLabel: 'Почтовый индекс',
                                            itemId: 'tfPostcode',
                                            maxLength: 500,
                                            labelWidth: 130
                                        }
                                    ]
                                },
                                    {
                                        xtype: 'fieldset',
                                        defaults: {
                                            anchor: '100%',
                                            labelWidth: 190,
                                            labelAlign: 'right'
                                        },
                                        title: 'Должностные лица',
                                        items: [
                                            {
                                                xtype: 'b4selectfield',
                                                store: 'B4.store.dict.Inspector',
                                                textProperty: 'Fio',
                                                name: 'IssuedDisposal',
                                                fieldLabel: 'Ответственный исполнитель',
                                                columns: [
                                                    { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                                    { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                                ],
                                                dockedItems: [
                                                    {
                                                        xtype: 'b4pagingtoolbar',
                                                        displayInfo: true,
                                                        store: 'B4.store.dict.Inspector',
                                                        dock: 'bottom'
                                                    }
                                                ],
                                                itemId: 'sfIssuredDisposal',
                                                allowBlank: false,
                                                editable: false
                                        },
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'disposalInspectors',
                                            itemId: 'trigFInspectors',
                                            fieldLabel: 'Члены комиссии',
                                            allowBlank: false,
                                            editable: false
                                        }
                                    ]
                                },                                
                                {
                                    xtype: 'textarea',
                                    labelWidth: 165,
                                    name: 'Description',
                                    fieldLabel: 'Описание'
                                }
                            ]
                        },
                        {
                            xtype: 'disposalViolationGrid',
                            flex: 1
                        },
                        //{
                        //    xtype: 'disposalsubjectverificationgrid',
                        //    flex: 1
                        //},
                        {
                            xtype: 'disposalexpertgrid',
                            flex: 1
                        },
                        //{
                        //    xtype: 'disposalsurveypurposegrid',
                        //    flex: 1
                        //},
                        //{
                        //    xtype: 'disposalsurveyobjectivegrid',
                        //    flex: 1
                        //},
                        //{
                        //    xtype: 'disposalinspfoundationcheckpanel',
                        //    flex: 1
                        //},
                        //{
                        //    xtype: 'disposalinspfoundationgrid',
                        //    flex: 1,
                        //    hidden: true
                        //},
                        {
                            xtype: 'disposaladminregulationgrid',
                            flex: 1
                        },
                        //{
                        //    xtype: 'disposalprovideddocgrid',
                        //    flex: 1
                        //},
                        //{
                        //    xtype: 'disposaladditionaldoc',
                        //    flex: 1
                        //},
                        //{
                        //    xtype: 'disposalcontrolmeasuresgrid',
                        //    flex: 1
                        //},
                        //{
                        //    layout: {
                        //        type: 'vbox',
                        //        align: 'stretch'
                        //    },
                        //    title: 'Уведомление о проверке',
                        //    itemId: 'tabDisposalNoticeOfInspection',
                        //    border: false,
                        //    bodyPadding: 5,
                        //    margins: -1,
                        //    frame: true,
                        //    autoScroll: true,
                        //    defaults: {
                        //        labelWidth: 200,
                        //        labelAlign: 'right'
                        //    },
                        //    items: [
                        //        {
                        //            xtype:'container',
                        //            layout: 'hbox',
                        //            defaults: {
                        //                labelWidth: 200,
                        //                labelAlign: 'right'
                        //            },
                        //            items: [
                        //                {
                        //                    xtype: 'datefield',
                        //                    name: 'NoticeDateProtocol',
                        //                    fieldLabel: 'Дата составления протокола',
                        //                    format: 'd.m.Y',
                        //                    width: 300
                        //                },
                        //                {
                        //                    fieldLabel: 'Время составления протокола',
                        //                    name: 'NoticeTimeProtocol',
                        //                    xtype: 'timefield',
                        //                    format: 'H:i',
                        //                    submitFormat: 'Y-m-d H:i:s',
                        //                    minValue: '8:00',
                        //                    maxValue: '22:00',
                        //                    width: 300
                        //                }
                        //            ]
                        //        },
                        //        {
                        //            padding: '5 0 0 0',
                        //            xtype: 'textfield',
                        //            fieldLabel: 'Место составления протокола',
                        //            name: 'NoticePlaceCreation'
                        //        },
                        //        {
                        //            xtype: 'textarea',
                        //            name: 'NoticeDescription',
                        //            fieldLabel: 'Место и время сбора представителей',
                        //            maxLength: 200,
                        //            height: 50
                        //        }
                        //    ]
                        //},
                        {
                            xtype: 'disposalannexgrid',
                            flex: 1
                        }
                        //{
                        //    xtype: 'disposaldocconfirm',
                        //    flex: 1
                        //}
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGJIToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});