Ext.define('B4.view.comission.ComissionsGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.doccomissiongrid',

    requires: [
        'B4.ux.button.Add',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'Ext.ux.grid.FilterBar',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.form.ComboBox',
        'B4.enums.ComissionDocumentDecision',
        'B4.form.SelectField',
    ],

    store: 'comission.ListComissions',
    title: 'Заседания комиссий',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'actioncolumn',
                    width: 50,
                    items: [{
                        tooltip: 'Переход к комиссии',
                        icon: B4.Url.content('content/img/icons/arrow_right.png'),
                        handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                            debugger;
                            me.fireEvent('gotocomussion', rec);
                        }
                    }]

                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ComissionDocumentDecision',
                    dataIndex: 'ComissionDocumentDecision',
                    flex: 1,
                    text: 'Решение',
                    filter: true
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус комиссии',
                    width: 100,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'adm_comission_meeting';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ZonalInspection',
                    text: 'Комиссия',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommissionNumber',
                    text: 'Номер',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CommissionDate',
                    text: 'Дата комиссии',
                    format: 'd.m.Y',
                    flex: 1,
                    hideable: false,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ComissionName',
                    text: 'Наименование',
                    flex: 1,
                    filter: { xtype: 'textfield' }
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
                            xtype: 'b4updatebutton'
                        },     
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