﻿Ext.define('B4.view.constructionobject.document.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.form.field.ComboBox',
        'B4.form.ComboBox'
    ],

    title: 'Документация',
    alias: 'widget.constructionobjectdocumentgrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.constructionobject.Document');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Type',
                    text: 'Тип документа',
                    enumName: 'B4.enums.ConstructionObjectDocumentType',
                    filter: true
                },
                {
                    dataIndex: 'Name',
                    text: 'Документ',
                    flex: 1
                },
                {
                    xtype:'datecolumn',
                    dataIndex: 'Date',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 150
                },
                {
                    dataIndex: 'Contragent',
                    text: 'Участник',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});