﻿Ext.define('B4.view.protocol197.AnnexGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.button.Sign',
        'B4.enums.TypeAnnex',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.MessageCheck'
    ],

    alias: 'widget.protocol197AnnexGrid',
    title: 'Приложения',
    store: 'protocol197.Annex',
    itemId: 'protocol197AnnexGrid',

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
                    xtype: 'b4enumcolumn',
                    anchor: '100%',
                    dataIndex: 'TypeAnnex',
                    text: 'Тип документа',
                    enumName: 'B4.enums.TypeAnnex',
                    name: 'TypeAnnex',
                    filter: true
                    
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsProof',
                    width: 100,
                    text: 'Доказательство',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
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
                    xtype: 'gridcolumn',
                    dataIndex: 'SignedFile',
                    width: 100,
                    text: 'Подписанный файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileTransport/GetFileFromPrivateServer?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    anchor: '100%',
                    dataIndex: 'MessageCheck',
                    text: 'Статус отправки',
                    enumName: 'B4.enums.MessageCheck',
                    name: 'MessageCheck',
                    filter: true

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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4signbutton',
                                    disabled: true
                                },
                                {
                                    xtype: 'button',
                                    text: 'Добавить массово',
                                    iconCls: 'icon-add',
                                    itemId: 'massAddition'
                                },
                            ]
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