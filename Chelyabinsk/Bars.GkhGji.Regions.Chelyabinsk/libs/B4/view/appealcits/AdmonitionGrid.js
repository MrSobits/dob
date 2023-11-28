﻿Ext.define('B4.view.appealcits.AdmonitionGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.admonitiongrid',

    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
    ],
    closable: true,
    title: 'Реестр предостережений',
    store: 'appealcits.Admonition',
    itemId: 'admonitiongrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
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
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес МКД',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Контрагент',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executor',
                    flex: 1,
                    text: 'Член Комиссии',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'ДЛ издавшее предостережение',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerfomanceDate',
                    flex: 1,
                    text: 'Срок исполнения',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerfomanceFactDate',
                    flex: 1,
                    text: 'Дата факт. исполнения',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    var planDate = record.get('PerfomanceDate');
                    var factDate = record.get('PerfomanceFactDate');
                    var currentdate = new Date();
                    var planDateDt = new Date(planDate);
                    var datetime = currentdate.getFullYear() + "-" + currentdate.getDate();

                    if (planDateDt <= currentdate && factDate === null) {
                        
                        return 'back-red';
                    }
                  
                    return '';
                }
            },
            dockedItems: [
                    {
                        xtype: 'toolbar',
                        dock: 'top',
                        items: [
                            {
                                xtype: 'b4updatebutton'
                            },
                            {
                                xtype: 'button',
                                iconCls: 'icon-table-go',
                                text: 'Экспорт',
                                textAlign: 'left',
                                itemId: 'btnExport'
                            }
                        ]
                    },
                    {
                        xtype: 'b4pagingtoolbar',
                        displayInfo: true,
                        store: this.store,
                        dock: 'bottom'
                    }
            ]
        });

        me.callParent(arguments);
    }
});