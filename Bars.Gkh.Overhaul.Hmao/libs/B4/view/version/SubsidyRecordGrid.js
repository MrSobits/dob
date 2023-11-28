﻿Ext.define('B4.view.version.SubsidyRecordGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.versionsubsidyrecordgrid',
    requires: [
        'B4.store.subsidy.SubsidyRecord'
        //'B4.grid.feature.Summary'
    ],

    cls: 'x-large-head',
    title: 'Субсидирование',
    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.subsidy.SubsidyRecord');

        var groupingFeature = Ext.create('Ext.grid.feature.Grouping', {
            groupHeaderTpl: ' ',
            collapsible: false
        });

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            features: [
                groupingFeature
            ],
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SubsidyYear',
                    flex: 1,
                    sortable: false,
                    text: 'Год',
                    width: 80,
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    },
                    renderer: function (value, meta, rec) {
                        var result = value;
                        if (value == 0 && rec.get('IsSummary')) {
                            result = 'Итого';
                        }
                        return result;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanOwnerCollection',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Плановая сумма сбора средств собственников, руб.',
                    renderer: me.showValue,
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanOwnerPercent',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    text: 'Плановая собираемость, %',
                    renderer: me.showValue
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NotReduceSizePercent',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    text: 'Неснижаемый размер регионального фонда, %',
                    renderer: me.showValue
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerSumForCr',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Средства собственников на капитальный ремонт, руб.',
                    renderer: function (value, meta, rec) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetRegion',
                    flex: 1,
                    text: 'Региональный бюджет, руб.',
                    tdCls: 'b-editable-spec',
                    sortable: false,
                    minWidth: 120,
                    renderer: me.showValue
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetMunicipality',
                    flex: 1,
                    tdCls: 'b-editable-spec',
                    text: 'Бюджет муниципальных образований, руб.',
                    sortable: false,
                    minWidth: 120,
                    renderer: me.showValue
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetFcr',
                    flex: 1,
                    text: 'Средства ГК ФСР ЖКХ, руб.',
                    sortable: false,
                    minWidth: 120,
                    tdCls: 'b-editable-spec',
                    renderer: me.showValue
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetOtherSource',
                    flex: 1,
                    text: 'Средства иных источников, руб.',
                    tdCls: 'b-editable-spec',
                    sortable: false,
                    minWidth: 120,
                    renderer: me.showValue
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BudgetCr',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Итоговый бюджет на капитальный ремонт, руб',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CorrectionFinance',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Расчетная потребность в финансировании капитального ремонта, руб',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    dataIndex: 'BalanceAfterCr',
                    xtype: 'gridcolumn',
                    sortable: false,
                    flex: 1,
                    minWidth: 120,
                    text: 'Остаток  средств после проведения КР на конец года, руб',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    var result = 'x-subsidy-row';
                    if (record.get('IsShortTerm')) {
                        result += ' x-subsidy-spec-row';
                    }
                    if (record.get('IsSummary')) {
                        result = 'x-summary';
                    }
                    return result;
                }
            }
        });

        me.callParent(arguments);
    },   
    
    showValue: function (value, meta, rec) {
        if (rec.get('IsShortTerm')) {
            return Ext.util.Format.currency(value, null, 2);
        }
        return "-";
    }
});