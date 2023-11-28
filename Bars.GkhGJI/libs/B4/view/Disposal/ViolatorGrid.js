Ext.define('B4.view.disposal.ViolatorGrid', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.disposalViolatorGrid',
    requires: [
        'B4.form.FiasSelectAddress',
        'B4.store.Contragent',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.EnumCombo',
        'B4.enums.TypeExecutantProtocolMvd',
        'B4.enums.TypeRepresentativePresence',
        'B4.store.dict.PhysicalPersonDocType',
        'B4.enums.TypeViolator',
        'B4.enums.TypeAddress',
        'B4.enums.PlaceOffense',
        'B4.store.dict.JurInstitution'
    ],

    store: 'disposal.Violator',
    itemId: 'disposalViolatorGrid',
    title: 'Нарушители',

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
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 160,
                        labelAlign: 'right'
                    },
                    items: [
                        this.Violator = Ext.create('B4.form.EnumCombo', {
                            editable: false,
                            allowBlank: false,
                            name: 'TypeViolator',
                            itemId: 'cbTypeViolator',
                            fieldLabel: 'Тип нарушителя',
                            enumName: B4.enums.TypeViolator
                        }),
                        //{
                        //    xtype: 'b4enumcombo',
                        //    name: 'TypeViolator',
                        //    itemId: 'cbTypeViolator',
                        //    fieldLabel: 'Тип нарушителя',
                        //    enumName: B4.enums.TypeViolator
                        //},
                        //{
                        //    xtype: 'b4combobox',
                        //    itemId: 'cbExecutant',
                        //    name: 'Executant',
                        //    allowBlank: false,
                        //    editable: false,
                        //    fieldLabel: 'Тип исполнителя',
                        //    fields: ['Id', 'Name', 'Code'],
                        //    url: '/ExecutantDocGji/List',
                        //    queryMode: 'local',
                        //    triggerAction: 'all'
                        //},
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
                                labelWidth: 160,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                this.cbCity = Ext.create('B4.form.ComboBox', {
                                    editable: true,
                                    labelWidth: 120,
                                    labelAlign: 'right',
                                    storeAutoLoad: false,
                                    fieldLabel: 'ФИО',
                                    emptyText: 'Введите ФИО...',
                                    flex: 1,
                                    typeAhead: false,
                                    fields: ['GuidId', 'Code', 'Name', 'AddressName', 'PostCode', 'AddressGuid', 'MirrorGuid'],
                                    url: '/Fias/GetPlacesList',
                                    mode: 'remote',
                                    valueField: 'AddressGuid',
                                    displayField: 'AddressName',
                                    triggerAction: 'query',
                                    minChars: 3,
                                    autoSelect: true,
                                    queryDelay: 500,
                                    queryParam: 'filter',
                                    loadingText: 'Загрузка...',
                                    trigger1Cls: 'x-form-clear-trigger',
                                    selectOnFocus: false,
                                    allowBlank: false,
                                    onTrigger1Click: function () {
                                        this.clearValue();
                                        this.setEditable(true);
                                      
                                      //  me.fillAddressField();
                                    },
                                    validator: function () {
                                        return this.value ? true : "Выберите значение из списка";
                                    },
                                    listeners: {
                                        storebeforeload: {
                                            fn: function (field, store, options) {
                                                var rec = this.Violator.getRecord(this.Violator.getValue());
                                                if (rec) {
                                                    options.params.parentguid = rec.Name;
                                                }
                                                return !Ext.isEmpty(options.params.parentguid);
                                            },
                                            scope: this
                                        },
                                        select: {
                                            fn: function (combo, records) {
                                                var record = records[0];
                                                if (record) {
                                                    if (record.data.PostCode) {
                                                        this.tfPostCode.setValue(record.data.PostCode);
                                                    }
                                                    if (combo.oldValue != record.data.GuidId && this.cbStreet.getValue()) {
                                                        this.cbStreet.clearValue();
                                                        this.cbStreet.store.removeAll();
                                                    }

                                                    combo.setEditable(false);
                                                    this.cbStreet.setEditable(true);
                                                } else {
                                                    combo.setEditable(true);
                                                    this.cbStreet.setEditable(false);
                                                }

                                                this.fillAddressField();
                                            },
                                            scope: this
                                        }
                                    }
                                }),
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPersonPosition',
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
                                disabled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'PhysicalPersonDocType',
                                    fieldLabel: 'Вид документа ФЛ',
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
                                    fieldLabel: 'Серия документа ФЛ',
                                    allowBlank: true,
                                    flex: 1,
                                    //editable: true,
                                    maxLength: 20
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPersonDocumentNumber',
                                    itemId: 'dfPhysicalPersonDocumentNumber',
                                    fieldLabel: 'Номер документа ФЛ',
                                    allowBlank: true,
                                    flex: 1,
                                    //editable: true,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPersonDocument',
                                    itemId: 'dfPhysicalPersonDocument',
                                    fieldLabel: 'Паспорт выдан',
                                    allowBlank: true,
                                    flex: 1,
                                    //editable: true,
                                    maxLength: 300
                                }, 
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPersonDocumentCode',
                                    itemId: 'dfPhysicalPersonDocumentCode',
                                    fieldLabel: 'Код подразделения',
                                    allowBlank: true,
                                    flex: 1,
                                    //editable: true,
                                    maxLength: 20
                                },
                                //{
                                //    xtype: 'checkbox',
                                //    itemId: 'dfPhysicalPersonIsNotRF',
                                //    name: 'PhysicalPersonIsNotRF',
                                //    fieldLabel: 'Не является гражданином РФ',
                                //    allowBlank: true,
                                //    flex: 1
                                //    //editable: true
                                //}
                            ]
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Дата рождения',
                            name: 'PersonDateBirth',
                            itemId: 'tfPersonDateBirth',
                            maxLength: 250,
                            disabled: true
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Место рождения',
                            name: 'PersonBirthDatePlace',
                            itemId: 'tfDatePlaceOfBirth',
                            maxLength: 250,
                            disabled: true
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Адрес регистрации места жительства',
                            name: 'PersonRegistrationAddress',
                            itemId: 'tfRegistrationAddress',
                            maxLength: 250,
                            disabled: true
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Адрес фактического места жительства',
                            name: 'PersonFactAddress',
                            itemId: 'tfFactAddress',
                            maxLength: 250,
                            disabled: true
                        },
                        //{
                        //    xtype: 'container',
                        //    margin: '0 0 5 0',
                        //    layout: 'hbox',
                        //    defaults: {
                        //        labelWidth: 160,
                        //        labelAlign: 'right'
                        //    },
                        //    items: [
                        //        {
                        //            xtype: 'b4enumcombo',
                        //            name: 'TypePresence',
                        //            fieldLabel: 'В присутствии/отсутствии',
                        //            itemId: 'ecTypePresence',
                        //            width: 450,
                        //            minWidth: 450,
                        //            enumName: B4.enums.TypeRepresentativePresence
                        //        },
                        //        {
                        //            xtype: 'textfield',
                        //            name: 'Representative',
                        //            fieldLabel: 'Представитель',
                        //            itemId: 'tfRepresentative',
                        //            maxLength: 500,
                        //            disabled: true,
                        //            flex: 1,
                        //            labelWidth: 100
                        //        }
                        //    ]
                        //},
                        //{
                        //    xtype: 'textarea',
                        //    name: 'ReasonTypeRequisites',
                        //    itemId: 'taReasonTypeRequisites',
                        //    maxLength: 1000,
                        //    disabled: true,
                        //    fieldLabel: 'Вид и реквизиты основания'
                        //}
                    ]
                },
            ],
            viewConfig: {


            },
        });

        me.callParent(arguments);
    }
});