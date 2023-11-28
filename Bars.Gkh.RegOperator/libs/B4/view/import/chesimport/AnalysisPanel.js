Ext.define('B4.view.import.chesimport.AnalysisPanel', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.enums.regop.FileType',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Delete',
        'B4.view.Control.GkhButtonPrint'
    ],

    title: 'Разбор файла',
    alias: 'widget.chesimportanalysisgrid',

    columnLines: true,
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('Ext.data.ArrayStore', { fields: [ 'fileType' ] });

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'fileType',
                    text: 'Тип файла',
                    enumName: 'B4.enums.regop.FileType',
                    flex: 1,
                    filter: false
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                                    xtype: 'button',
                                    text: 'Загрузить данные в систему',
                                    iconCls: 'icon-application-go',
                                    action: 'import'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});