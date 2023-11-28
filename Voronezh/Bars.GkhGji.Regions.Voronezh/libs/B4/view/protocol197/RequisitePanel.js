Ext.define('B4.view.protocol197.RequisitePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.protocol197RequisitePanel',
    itemId: 'requisitepanel',
    
    requires: [
        'B4.store.Contragent',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.EnumCombo',
        'B4.enums.TypeRepresentativePresence',
        'B4.store.dict.PhysicalPersonDocType',
        'B4.form.FiasSelectAddress',
        'B4.store.dict.SocialStatus',
        'B4.enums.FamilyStatus',
        'B4.store.TransportOwner',
        'B4.store.dict.Inspector',
        'B4.enums.TypeViolator'
    ],
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            bodyStyle: Gkh.bodyStyle,
            title: 'Реквизиты',
            border: false,
            bodyPadding: 5,
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            autoScroll: true,
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '5 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 170
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'ToCourt',
                            fieldLabel: 'Документы переданы в суд',
                            itemId: 'cbToCourt',
                            flex: 0.7
                        },
                        {
                            xtype: 'checkbox',
                            name: 'HasAnotherResolutions',
                            fieldLabel: 'Повторное',
                            itemId: 'cbHasAnotherResolutions',
                            flex: 0.7
                        },
                        {
                            xtype: 'textfield',
                            name: 'UIN',
                            fieldLabel: 'УИН',
                            labelWidth: 50,
                            width: 350,
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 170
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'protocolInspectors',
                            itemId: 'trigfInspector',
                            fieldLabel: 'Составители',
                            allowBlank: false,
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateToCourt',
                            fieldLabel: 'Дата передачи документов',
                            format: 'd.m.Y',
                            itemId: 'dfDateToCourt',
                            flex: 0.7
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 160,
                        labelAlign: 'right'
                    },
                    title: 'Информация о нарушителе',
                    items: [
                     
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                labelAlign: 'right',
                                disabled: false,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    itemId: 'cbExecutant',
                                    name: 'Executant',
                                    allowBlank: false,
                                    editable: false,
                                    fieldLabel: 'Тип нарушителя',
                                    fields: ['Id', 'Name', 'Code'],
                                    url: '/ExecutantDocGji/List',
                                    queryMode: 'local',
                                    triggerAction: 'all'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NumberKUSP',
                                    fieldLabel: 'Номер КУСП',
                                    itemId: 'tfKUSP',
                                    maxLength: 500,
                                    labelWidth: 130
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                labelAlign: 'right',
                                disabled: false,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Transport',
                                    fieldLabel: 'Транспортное средство',
                                    store: 'B4.store.TransportOwner',
                                    textProperty: 'NamberTransport',
                                    editable: false,
                                    flex: 1,
                                    itemId: 'sfTransportOwner',
                                    allowBlank: true,
                                    columns: [
                                        { text: 'Марка т/с', dataIndex: 'NameTransport', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Гос. номер', dataIndex: 'NamberTransport', flex: 1, filter: { xtype: 'textfield' } }

                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NameTransport',
                                    fieldLabel: 'Марка т/с',
                                    itemId: 'tfNameTransport',
                                    maxLength: 500,
                                    labelWidth: 80
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NamberTransport',
                                    fieldLabel: 'Гос. номер',
                                    itemId: 'tfNamberTransport',
                                    maxLength: 500,
                                    labelWidth: 80
                                }
                            ]
                        },
                      
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.Contragent',
                            textProperty: 'ShortName',
                            name: 'Contragent',
                            fieldLabel: 'Юридическое лицо',
                            itemId: 'sfContragent',
                            disabled: true,
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                {
                                    text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right',
                                disabled: true,
                                flex: 1
                            },
                            items: [
                                {

                                    xtype: 'b4combobox',
                                    name: 'Fio',
                                    itemId: 'sfName',
                                    editable: true,
                                    labelWidth: 180,
                                    labelAlign: 'right',
                                    storeAutoLoad: false,
                                    fieldLabel: 'ФИО',
                                    emptyText: 'Введите ФИО...',
                                    flex: 1,
                                    typeAhead: false,
                                    fields: ['Fio', 'DateBirthTxt', 'PlaceResidence', 'PassportSeries','PassportNumber'],
                                    url: '/Protocol197/GetNameList',
                                    mode: 'remote',
                                    valueField: 'Fio',
                                    displayField: 'Fio',
                                    tpl: Ext.create('Ext.XTemplate',
                                        '<ul class="x-list-plain"><tpl for=".">',
                                        '<li role="option" class="x-boundlist-item">{Fio}, {DateBirthTxt}, {PlaceResidence}, {PassportSeries}, {PassportNumber}</li>',
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
                                             
                                                var editpanel = field.up('#requisitepanel');
                                                var selectfield = editpanel.down('#sfContragent');
                                                var enumcombobox = editpanel.down('#cbExecutant');
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
                                                    var editpanel = combo.up('#requisitepanel');

                                                    var phonenumber = editpanel.down('#tfPhoneNumber');
                                                    var passportseries = editpanel.down('#dfPhysicalPersonDocumentSerial');
                                                    var passportnumber = editpanel.down('#dfPhysicalPersonDocumentNumber');
                                                    var passpoetissued = editpanel.down('#dfPassportIssued');
                                                    var dataissue = editpanel.down('#dfDateIssue');
                                                    var departmentcode = editpanel.down('#dfDepartmentCode');
                                                    var databirth = editpanel.down('#tfDateBirth');
                                                  
                                                    var birthplace = editpanel.down('#tfBirthPlace');
                                                    var protocolFiasRegistrationAddressField = editpanel.down('#protocolFiasRegistrationAddressField');
                                                    var protocolFiaFactAddressField = editpanel.down('#protocolFiaFactAddressField');
                                                    var sfSocialStatus = editpanel.down('#sfSocialStatus');
                                                    var nfDependentsNumber = editpanel.down('#nfDependentsNumber');
                                                    var job = editpanel.down('#tfJob');

                                                    var cbIsActuallyResidenceOutState = editpanel.down('#cbIsActuallyResidenceOutState');
                                                    var tfActuallyResidenceOutState = editpanel.down('#tfActuallyResidenceOutState');
                                                    var tfPlaceResidenceOutState = editpanel.down('#tfPlaceResidenceOutState');
                                                    var cbIsPlaceResidenceOutState = editpanel.down('#cbIsPlaceResidenceOutState');

                                                    phonenumber.setValue(record.raw.PhoneNumber);
                                                    passportseries.setValue(record.raw.PassportSeries);
                                                    passportnumber.setValue(record.raw.PassportNumber);
                                                    passpoetissued.setValue(record.raw.PassportIssued);
                                                    dataissue.setValue(record.raw.DateIssue);                                                 
                                                    databirth.setValue(record.raw.DateBirth);
                                                    birthplace.setValue(record.raw.BirthPlace);
                                                    nfDependentsNumber.setValue(record.raw.DependentsNumber);
                                                    job.setValue(record.raw.Job);

                                                    cbIsActuallyResidenceOutState.setValue(record.raw.IsActuallyResidenceOutState);
                                                    tfActuallyResidenceOutState.setValue(record.raw.ActuallyResidenceOutState);
                                                    tfPlaceResidenceOutState.setValue(record.raw.PlaceResidenceOutState);
                                                    cbIsPlaceResidenceOutState.setValue(record.raw.IsPlaceResidenceOutState);

                                                    if (record.raw.FiasRegistrationAddress) {
                                                        debugger;
                                                        protocolFiasRegistrationAddressField.setValue(record.raw.FiasRegistrationAddress);
                                                    }
                                                    if (record.raw.FiasFactAddress) {
                                                        protocolFiaFactAddressField.setValue(record.raw.FiasFactAddress);
                                                    }
                                                    if (record.raw.SocialStatus) {
                                                        sfSocialStatus.setValue(record.raw.SocialStatus);
                                                    }
                                
                                             

                                                    combo.setEditable(false);
                                                    debugger;
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
                                    xtype: 'textfield',
                                    name: 'PersonPosition',
                                    fieldLabel: 'Должность',
                                    itemId: 'tfPosition',
                                    maxLength: 500,
                                    labelWidth: 130
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата рождения',
                                    name: 'DateBirth',
                                    itemId: 'tfDateBirth',
                                    flex: 0.5,
                                    maxLength: 250,
                                    // disabled: true
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Место рождения',
                                    name: 'BirthPlace',
                                    flex: 1,
                                    itemId: 'tfBirthPlace',
                                    maxLength: 250,
                                    //  disabled: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right',
                                disabled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'IsPlaceResidenceOutState',
                                    itemId: 'cbIsPlaceResidenceOutState',
                                    flex: 0.5,
                                    fieldLabel: 'Регистрация за пределами субъекта'
                                },
                                {
                                    //Адрес места правонаружения
                                    xtype: 'b4fiasselectaddress',
                                    flex: 3,
                                    labelWidth: 220,
                                    labelAlign: 'right',
                                    name: 'FiasRegistrationAddress',
                                    itemId: 'protocolFiasRegistrationAddressField',
                                    fieldLabel: 'Адрес регистрации нарушителя',
                                    fieldsRegex: {
                                        tfHousing: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        },
                                        tfBuilding: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        }
                                    }
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PlaceResidenceOutState',
                                    itemId: 'tfPlaceResidenceOutState',
                                    hidden: true,
                                    fieldLabel: 'Адрес регистрации',
                                    labelWidth: 120,
                                    maxLength: 500,
                                    flex: 3,
                                    allowBlank: true,
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right',
                                disabled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'IsActuallyResidenceOutState',
                                    itemId: 'cbIsActuallyResidenceOutState',
                                    flex: 0.5,
                                    fieldLabel: 'Место фактического пребывания за пределами субъекта'
                                },
                                {
                                    //Адрес места правонаружения
                                    xtype: 'b4fiasselectaddress',
                                    flex: 3,
                                    labelAlign: 'right',
                                    name: 'FiasFactAddress',
                                    labelWidth: 220,
                                    itemId: 'protocolFiaFactAddressField',
                                    fieldLabel: 'Адрес факт. места жительства',
                                    fieldsRegex: {
                                        tfHousing: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        },
                                        tfBuilding: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    name: 'CopyButtonFactAddress',
                                    itemId: 'btnCopyButtonFactAddress',
                                    text: 'Заполнить',
                                    flex: 0.5,
                                    margin: '0 0 0 10'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ActuallyResidenceOutState',
                                    itemId: 'tfActuallyResidenceOutState',
                                    fieldLabel: 'Адрес места жительства',
                                    labelWidth: 120,
                                    maxLength: 500,
                                    hidden: true,
                                    flex: 3,
                                    allowBlank: true,
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right',
                                disabled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'PhysicalPersonDocType',
                                    fieldLabel: 'Вид документа',
                                    store: 'B4.store.dict.PhysicalPersonDocType',
                                    editable: false,
                                    flex: 1,
                                    itemId: 'dfPhysicalPersonDocType',
                                    allowBlank: true,
                                    columns: [
                                        { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }

                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPersonDocumentSerial',
                                    itemId: 'dfPhysicalPersonDocumentSerial',
                                    fieldLabel: 'Серия документа',
                                    allowBlank: true,
                                    flex: 1,
                                    //editable: true,
                                    maxLength: 20
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPersonDocumentNumber',
                                    itemId: 'dfPhysicalPersonDocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    allowBlank: true,
                                    flex: 1,
                                    //editable: true,
                                    maxLength: 20
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'dfPhysicalPersonIsNotRF',
                                    name: 'PhysicalPersonIsNotRF',
                                    fieldLabel: 'Не является гражданином РФ',
                                    allowBlank: true,
                                    flex: 1
                                    //editable: true
                                }
                            ]
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
                                    name: 'PassportIssued',
                                    itemId: 'dfPassportIssued',
                                    fieldLabel: 'Выдан',
                                    allowBlank: true,
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
                                //{
                                //    xtype: 'textfield',
                                //    name: 'DepartmentCode',
                                //    itemId: 'dfDepartmentCode',
                                //    fieldLabel: 'Код подразделения',
                                //    //  allowBlank: true,
                                //    flex: 1,
                                //    //editable: true,
                                //    maxLength: 20
                                //},
                            ]
                        },
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'FamilyStatus',
                                    fieldLabel: 'Семейное положение',
                                    itemId: 'ecFamilyStatus',
                                    width: 450,
                                    minWidth: 450,
                                    enumName: B4.enums.FamilyStatus
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Job',
                                    itemId: 'tfJob',
                                    flex: 1,
                                    fieldLabel: 'Место работы',
                                    maxLength: 1000,
                                    allowBlank: true,
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhoneNumber',
                                    itemId: 'tfPhoneNumber',
                                    maxLength: 100,
                                    flex: 1,
                                    fieldLabel: 'Контактный телефон',
                                    allowBlank: true
                                }
                            ]
                        }, 
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 180,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    itemId: 'sfSocialStatus',
                                    name: 'SocialStatus',
                                    flex:1,
                                    fieldLabel: 'Социальный статус',
                                    store: 'B4.store.dict.SocialStatus',
                                    editable: false
                                },
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    maxValue: 20,
                                    flex: 0.5,
                                    itemId: 'nfDependentsNumber',
                                    name: 'DependentsNumber',
                                    fieldLabel: 'Количество иждевенцев'
                                },
                            ]
                        }
                    ]
                },                
                {
                    xtype: 'gkhtriggerfield',
                    name: 'surveySubjectRequirements',
                    itemId: 'trigfSurveySubjectRequirements',
                    hidden:true,
                    fieldLabel: 'Перечень требований'
                },
                {
                    xtype: 'fieldset',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 160,
                        labelAlign: 'right'
                    },
                    shrinkWrap: true,
                    hidden: true,
                    title: 'Уведомление о времени и месте составления протокола',
                    items: [
                        {
                            xtype: 'checkbox',
                            fieldLabel: 'Вручено через канцелярию',
                            name: 'NotifDeliveredThroughOffice',
                            itemId: 'cbNotifDeliveredThroughOffice'
                        },
                        {
                            xtype: 'datefield',
                            name: 'FormatDate',
                            itemId: 'dfNotifDeliveryDate',
                            fieldLabel: 'Дата вручения (регистрации) уведомления',
                            labelWidth: 275,
                            disabled: true
                        },
                        {
                            xtype: 'numberfield',
                            itemId: 'nfNotifNum',
                            name: 'NotifNumber',
                            fieldLabel: 'Номер регистрации',
                            disabled: true,
                            hideTrigger: true
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Рассмотрение',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 160,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateOfProceedings',
                                    fieldLabel: 'Дата и время расмотрения дела:',
                                    format: 'd.m.Y',
                                    allowBlank:false,
                                    labelWidth: 200,
                                    width: 330
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'HourOfProceedings',
                                    margin: '0 0 0 10',
                                    fieldLabel: '',
                                    labelWidth: 25,
                                    width: 45,
                                    maxValue: 23,
                                    minValue: 0
                                },
                                {
                                    xtype: 'label',
                                    text: ':',
                                    margin: '5'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'MinuteOfProceedings',
                                    width: 45,
                                    maxValue: 59,
                                    minValue: 0
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ProceedingsPlace',
                                    itemId: 'tfProceedingsPlace',
                                    disabled: true,
                                    labelWidth: 180,
                                    flex:1,
                                    fieldLabel: 'Место рассмотрения дела',
                                    maxLength: 1000
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'ProceedingCopyNum',
                                    fieldLabel: 'Количество экземпляров',
                                    hidden:true,
                                    hideTrigger: true,
                                    flex: 1,
                                    labelWidth: 160,
                                    minValue: 0
                                }
                            ]
                        },           
                        {
                            xtype: 'b4selectfield',
                            name: 'ComissionMeeting',
                            fieldLabel: 'Комиссия',
                            disabled: true,
                            store: 'B4.store.comission.ComissionMeeting',
                            textProperty: 'ComissionName',
                            editable: false,
                            flex: 1,
                            itemId: 'dfComissionMeeting',
                            allowBlank: true,
                            columns: [
                                { text: 'Наименование', dataIndex: 'ComissionName', flex: 1, filter: { xtype: 'textfield' } },
                                { xtype: 'datecolumn', text: 'Дата комиссии', dataIndex: 'CommissionDate', flex: 1, format: 'd.m.Y', filter: { xtype: 'datefield', operand: CondExpr.operands.eq } }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Проведение комиссии',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 160,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            margin: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'TypePresence',
                                    fieldLabel: 'В присутствии/отсутствии',
                                    itemId: 'ecTypePresence',
                                    width: 450,
                                    minWidth: 450,
                                    enumName: B4.enums.TypeRepresentativePresence
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Representative',
                                    fieldLabel: 'Представитель',
                                    itemId: 'tfRepresentative',
                                    maxLength: 500,
                                    disabled: true,
                                    flex: 1,
                                    labelWidth: 100
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'ReasonTypeRequisites',
                            itemId: 'taReasonTypeRequisites',
                            maxLength: 1000,
                            disabled: true,
                            fieldLabel: 'Вид и реквизиты основания'
                        },
                        {
                            xtype: 'textarea',
                            name: 'Remarks',
                            fieldLabel: 'Объяснения нарушителя',
                            maxLength: 1000
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});