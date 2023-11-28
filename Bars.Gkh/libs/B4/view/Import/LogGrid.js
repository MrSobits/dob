Ext.define('B4.view.Import.LogGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.Import.Log'
    ],

    title: 'Логи загрузок',
    alias: 'widget.importLogGrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.Import.Log');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Operator',
                    flex: 1,
                    text: 'Логин',
                    filter: {
                        xtype: 'textfield',
                        flex: 1
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'UploadDate',
                    width: 100,
                    text: 'Дата загрузки',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileName',
                    flex: 1,
                    text: 'Наименование файла',
                    filter: {
                        xtype: 'textfield',
                        flex: 1
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountImportedFile',
                    flex: 1,
                    text: 'Кол-во файлов',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ImportKey',
                    width: 250,
                    text: 'Тип импорта',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Key',
                        displayField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/GkhImport/GetImportList'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountWarning',
                    flex: 1,
                    text: 'Кол-во предупреждений',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountError',
                    flex: 1,
                    text: 'Кол-во ошибок',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountImportedRows',
                    flex: 1,
                    text: 'Кол-во импортированных строк',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountChangedRows',
                    flex: 1,
                    text: 'Кол-во изменнных строк',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        operand: CondExpr.operands.eq
                    }
                },
                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    flex: 1,
                    text: 'Файлы',
                    renderer: function (value) {
                        var fileId = value.Id,
                            url;
                        if (fileId > 0) {
                            url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', fileId));
                            return '<a href="' + url + '" target="_blank" style="color: black">Скачать</a>';
                        }
                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LogFile',
                    flex: 1,
                    text: 'Лог',
                    renderer: function (value) {
                        var fileId = value.Id,
                            url;
                        if (fileId > 0) {
                            url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', fileId));
                            return '<a href="' + url + '" target="_blank" style="color: black">Скачать</a>';
                        }
                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LogFile',
                    flex: 1,
                    text: 'Лог с открытого сервера',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileTransport/GetFileFromPublicServer?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
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