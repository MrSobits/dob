Ext.define('B4.view.longtermprobject.propertyownerprotocols.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.propertyownerprotocolseditwin',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.enums.PropertyOwnerProtocolType'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    bodyPadding: 5,
    itemId: 'propertyownerprotocolsEditWindow',
    title: 'Редактирование',

    initComponent: function() {
        var me = this;

        var contragentStore = Ext.create('B4.store.Contragent', {
            proxy: {
                type: 'b4proxy',
                controllerName: 'Contragent',
                listAction: 'GetAllActiveContragent'
            }
        });

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'TypeProtocol',
                    items: B4.enums.PropertyOwnerProtocolType.getItems(),
                    displayField: 'Display',
                    valueField: 'Value',
                    fieldLabel: 'Тип протокола',
                    editable: false
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер'
                        },
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата',
                            maxValue: new Date(),
                            validator: function (v) {
                                if (this.parseDate(v) > this.maxValue) {
                                    return 'Выбранная дата больше текущей даты';
                                }

                                return true;
                            }
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'DocumentFile',
                    fieldLabel: 'Файл',
                    itemId: 'ffDocumentFile',
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Описание',
                    name: 'Description'
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Сумма займа',
                    name: 'LoanAmount',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ','
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Borrower',
                    fieldLabel: 'Заемщик',
                    store: contragentStore,
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальный район',
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
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Lender',
                    fieldLabel: 'Кредитор',
                    store: contragentStore,
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальный район',
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
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'fieldset',
                    padding: '5',
                    title: 'Количественные характеристики',
                    name: 'Parameters',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                allowBlank: false,
                                decimalSeparator: ',',
                                minValue: 0,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    labelWidth: 180,
                                    name: 'NumberOfVotes',
                                    fieldLabel: 'Количество голосов (кв.м.)'
                                },
                                {
                                    labelWidth: 220,
                                    name: 'TotalNumberOfVotes',
                                    fieldLabel: 'Общее количество голосов (кв.м.)'
                                }
                            ]
                        },
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            labelWidth: 180,
                            labelAlign: 'right',
                            name: 'PercentOfParticipating',
                            fieldLabel: 'Доля принявших участие (%)',
                            anchor: '50%',
                            allowBlank: false,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            decimalSeparator: ',',
                            minValue: 0
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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