Ext.define('B4.view.person.QualificationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.personqualificationgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.person.QualificationCertificate',
        'B4.enums.TypeCancelationQualCertificate'
    ],

    title: 'Квалификационные аттестаты',
    
    closable: true,

    // необходимо для того чтобы неработали восстановления для грида посколкьу колонки показываются и скрываются динамически
    provideStateId: Ext.emptyFn,
    stateful: false,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.person.QualificationCertificate');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    width: 100,
                    text: 'Номер'
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'FullName',
                //    width: 100,
                //    text: 'ФИО'
                //},
                {
                    xtype: 'datecolumn',
                    dataIndex: 'IssuedDate',
                    text: 'Дата выдачи',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RequestToExamName',
                    width: 150,
                    text: 'Заявка на доступ к экзамену'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    text: 'Дата окончания действия',
                    format: 'd.m.Y',
                    width: 150
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeCancelation',
                    flex: 2,
                    text: 'Основание аннулирования',
                    renderer: function (val) {
                        if (val) {
                            return B4.enums.TypeCancelationQualCertificate.displayRenderer(val);
                        }
                        return "";
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CancelationDate',
                    text: 'Дата аннулирования',
                    format: 'd.m.Y',
                    width: 150
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
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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