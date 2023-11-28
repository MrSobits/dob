﻿Ext.define('B4.view.normconsumption.HotWaterGrid', {
    extend: 'Ext.form.Panel',
    alias: 'widget.normconshotwatergrid',
    bodyStyle: Gkh.bodyStyle,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.store.normconsumption.NormConsumption',
        'B4.enums.YesNo',
        'B4.form.EnumCombo',
        'B4.store.dict.PeriodNormConsumption'
    ],

    title: 'Сведения по нормативам потребления ГВС',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.normconsumption.NormConsumptionHotWater');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    margin: 5,
                    border: null,
                    padding: 0,
                    layout: {
                        type: 'hbox',
                        align: 'strecth'
                    },
                    items: [
                        
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'leftstrecth'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Municipality',
                                    readOnly: true,
                                    editable: true,
                                    fieldLabel: 'Муниципальный район',
                                    labelWidth: 175,
                                    width: 300
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Period',
                                    readOnly: true,
                                    editable: true,
                                    fieldLabel: 'Период',
                                    labelWidth: 125,
                                    width: 300
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Норматив потребления',
                                    enumName: 'B4.enums.NormConsumptionType',
                                    name: 'Type',
                                    readOnly: true,
                                    labelWidth: 175,
                                    width: 400
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'gridpanel',
                    store: store,
                    cls: 'x-large-head',
                    columnLines: true,
                    enableLocking: true,

                    flex: 1,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Municipality',
                            text: 'Муниципальный район',
                            locked: true,
                            width: 200,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Address',
                            text: 'Адрес',
                            locked: true,
                            width: 200,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'FloorNumber',
                            text: 'Количество этажей',
                            lockable: false,
                            width: 70,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'MetersInstalled',
                            width: 70,
                            header: 'Наличие приборов учета',
                            renderer: function (val) {
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'BuildYear',
                            text: 'Год постройки',
                            width: 70,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Общая площадь дома, кв.м.',
                            dataIndex: 'AreaHouse',
                            width: 70,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            dataIndex: 'AreaLivingRooms',
                            text: 'Общая площадь жилых помещений (квартиры), кв.м.',
                            width: 70,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            dataIndex: 'AreaNotLivingRooms',
                            text: 'Общая площадь нежилых помещений, кв.м.',
                            width: 70,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            dataIndex: 'AreaOtherRooms',
                            text: 'Общая площадь помещений, входящих в состав имущества, кв.м.',
                            width: 70,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'IsIpuNotLivingPermises',
                            width: 70,
                            header: 'Наличие отдельного ипу у нежилых помещений',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            dataIndex: 'AreaIpuNotLivingPermises',
                            text: 'Площадь нежилых помещений при наличии отдельного ипу у нежилых помещений, кв.м.',
                            width: 70,
                            editor: {
                                xtype: 'numberfield'
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            dataIndex: 'VolumeHotWaterNotLivingIsIpu',
                            text: 'Объем ГВС по нежилым помещениям при наличии ипу, куб.м.',
                            width: 70,
                            editor: {
                                xtype: 'numberfield'
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            dataIndex: 'VolumeWaterOpuOnPeriod',
                            text: 'Для домов с ОПУ Объем воды по общедомовому прибору за отопительный период куб.м',
                            width: 80,
                            editor: {
                                xtype: 'numberfield'
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            dataIndex: 'HeatingPeriod',
                            text: 'Отопительный период, дни',
                            width: 80,
                            editor: {
                                xtype: 'numberfield',
                                allowDecimals: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                return val;
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'TypeSystemHotWater',
                            text: 'Вид системы горячего водоснабжения (централизованная, ИТП)',
                            width: 200
                        },
                        {
                            dataIndex: 'ResidentsNumber',
                            text: 'Количество проживающих',
                            width: 80
                        },
                        {
                            dataIndex: 'DepreciationIntrahouseUtilities',
                            text: 'Износ внутридомовых инженерных сетей',
                            width: 90
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'OverhaulDate',
                            text: 'Дата проведения капитального ремонта',
                            format: 'd.m.Y',
                            width: 80
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'IsBath1200',
                            width: 120,
                            header: 'МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами, раковинами, мойками, ваннами сидячими длиной 1200 мм с душем',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'IsBath1500With1550',
                            width: 120,
                            header: 'МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами, раковинами, мойками, ваннами длиной 1500 - 1550 мм с душем',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'IsBath1650With1700',
                            width: 120,
                            header: 'МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами, раковинами, мойками, ваннами длиной 1650 - 1700 мм с душем',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'IsBathNotShower',
                            width: 120,
                            header: 'МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами, раковинами, мойками, ваннами без душа',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'IsShower',
                            width: 120,
                            header: 'МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами, раковинами, мойками, ваннами без душа',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'SharedShowerInHostel',
                            width: 120,
                            header: 'Дома, использующиеся в качестве общежитий с общими душевыми',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'IsHostelShowerAllLivPermises',
                            width: 120,
                            header: 'Дома, использующиеся в качестве общежитий с душами при всех жилых комнатах',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-' },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },


                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'ShowerInHostelInSection',
                            width: 120,
                            header: 'Дома, использующиеся в качестве общежитий с общими кухнями и блоками душевых на этажах при жилых комнатах в каждой секции здания',
                            editor: {
                                xtype: 'b4combobox',
                                valueField: 'Value',
                                displayField: 'Display',
                                emptyItem: { Name: '-', Display: '', Value: 0 },
                                store: B4.enums.YesNo.getStore(),
                                editable: false
                            },
                            renderer: function(val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNo',
                            dataIndex: 'Gvs12Floor',
                            width: 150,
                            text: 'В жилых домах квартирного типа с водопроводом, с центральной или местной (выгреб) канализацией и централизованным ГВС высотой свыше 12 этажей с централизованным ГВС и повышенным требованиям к их благоустройству',
                            renderer: function (val, meta) {
                                meta.style = 'background-color:#fefee0;';
                                if (val == 0) {
                                    return null;
                                }
                                return B4.enums.YesNo.displayRenderer(val);
                            }
                        }
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: store,
                            dock: 'bottom'
                        }
                    ],
                    normalGridConfig: {
                        plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                         Ext.create('Ext.grid.plugin.CellEditing', {
                             clicksToEdit: 1,
                             pluginId: 'cellEditing'
                         })]
                    },
                    lockedGridConfig: {
                        plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')]
                    },
                    viewConfig: {
                        loadMask: true
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
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
                                text: 'Печать',
                                action: 'Export',
                                iconCls: 'icon-page-excel'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    handler: function () {
                                        this.up('normconshotwatergrid').close();
                                    }
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